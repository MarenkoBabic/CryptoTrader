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
                TradeHistoryListViewModel vm = new TradeHistoryListViewModel();
                Ticker dbTicker = new Ticker();
                using (var db = new CryptoTraderEntities())
                {
                    Person dbPerson = db.Person.Where(a => a.email == User.Identity.Name).FirstOrDefault();
                    if (db.TradeHistory.Any())
                    {
                        vm.HistoryList = db.TradeHistory.Where(a => a.person_id == dbPerson.id).ToList();
                    }
                }
                return View();

            }
            else
            {
                TempData["ErrorMessage"] = "Sie müssen ein einloggen";
                return RedirectToAction("Index", "Home");
            }
        }
        /// <summary>
        /// Bitcoin kaufen
        /// </summary>
        /// <param name="vm">ViewModel</param>
        /// <param name="id">IdAuswahl ob per Anzahl Bitcoins oder Euro</param>
        /// <returns>View mit neuen Kontostand</returns>
        [HttpPost]
        public ActionResult TradeByBTC(TradeByBitCoinViewModel vm, string submit)
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
                    bool result = TradeManager.HaveEnoughMoney(vm.BalanceAmount, vm.TickerRate, vm.TradeAmountBTC);
                    if (result)
                    {
                        dbBalance.amount += TradeManager.TradeAmountByBTC(vm.TickerRate, vm.TradeAmountBTC);

                        dbTradeHistory.person_id = dbPerson.id;
                        dbTradeHistory.amount = vm.TradeAmountBTC;

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
                    bool result = TradeManager.HaveEnoughBTC(ShowBitCoin(), vm.TradeAmountBTC);
                    if (result)
                    {
                        dbBalance.amount -= TradeManager.TradeAmountByBTC(vm.TickerRate, vm.TradeAmountBTC);

                        dbTradeHistory.amount = vm.TradeAmountBTC * (-1);
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
        /// Bitcoins verkaufen
        /// </summary>
        /// <param name="vm">Viewmodel</param>
        /// <param name="id">IdAuswahl ob per Anzahl Bitcoins oder Euro</param>
        /// <returns>View mit neuen Kontostand</returns>
        [HttpPost]
        public ActionResult TradeByEuro(TradeByEuroViewModel vm, int id)
        {

            using (var db = new CryptoTraderEntities())
            {
                vm.TickerRate = db.Ticker.OrderByDescending(a => a.id).Select(a => a.rate).First();
                vm.TickerId = db.Ticker.OrderByDescending(a => a.created).Select(a => a.id).First();
                Person dbPerson = db.Person.Where(a => a.email.Equals(User.Identity.Name)).FirstOrDefault();
                Balance dbBalance = db.Balance.Where(a => a.person_id == dbPerson.id).FirstOrDefault();
                TradeHistory dbTradeHistory = Mapper.Map<TradeHistory>(vm);

                //Kaufe BitCoins
                if (id == 1)
                {
                    bool result = TradeManager.HaveEnoughBTC(ShowBitCoin(), vm.TradeAmountEuro);
                    if (result)
                    {


                        dbBalance.amount -= vm.TradeAmountEuro;

                        dbTradeHistory.amount = TradeManager.GetBTCAmount(vm.TradeAmountEuro, vm.TickerRate);
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

                //Verkaufe Bitcoins
                else
                {
                    if (dbBalance.amount >= vm.TradeAmountEuro)
                    {
                        dbTradeHistory.person_id = dbPerson.id;
                        dbTradeHistory.amount = TradeManager.GetBTCAmount(vm.TradeAmountEuro, vm.TickerRate);

                        dbBalance.amount -= vm.TradeAmountEuro;
                        dbBalance.created = vm.Created;


                        if (ModelState.IsValid)
                        {
                            db.Entry(dbBalance).State = EntityState.Modified;
                            db.TradeHistory.Add(dbTradeHistory);
                            db.SaveChanges();
                        }
                        return View(vm);
                    }
                    TempData["ErrorMessage"] = "Limit überschritten laden sie Ihr Konto auf";
                    return RedirectToAction("Index");
                }
            }

            return View("Index");
        }

        /// <summary>
        /// Zeigt die Anzahl wieviel der Kunde BitCoin hat
        /// </summary>
        /// <returns>Decimal BitCoin</returns>
        public decimal ShowBitCoin()
        {
            TradeByBitCoinViewModel vm = new TradeByBitCoinViewModel();
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
                    return PartialView(@"~/Views/Trade/_TradeHistoryList.cshtml",vm);
                }
                return null;
            }
        }

    }
}