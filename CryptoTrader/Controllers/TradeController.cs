namespace CryptoTrader.Controllers
{
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
            Ticker dbTicker = new Ticker();
            SellBitCoinViewModel vm = new SellBitCoinViewModel();
            using (var db = new CryptoTraderEntities())
            {
                Person dbPerson = db.Person.Where(a => a.email == User.Identity.Name).FirstOrDefault();
                vm.HistoryList = db.TradeHistory.Where(a => a.person_id == dbPerson.id).ToList();
                vm.TickerRate = db.Ticker.OrderByDescending(a => a.id).Select(a => a.rate).First();
                vm.BitCoinAmount = vm.HistoryList.Sum(a => a.amount);
            }
            return View(vm);
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
            TradeHistory dbTradeHistory = Mapper.Map<TradeHistory>(vm);

            using (var db = new CryptoTraderEntities())
            {
                Ticker Ticker = db.Ticker.FirstOrDefault();
                Person dbPerson = db.Person.Where(a => a.email.Equals(User.Identity.Name)).FirstOrDefault();
                Balance dbBalance = db.Balance.Where(a => a.person_id == dbPerson.id).FirstOrDefault();

                //Kontostand
                if (db.Balance.Any())
                {
                    vm.BalanceAmount = db.Balance.Where(a => a.person_id == dbPerson.id).FirstOrDefault().amount;
                }
                else
                {
                    vm.BalanceAmount = 0.0m;
                }
                //Bitcoin Kurs
                vm.TickerRate = db.Ticker.OrderByDescending(a => a.id).Select(a => a.rate).First();

                dbTradeHistory.person_id = dbPerson.id;
                dbTradeHistory.ticker_id = Ticker.id;
                if (id == 1)
                {
                    bool result = TradeManager.HaveEnoughMoney(vm.BalanceAmount, vm.TickerRate, vm.TradeAmount);
                    if (result)
                    {
                        decimal tempAmount = TradeManager.BuyBitCoin(vm.BalanceAmount, vm.TickerRate, vm.TradeAmount);
                        dbBalance.amount = vm.BalanceAmount + (tempAmount);
                        var dbBalance2 = new Balance()
                        {
                            created = vm.Created,
                            amount = tempAmount,
                            currency = "Eur",
                            person_id = dbPerson.id,



                        };
                        if (ModelState.IsValid)
                        {
                            db.Entry(dbBalance).State = EntityState.Modified;
                            db.Balance.Add(dbBalance2);
                            db.TradeHistory.Add(dbTradeHistory);
                            db.SaveChanges();
                        }
                        return View(vm);
                    }
                    TempData["ErrorMessage"] = "Limit überschritten laden sie Ihr Konto auf";
                }
                else
                {
                    bool result = TradeManager.HaveEnoughMoney(dbTradeHistory.amount, vm.TickerRate, vm.EuroAmount);
                    if (result)
                    {
                        vm.BalanceAmount = TradeManager.BuyBitCoinPerEuro(vm.BalanceAmount, vm.TickerRate, dbTradeHistory.amount);


                        if (ModelState.IsValid)
                        {
                            db.Entry(dbBalance).State = EntityState.Modified;
                            db.TradeHistory.Add(dbTradeHistory);
                            db.SaveChanges();
                        }
                        return View(vm);
                    }
                    TempData["ErrorMessage"] = "Limit überschritten laden sie Ihr Konto auf";
                    return View("Index", vm);
                }
                return View();
            }
        }


        public ActionResult SellBitCoin()
        {
            Ticker dbTicker = new Ticker();
            SellBitCoinViewModel vm = new SellBitCoinViewModel();
            using (var db = new CryptoTraderEntities())
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

        /// <summary>
        /// Bitcoins verkaufen
        /// </summary>
        /// <param name="vm">Viewmodel</param>
        /// <param name="id">IdAuswahl ob per Anzahl Bitcoins oder Euro</param>
        /// <returns>View mit neuen Kontostand</returns>
        [HttpPost]
        public ActionResult SellBitCoin(SellBitCoinViewModel vm, int id)
        {
            TradeHistory dbTradeHistory = Mapper.Map<TradeHistory>(vm);

            using (var db = new CryptoTraderEntities())
            {
                Person dbPerson = db.Person.Where(a => a.email.Equals(User.Identity.Name)).FirstOrDefault();
                Balance dbBalance = db.Balance.Where(a => a.person_id == dbPerson.id).FirstOrDefault();

                vm.BalanceAmount = db.Balance.Where(a => a.person_id == dbPerson.id).FirstOrDefault().amount;


                Ticker Ticker = db.Ticker.FirstOrDefault();
                dbTradeHistory.person_id = dbPerson.id;
                dbTradeHistory.ticker_id = Ticker.id;
                vm.TickerRate = db.Ticker.OrderByDescending(a => a.id).Select(a => a.rate).First();

                if (id == 1)
                {
                    bool result = TradeManager.HaveEnoughBTC(dbTradeHistory.amount, vm.TradeAmount);
                    if (result)
                    {
                        vm.BalanceAmount = TradeManager.SellBitCoin(vm.BalanceAmount, vm.TickerRate, vm.TradeAmount);
                        dbBalance.amount = vm.BalanceAmount;
                        if (ModelState.IsValid)
                        {
                            db.Entry(dbBalance).State = EntityState.Modified;
                            db.TradeHistory.Add(dbTradeHistory);
                            db.SaveChanges();
                        }
                        return View(vm);
                    }
                    TempData["ErrorMessage"] = "Nicht genug BitCoin vorhanden";
                }
                else
                {
                    bool result = TradeManager.HaveEnoughBTC(dbTradeHistory.amount, vm.EuroAmount);
                    if (result)
                    {
                        vm.BalanceAmount = TradeManager.SellBitCoinPerEuro(vm.BalanceAmount, vm.TickerRate, dbTradeHistory.amount);
                        if (ModelState.IsValid)
                        {
                            db.Entry(dbBalance).State = EntityState.Modified;
                            db.TradeHistory.Add(dbTradeHistory);
                            db.SaveChanges();
                        }
                        return View(vm);
                    }
                    TempData["ErrorMessage"] = "Nicht genug BitCoin vorhanden";
                }
            }

            return View();
        }
    }
}