namespace CryptoTrader.Controllers
{
    using System.Web.Mvc;
    using CryptoTrader.Models.ViewModel;
    using CryptoTrader.Models.DbModel;
    using System.Collections.Generic;
    using System.Linq;
    using System.Globalization;
    using System;

    public class TradeController : Controller
    {
        // GET: Trade
        public ActionResult Index()
        {
            string rate = Manager.ApiKraken.ShowRate();
            Ticker dbTicker = new Ticker();
            TradeViewModel vm = new TradeViewModel();
            using(var db = new CryptoTraderEntities() )
            {
                dbTicker.created = vm.Created;
                dbTicker.currency_src = "1";
                dbTicker.currency_trg = "Eur";
                dbTicker.rate = Math.Round(Convert.ToDecimal(rate),2);
                Person dbPerson = db.Person.Where( a => a.email == User.Identity.Name ).FirstOrDefault();
                vm.HistoryList = db.TradeHistory.Where(a => a.person_id == dbPerson.id).ToList();
            }

            return View(vm);
        }
        [HttpPost]
        public ActionResult Index( TradeViewModel vm )
        {
            return View();
        }
    }
}