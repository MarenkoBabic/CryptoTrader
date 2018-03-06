namespace CryptoTrader.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using CryptoTrader.Manager;
    using CryptoTrader.Model.DbModel;
    using CryptoTrader.Model.ViewModel;
    using CryptoTrader.Models.ViewModel;
    using Jayrock.Json;
    using Newtonsoft.Json.Linq;

    public class TickerController : Controller
    {
        // GET: Ticker
        public static decimal GetTicker()
        {
            using (var db = new CryptoEntities())
            {
                var ticker = db.Ticker.OrderByDescending(a => a.id).Select(a => a.rate).First();
                return Math.Round(ticker, 2);
            }
        }

        public string ShowRate()
        {
            ApiViewModel vm = new ApiViewModel();
            JsonObject cTicker = ApiKraken.TickerInfo();

            dynamic rate = JObject.Parse(cTicker.ToString());
            foreach (JToken item in rate["result"])
            {
                vm.Rate = (decimal)item.Last["c"][0];
            }
            using (var db = new CryptoEntities())
            {
                Ticker dbTicker = new Ticker();

                dbTicker.created = vm.created;
                dbTicker.rate = vm.Rate;
                dbTicker.currency_src = vm.Currency_src;
                dbTicker.currency_trg = vm.Currency_trg;
                db.Ticker.Add(dbTicker);
                db.SaveChanges();
            }

            return GetTicker().ToString();
        }

        public JsonResult LoadChartData()
        {
            List<TickerChartViewModel> result = TickerList();
            return Json(TickerChartViewModel.GetList(result), JsonRequestBehavior.AllowGet);
        }

        private static List<TickerChartViewModel> TickerList()
        {
            using (var db = new CryptoEntities())
            {
                List<TickerChartViewModel> getTickerList = new List<TickerChartViewModel>();

                var dbTickerList = db.Ticker.Where(x => x.currency_trg == "BTC").ToList();

                foreach (Ticker x in dbTickerList)
                {
                    getTickerList.Add(new TickerChartViewModel
                    {
                        UnixTime = Manager.DateTimeHelper.ConvertToUnixTimeMs(x.created).ToString(),
                        Value = x.rate.ToString()
                    }
                    );
                }
                return getTickerList;
            }
        }

        public string ShowBalance()
        {
            using (var db = new CryptoEntities())
            {
                var dbPerson = db.Person.Where(a => a.email == User.Identity.Name).FirstOrDefault();
                decimal amount = db.Balance.Where(a => a.person_id == dbPerson.id).Sum(a => a.amount);

                return Math.Round(amount, 2).ToString();
            }
        }

    }
}