﻿namespace CryptoTrader.Controllers
{
    using System.Web.Mvc;
    using CryptoTrader.Models.ViewModel;
    using CryptoTrader.Models.DbModel;
    using System.Collections.Generic;
    using System.Linq;
    using System.Globalization;
    using System;
    using CryptoTrader.Manager;

    public class TradeController : Controller
    {
        // GET: Trade
        public ActionResult Index()
        {
            Ticker dbTicker = new Ticker();
            TradeViewModel vm = new TradeViewModel();
            using(var db = new CryptoTraderEntities() )
            {
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