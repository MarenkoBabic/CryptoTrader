namespace CryptoTrader.Controllers
{
    using AutoMapper;
    using CryptoTrader.Manager;
    using CryptoTrader.Model.DbModel;
    using CryptoTrader.Model.ViewModel;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;

    public class TradeController : Controller
    {

        /// <summary>
        /// Befüllt die Historylist und gibt sie zum View mit
        /// </summary>
        /// <returns>TradeHistoryList</returns>
        public ActionResult Index()
        {
            bool result = ValidationController.IsUserAuthenticated(User.Identity.IsAuthenticated);
            if (result)
            {
                using (var db = new CryptoTraderEntities())
                {
                    TradeViewModel tradeVM = new TradeViewModel();
                    Person dbPerson = db.Person.Where(a => a.email == User.Identity.Name).FirstOrDefault();
                    tradeVM.HistoryList = null;

                    if (db.BankTransferHistory.Any(a => a.person_id == dbPerson.id))
                    {
                        tradeVM.HistoryList = FillList.GetTradeHistoryList(dbPerson.id);
                        tradeVM.FirstName = dbPerson.firstName;
                        return View(tradeVM);
                    }
                    return View(tradeVM);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Sie müssen eingeloggt sein";
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Bitcoin Handeln
        /// </summary>
        /// <param name="vm">TradeBitCoinViewModel</param>
        /// <param name="submit">SubmitButton TradeIndex</param>
        /// <returns>View</returns>
        [HttpPost]
        public ActionResult Trade(TradeViewModel tradeVM, string submit)
        {
            if (decimal.Parse(tradeVM.EuroTrade) > 0.0m || decimal.Parse(tradeVM.BtcTrade) > 0)
            {
                using (var db = new CryptoTraderEntities())
                {
                    Person dbPerson = db.Person.Where(a => a.email.Equals(User.Identity.Name)).FirstOrDefault();
                    Balance dbBalance = db.Balance.Where(a => a.person_id == dbPerson.id).FirstOrDefault();
                    TradeHistory dbTradeHistory = Mapper.Map<TradeHistory>(tradeVM);

                    tradeVM.TickerRate = db.Ticker.OrderByDescending(a => a.id).Select(a => a.rate).First();

                    bool haveBalanceData = db.Balance.Any(a => a.person_id == dbPerson.id);
                    //Kontostand
                    if (haveBalanceData)
                    {
                        tradeVM.BalanceAmount = db.Balance.Where(a => a.person_id == dbPerson.id).FirstOrDefault().amount;
                    }
                    else
                    {
                        tradeVM.BalanceAmount = 0.0m;
                    }

                    //Bitcoin Kaufen
                    if (submit == "buy")
                    {
                        bool result = TradeManager.HaveEnoughMoney(tradeVM.BalanceAmount, tradeVM.TickerRate, decimal.Parse(tradeVM.BtcTrade));
                        if (result)
                        {
                            dbBalance.amount -= TradeManager.TradeAmountByBTC(tradeVM.TickerRate, decimal.Parse(tradeVM.BtcTrade));
                            dbTradeHistory.amount = decimal.Parse(tradeVM.BtcTrade);

                            dbTradeHistory.person_id = dbPerson.id;
                            if (ModelState.IsValid)
                            {
                                db.Entry(dbBalance).State = EntityState.Modified;
                                db.TradeHistory.Add(dbTradeHistory);
                                db.SaveChanges();
                            }
                            return RedirectToAction("Index");
                        }
                        TempData["ErrorMessage"] = "Limit überschritten laden sie Ihr Konto auf";
                    }

                    //Bitcoin verkaufen
                    else
                    {
                        bool result = TradeManager.HaveEnoughBTC(dbTradeHistory.amount, decimal.Parse(tradeVM.BtcTrade));
                        if (result)
                        {
                            dbBalance.amount += TradeManager.TradeAmountByBTC(tradeVM.TickerRate, decimal.Parse(tradeVM.BtcTrade));

                            dbTradeHistory.person_id = dbPerson.id;
                            dbTradeHistory.amount = decimal.Parse(tradeVM.BtcTrade) * (-1);
                            db.Entry(dbBalance).State = EntityState.Modified;
                            db.TradeHistory.Add(dbTradeHistory);
                            db.SaveChanges();
                            return RedirectToAction("Index");

                        }
                        TempData["ErrorMessage"] = "Nicht genug BitCoin vorhanden";

                    }
                    return RedirectToAction("Index", decimal.Parse(tradeVM.BtcTrade));
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Betrag eingeben";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Zeigt die Anzahl wieviel der Kunde BitCoin hat
        /// </summary>
        /// <returns>Decimal BitCoin</returns>
        public decimal ShowBitCoin()
        {
            TradeViewModel vm = new TradeViewModel();
            using (var db = new CryptoTraderEntities())
            {
                Person dbPerson = db.Person.Where(a => a.email == User.Identity.Name).FirstOrDefault();
                bool result = db.TradeHistory.Any(a => a.person_id == dbPerson.id);
                if (result)
                {
                    foreach (TradeHistory item in db.TradeHistory.Where(a => a.person_id == dbPerson.id))
                    {
                        vm.BitCoinAmount += item.amount;
                    }
                    return Math.Round(vm.BitCoinAmount, 2);
                }
                return 00.00m;
            }
        }

        /// <summary>
        /// Berechnet den Euro Wert
        /// </summary>
        /// <param name="TradeAmountBTC">Anzahl Bitcoins</param>
        /// <returns>Euro</returns>
        public ActionResult GetEuro(TradeViewModel vm)
        {
            decimal.TryParse(vm.BtcTrade, out decimal BTC);
            if (BTC > 0)
            {
                using (var db = new CryptoTraderEntities())
                {
                    decimal dbTicker = db.Ticker.OrderByDescending(a => a.id).Select(a => a.rate).First();
                    vm.EuroTrade = (dbTicker * BTC).ToString();
                    return Json(Math.Round(decimal.Parse(vm.EuroTrade), 6).ToString(), JsonRequestBehavior.AllowGet);
                }
            }
            return Json("0,0", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Berechnet die Bitcoin Anzahl
        /// </summary>
        /// <param name="TradeAmountEuro">Euro</param>
        /// <returns>BitCoin Anzahl</returns>
        public ActionResult GetBTC(TradeViewModel vm)
        {
            decimal.TryParse(vm.EuroTrade, out decimal Euro);
            if (Euro > 0)
            {
                using (var db = new CryptoTraderEntities())
                {
                    decimal dbTicker = db.Ticker.OrderByDescending(a => a.id).Select(a => a.rate).First();

                    vm.BtcTrade = (Euro / dbTicker).ToString();
                    return Json(Math.Round(decimal.Parse(vm.BtcTrade),6).ToString(), JsonRequestBehavior.AllowGet);
                }
            }
            return Json("0,0", JsonRequestBehavior.AllowGet);
        }
    }
}