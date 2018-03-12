namespace CryptoTrader.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using CryptoTrader.Manager;
    using CryptoTrader.Model.DbModel;
    using CryptoTrader.Model.ViewModel;

    public class TradeController : Controller
    {
        // GET: Ansicht
        public ActionResult BuyBitCoin()
        {
            bool result = ValidationController.IsUserAuthenticated(User.Identity.IsAuthenticated);
            if (result)
            {
                Ticker dbTicker = new Ticker();
                SellBitCoinViewModel vm = new SellBitCoinViewModel();
                using (var db = new JaroshEntities())
                {
                    Person dbPerson = db.Person.Where(a => a.email == User.Identity.Name).FirstOrDefault();
                    vm.HistoryList = db.TradeHistory.Where(a => a.person_id == dbPerson.id).ToList();
                    vm.TickerRate = db.Ticker.OrderByDescending(a => a.id).Select(a => a.rate).First();
                    vm.BitCoinAmount = vm.HistoryList.Sum(a => a.amount);
                }
                return View(vm);

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
        public ActionResult BuyBitCoin(SellBitCoinViewModel vm, int id)
        {

            using (var db = new JaroshEntities())
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

                if (id == 1)
                {
                    bool result = TradeManager.HaveEnoughMoney(vm.BalanceAmount, vm.TickerRate, vm.TradeAmount);
                    if (result)
                    {
                        decimal resultAmount = TradeManager.BuyBitCoin(vm.TickerRate, vm.TradeAmount);
                        dbBalance.amount -= resultAmount * (-1);

                        dbTradeHistory.person_id = dbPerson.id;

                        if (ModelState.IsValid)
                        {
                            db.Entry(dbBalance).State = EntityState.Modified;
                            db.TradeHistory.Add(dbTradeHistory);
                            db.SaveChanges();
                        }
                        return RedirectToAction("BuyBitCoin");
                    }
                    TempData["ErrorMessage"] = "Limit überschritten laden sie Ihr Konto auf";
                }

                else
                {
                    if (dbBalance.amount >= vm.EuroAmount)
                    {
                        decimal btcAmount = TradeManager.BuyBitCoinPerEuro(vm.TickerRate, vm.EuroAmount);
                        dbTradeHistory.person_id = dbPerson.id;

                        dbBalance.amount -= vm.EuroAmount;
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
                    return RedirectToAction("BuyBitCoin");
                }
                return View();
            }
        }

        public ActionResult SellBitCoin()
        {
            bool result = ValidationController.IsUserAuthenticated(User.Identity.IsAuthenticated);
            if (result)
            {
                Ticker dbTicker = new Ticker();
                SellBitCoinViewModel vm = new SellBitCoinViewModel();
                using (var db = new JaroshEntities())
                {
                    Person dbPerson = db.Person.Where(a => a.email == User.Identity.Name).FirstOrDefault();
                    if (db.TradeHistory.Any())
                    {
                        vm.HistoryList = db.TradeHistory.Where(a => a.person_id == dbPerson.id).ToList();
                    }
                    vm.TickerRate = db.Ticker.OrderByDescending(a => a.id).Select(a => a.rate).First();

                }
                return View(vm);

            }
            else
            {
                TempData["ErrorMessage"] = "Sie müssen ein einloggen";
                return RedirectToAction("Index", "Home");
            }

        }

        /// <summary>
        /// Bitcoins verkaufen
        /// </summary>
        /// <param name="vm">Viewmodel</param>
        /// <param name="id">IdAuswahl ob per Anzahl Bitcoins oder Euro</param>
        /// <returns>View mit neuen Kontostand</returns>
        [HttpPost]
        public ActionResult SellBitCoin(SellBitCoinViewModel vm, int id)
        {

            using (var db = new JaroshEntities())
            {
                vm.TickerRate = db.Ticker.OrderByDescending(a => a.id).Select(a => a.rate).First();
                vm.TickerId = db.Ticker.OrderByDescending(a => a.created).Select(a => a.id).First();
                Person dbPerson = db.Person.Where(a => a.email.Equals(User.Identity.Name)).FirstOrDefault();
                Balance dbBalance = db.Balance.Where(a => a.person_id == dbPerson.id).FirstOrDefault();
                TradeHistory dbTradeHistory = Mapper.Map<TradeHistory>(vm);


                if (id == 1)
                {
                    bool result = TradeManager.HaveEnoughBTC(ShowBitCoin(), vm.TradeAmount);
                    if (result)
                    {
                        decimal resultAmount = TradeManager.SellBitCoin(vm.TickerRate, vm.TradeAmount);
                        
                        dbBalance.amount -= resultAmount * (-1);

                        dbTradeHistory.amount = vm.TradeAmount *(-1);
                        dbTradeHistory.person_id = dbPerson.id;

                        if (ModelState.IsValid)
                        {
                            db.Entry(dbBalance).State = EntityState.Modified;
                            db.TradeHistory.Add(dbTradeHistory);
                            db.SaveChanges();
                        }
                        return RedirectToAction("SellBitCoin");

                    }
                    TempData["ErrorMessage"] = "Nicht genug BitCoin vorhanden";
                }
                else
                {
                    bool result = TradeManager.HaveEnoughBTC(dbTradeHistory.amount, vm.EuroAmount);
                    if (result)
                    {
                        vm.BalanceAmount = TradeManager.SellBitCoinPerEuro(vm.TickerRate, dbTradeHistory.amount);
                        if (ModelState.IsValid)
                        {
                            db.Entry(dbBalance).State = EntityState.Modified;
                            db.TradeHistory.Add(dbTradeHistory);
                            db.SaveChanges();
                        }
                        return RedirectToAction("SellBitCoin");
                    }
                    TempData["ErrorMessage"] = "Nicht genug BitCoin vorhanden";
                }
            }

            return View();
        }

        public decimal ShowBitCoin()
        {
            SellBitCoinViewModel vm = new SellBitCoinViewModel();
            using (var db = new JaroshEntities())
            {
                Person dbPerson = db.Person.Where(a => a.email == User.Identity.Name).FirstOrDefault();
                bool result = db.TradeHistory.Any(a => a.person_id == dbPerson.id);
                if (result)
                {

                    var history = db.TradeHistory.Where(a => a.person_id == dbPerson.id).ToList();
                    foreach (TradeHistory item in history)
                    {
                        vm.BitCoinAmount += item.amount;
                    }
                    return Math.Round(vm.BitCoinAmount, 2);
                }
                return 00.00m;
            }
        }

    }
}