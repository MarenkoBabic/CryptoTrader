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
        /// <returns>View mit ViewModel</returns>
        public ActionResult Index()
        {
            bool result = ValidationController.IsUserAuthenticated(User.Identity.IsAuthenticated);
            if (result)
            {
                TradeBitCoinViewModel vm = new TradeBitCoinViewModel();
                using (var db = new CryptoTraderEntities())
                {
                    Person dbPerson = db.Person.Where(a => a.email == User.Identity.Name).FirstOrDefault();
                    bool haveHistory = db.TradeHistory.Any(a => a.person_id == dbPerson.id);

                    if (haveHistory)
                    {
                        vm.HistoryList = db.TradeHistory.Where(a => a.person_id == dbPerson.id).Include(a => a.Ticker).OrderByDescending(a => a.created).ToList();
                        return View(vm);
                    }
                    else
                    {
                        vm.HistoryList = null;
                        return View(vm);
                    }
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Sie müssen ein einloggen";
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
        public ActionResult Trade(TradeBitCoinViewModel vm, string submit)
        {
            if (vm.EuroTrade > 0 || vm.BtcTrade > 0)
            {
                using (var db = new CryptoTraderEntities())
                {
                    vm.TickerRate = db.Ticker.OrderByDescending(a => a.id).Select(a => a.rate).First();
                    vm.TickerId = db.Ticker.OrderByDescending(a => a.created).Select(a => a.id).First();
                    Person dbPerson = db.Person.Where(a => a.email.Equals(User.Identity.Name)).FirstOrDefault();
                    Balance dbBalance = db.Balance.Where(a => a.person_id == dbPerson.id).FirstOrDefault();
                    TradeHistory dbTradeHistory = Mapper.Map<TradeHistory>(vm);
                    bool haveBalanceData = db.Balance.Any(a => a.person_id == dbPerson.id);
                    //Kontostand
                    if (haveBalanceData)
                    {
                        vm.BalanceAmount = db.Balance.Where(a => a.person_id == dbPerson.id).FirstOrDefault().amount;
                    }
                    else
                    {
                        vm.BalanceAmount = 0.0m;
                    }

                    //Bitcoin Kaufen
                    if (submit == "buy")
                    {
                        bool result = TradeManager.HaveEnoughMoney(vm.BalanceAmount, vm.TickerRate, vm.BtcTrade);
                        if (result)
                        {
                            dbBalance.amount -= TradeManager.TradeAmountByBTC(vm.TickerRate, vm.BtcTrade);

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
                        bool result = TradeManager.HaveEnoughBTC(ShowBitCoin(), vm.BtcTrade);
                        if (result)
                        {
                            dbBalance.amount += TradeManager.TradeAmountByBTC(vm.TickerRate, vm.BtcTrade);

                            dbTradeHistory.person_id = dbPerson.id;
                            db.Entry(dbBalance).State = EntityState.Modified;
                            db.TradeHistory.Add(dbTradeHistory);
                            db.SaveChanges();
                            return RedirectToAction("Index");

                        }
                        TempData["ErrorMessage"] = "Nicht genug BitCoin vorhanden";

                    }
                    return RedirectToAction("Index", vm);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Betrag eingeben";
                return RedirectToAction("Index", vm);
            }
        }

        /// <summary>
        /// Zeigt die Anzahl wieviel der Kunde BitCoin hat
        /// </summary>
        /// <returns>Decimal BitCoin</returns>
        public decimal ShowBitCoin()
        {
            TradeBitCoinViewModel vm = new TradeBitCoinViewModel();
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
        public ActionResult GetEuro(TradeBitCoinViewModel vm)
        {
            Regex regex = new Regex(@"[\d]{1,4}([.\,[\d]{1,2})?");
            if (regex.IsMatch(vm.EuroTrade.ToString()))
            {
                using (var db = new CryptoTraderEntities())
                {
                    decimal dbTicker = db.Ticker.OrderByDescending(a => a.id).Select(a => a.rate).First();

                    vm.EuroTrade = dbTicker * vm.BtcTrade;
                    return Json(vm.EuroTrade, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Berechnet die Bitcoin Anzahl
        /// </summary>
        /// <param name="TradeAmountEuro">Euro</param>
        /// <returns>BitCoin Anzahl</returns>
        public ActionResult GetBTC(decimal? EuroTrade)
        {
            Regex regex = new Regex(@"[\d]{1,4}([.\,[\d]{1,2})?");
            if (regex.IsMatch(EuroTrade.ToString()))
            {
                using (var db = new CryptoTraderEntities())
                {
                    decimal dbTicker = db.Ticker.OrderByDescending(a => a.id).Select(a => a.rate).First();

                    EuroTrade = EuroTrade / dbTicker;
                    return Json(EuroTrade, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}