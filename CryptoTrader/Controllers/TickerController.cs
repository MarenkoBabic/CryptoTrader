using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CryptoTrader.Models.DbModel;
using CryptoTrader.Models.ViewModel;
using Jayrock.Json;
using Newtonsoft.Json.Linq;
using CryptoTrader.Manager;

namespace CryptoTrader.Controllers
{
    public class TickerController : Controller
    {
        // GET: Ticker
        public ActionResult Index()
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
            return View();
        }


        public string ShowRate()
        {
            decimal rate = TickerManager.GetTicker();
            return rate.ToString();
        }

    }
}