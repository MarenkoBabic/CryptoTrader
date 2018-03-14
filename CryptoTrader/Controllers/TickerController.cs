namespace CryptoTrader.Controllers
{
    using AutoMapper;
    using CryptoTrader.Manager;
    using CryptoTrader.Model.DbModel;
    using CryptoTrader.Model.ViewModel;
    using Jayrock.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class TickerController : Controller
    {
        public ActionResult SaveTickertoDB()
        {
            ApiViewModel vm = new ApiViewModel();
            JsonObject cTicker = ApiKraken.TickerInfo();
            dynamic rate = JObject.Parse(cTicker.ToString());
            foreach (JToken item in rate["result"])
            {
                vm.Rate = (decimal)item.Last["c"][0];
            }
            Ticker dbTicker = Mapper.Map<Ticker>(vm);

            using (var db = new CryptoTraderEntities())
            {
                if (ModelState.IsValid)
                {
                    db.Ticker.Add(dbTicker);
                    db.SaveChanges();
                }
            }

            return null;
        }


        public string ShowRate()
        {
            SaveTickertoDB();
            return GetTicker().ToString();
        }

        public JsonResult LoadChartData()
        {
            List<TickerChartViewModel> result = TickerList();
            return Json(TickerChartViewModel.GetList(result), JsonRequestBehavior.AllowGet);
        }



        private static decimal GetTicker()
        {
            using (var db = new CryptoTraderEntities())
            {
                decimal ticker = db.Ticker.OrderByDescending(a => a.id).Select(a => a.rate).First();
                return Math.Round(ticker, 2);
            }
        }

        private static List<TickerChartViewModel> TickerList()
        {
            using (var db = new CryptoTraderEntities())
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

    }
}