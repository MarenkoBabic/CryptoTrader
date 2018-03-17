namespace CryptoTrader.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using CryptoTrader.Manager;
    using CryptoTrader.Model.DbModel;
    using CryptoTrader.Model.ViewModel;

    public class TradeController : Controller
    {
        public ActionResult Index()
        {

            bool result = ValidationController.IsUserAuthenticated(User.Identity.IsAuthenticated);
            if (result)
            {
                return View();
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
        /// <param name="vm">ViewModel</param>
        /// <param name="submit">Submit eingabe</param>
        /// <returns>View</returns>
        [HttpPost]
        public ActionResult Trade(BuyBitCoinViewModel vm, string submit)
        {
            using (var db = new CryptoTraderEntities())
            {
                vm.TickerRate = db.Ticker.OrderByDescending(a => a.id).Select(a => a.rate).First();
                vm.TickerId = db.Ticker.OrderByDescending(a => a.created).Select(a => a.id).First();
                Person dbPerson = db.Person.Where(a => a.email.Equals(User.Identity.Name)).FirstOrDefault();
                Balance dbBalance = db.Balance.Where(a => a.person_id == dbPerson.id).FirstOrDefault();
                TradeHistory dbTradeHistory = Mapper.Map<TradeHistory>(vm);

                //Kontostand
                if (db.Balance.Any())
                {
                    vm.BalanceAmount = db.Balance.Where(a => a.person_id == dbPerson.id).FirstOrDefault().amount;
                }
                else
                {
                    vm.BalanceAmount = 0.0m;
                }

                //Bitcoin Kaufen
                if (submit == "Kaufen")
                {
                    bool result = TradeManager.HaveEnoughMoney(vm.BalanceAmount, vm.TickerRate, vm.BuyTradeAmountBTC);
                    if (result)
                    {
                        dbBalance.amount -= TradeManager.TradeAmountByBTC(vm.TickerRate, vm.BuyTradeAmountBTC);

                        dbTradeHistory.person_id = dbPerson.id;
                        dbTradeHistory.amount = vm.BuyTradeAmountBTC;

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
                    bool result = TradeManager.HaveEnoughBTC(ShowBitCoin(), vm.BuyTradeAmountBTC);
                    if (result)
                    {
                        dbBalance.amount += TradeManager.TradeAmountByBTC(vm.TickerRate, vm.BuyTradeAmountBTC);

                        dbTradeHistory.amount = vm.BuyTradeAmountBTC * (-1);
                        dbTradeHistory.person_id = dbPerson.id;

                        if (ModelState.IsValid)
                        {
                            db.Entry(dbBalance).State = EntityState.Modified;
                            db.TradeHistory.Add(dbTradeHistory);
                            db.SaveChanges();
                        }
                        return RedirectToAction("Index");

                    }
                    TempData["ErrorMessage"] = "Nicht genug BitCoin vorhanden";

                }
                return View("Index");
            }
        }

        /// <summary>
        /// Zeigt die Anzahl wieviel der Kunde BitCoin hat
        /// </summary>
        /// <returns>Decimal BitCoin</returns>
        public decimal ShowBitCoin()
        {
            BuyBitCoinViewModel vm = new BuyBitCoinViewModel();
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
        /// Befüllt die TradeHistorylist aus der Datenbank
        /// </summary>
        /// <returns>TradeHistoryList</returns>
        public ActionResult TradeHistoryList()
        {
            Ticker dbTicker = new Ticker();
            TradeHistoryListViewModel vm = new TradeHistoryListViewModel();
            using (var db = new CryptoTraderEntities())
            {
                Person dbPerson = db.Person.Where(a => a.email == User.Identity.Name).FirstOrDefault();
                bool result = db.TradeHistory.Any(a => a.person_id == dbPerson.id);
                if (result)
                {
                    foreach (TradeHistory item in db.TradeHistory.Where(a => a.person_id == dbPerson.id))
                    {
                        item.ticker_id = dbTicker.id;
                        vm.HistoryList.Add(item);
                    }
                    return PartialView(@"~/Views/Trade/_TradeHistoryList.cshtml", vm);
                }
                return null;
            }
        }

        /// <summary>
        /// Berechnet den Euro Wert
        /// </summary>
        /// <param name="TradeAmountBTC">Anzahl Bitcoins</param>
        /// <returns>Euro</returns>
        public ActionResult GetEuro(decimal? TradeAmountBTC)
        {
            using (var db = new CryptoTraderEntities())
            {
                decimal dbTicker = db.Ticker.OrderByDescending(a => a.id).Select(a => a.rate).First();

                TradeAmountBTC = TradeAmountBTC * dbTicker;

                return Json(TradeAmountBTC.ToString(), JsonRequestBehavior.AllowGet);

            }
        }

        /// <summary>
        /// Berechnet die Bitcoin Anzahl
        /// </summary>
        /// <param name="TradeAmountEuro">Euro</param>
        /// <returns>BitCoin</returns>
        public ActionResult GetBTC(decimal? TradeAmountEuro)
        {
            using (var db = new CryptoTraderEntities())
            {
                decimal dbTicker = db.Ticker.OrderByDescending(a => a.id).Select(a => a.rate).First();

                TradeAmountEuro = TradeAmountEuro / dbTicker;

                return Json(TradeAmountEuro.ToString(), JsonRequestBehavior.AllowGet);

            }
        }


    }
}