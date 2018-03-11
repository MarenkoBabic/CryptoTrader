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
        // GET: Trade
        public ActionResult Index()
        {
            Ticker dbTicker = new Ticker();
            TradeViewModel vm = new TradeViewModel();
            using (var db = new CryptoTraderEntities())
            {
                Person dbPerson = db.Person.Where(a => a.email == User.Identity.Name).FirstOrDefault();
                vm.HistoryList = db.TradeHistory.Where(a => a.person_id == dbPerson.id).ToList();
                vm.TickerRate = db.Ticker.OrderByDescending(a => a.id).Select(a => a.rate).First();

            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(TradeViewModel vm, int id)
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
                    bool result = TradeManager.HaveEnoughMoney(vm.BalanceAmount, vm.TickerRate, vm.TradeAmountBTC);
                    if (result)
                    {
                        decimal tempAmount = TradeManager.BuyBitCoin(vm.BalanceAmount, vm.TickerRate, vm.TradeAmountBTC);
                        dbBalance.amount = vm.BalanceAmount +(tempAmount);
                        var dbBalance2 = new Balance() {
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

                else if (id == 2)
                {
                    bool result = TradeManager.HaveEnoughBTC(dbTradeHistory.amount, vm.TradeAmountBTC);
                    if (result)
                    {
                        vm.BalanceAmount = TradeManager.SellBitCoin(vm.BalanceAmount, vm.TickerRate, vm.TradeAmountBTC);
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

                else if (id == 3)
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
                }

                else if (id == 4)
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

                return View("Index", vm);
            }
        }


    }
}