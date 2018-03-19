﻿namespace CryptoTrader.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using AutoMapper;
    using CryptoTrader.Manager;
    using CryptoTrader.Model.DbModel;
    using CryptoTrader.Model.ViewModel;

    public class TradeController : Controller
    {

        /// <summary>
        /// Befüllt die Historylist und gibt sie zum View mit
        /// </summary>
        /// <returns>View mit ViewModel</returns>
        public ActionResult Index()
        {
            bool result = ValidationController.IsUserAuthenticated(User.Identity.IsAuthenticated);
            if (result)
            {
                TradeBitCoinViewModel vm = new TradeBitCoinViewModel();
                using (var db = new CryptoTraderEntities())
                {
                    Person dbPerson = db.Person.Where(a => a.email == User.Identity.Name).FirstOrDefault();
                    bool haveHistory = db.TradeHistory.Any(a => a.person_id == dbPerson.id);

                    if (haveHistory)
                    {
                        vm.HistoryList = db.TradeHistory.Where(a => a.person_id == dbPerson.id).Include(a => a.Ticker).OrderByDescending(a => a.created).ToList();
                        return View(vm);
                    }
                    else
                    {
                        vm.HistoryList = null;
                        return View(vm);
                    }
                }
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
        /// <param name="vm">TradeBitCoinViewModel</param>
        /// <param name="submit">SubmitButton TradeIndex</param>
        /// <returns>View</returns>
        [HttpPost]
        public ActionResult Trade(TradeBitCoinViewModel vm, string submit)
        {
            if (vm.TradeAmountEuro > 0 || vm.TradeAmountBTC > 0)
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
                            dbBalance.amount -= TradeManager.TradeAmountByBTC(vm.TickerRate, vm.TradeAmountBTC);

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
                            dbBalance.amount += TradeManager.TradeAmountByBTC(vm.TickerRate, vm.TradeAmountBTC);

                            dbTradeHistory.amount = vm.TradeAmountBTC * (-1);
                            dbTradeHistory.person_id = dbPerson.id;
                            db.Entry(dbBalance).State = EntityState.Modified;
                            db.TradeHistory.Add(dbTradeHistory);
                            db.SaveChanges();
                            return RedirectToAction("Index");

                        }
                        TempData["ErrorMessage"] = "Nicht genug BitCoin vorhanden";

                    }
                    return RedirectToAction("Index", vm);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Betrag eingeben";
                return RedirectToAction("Index", vm);
            }
        }

        /// <summary>
        /// Zeigt die Anzahl wieviel der Kunde BitCoin hat
        /// </summary>
        /// <returns>Decimal BitCoin</returns>
        public decimal ShowBitCoin()
        {
            TradeBitCoinViewModel vm = new TradeBitCoinViewModel();
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
        /// Berechnet den Euro Wert
        /// </summary>
        /// <param name="TradeAmountBTC">Anzahl Bitcoins</param>
        /// <returns>Euro</returns>
        public ActionResult GetEuro(decimal? BTCAmount)
        {
            Regex regex = new Regex(@"[\d]{1,4}([,\.[\d]{1,2})?");
            if (regex.IsMatch(BTCAmount.ToString().Replace(".",",")))
            {
                using (var db = new CryptoTraderEntities())
                {
                    decimal dbTicker = db.Ticker.OrderByDescending(a => a.id).Select(a => a.rate).First();

                    BTCAmount = BTCAmount * dbTicker;
                    
                    return Json(Math.Round((decimal)BTCAmount, 8).ToString(), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Berechnet die Bitcoin Anzahl
        /// </summary>
        /// <param name="TradeAmountEuro">Euro</param>
        /// <returns>BitCoin Anzahl</returns>
        public ActionResult GetBTC(decimal? EuroAmount)
        {
            Regex regex = new Regex(@"[\d]{1,4}([.\,[\d]{1,2})?");
            if (regex.IsMatch(EuroAmount.ToString().Replace(".", ",")))
            {
                using (var db = new CryptoTraderEntities())
                {
                    decimal dbTicker = db.Ticker.OrderByDescending(a => a.id).Select(a => a.rate).First();

                    EuroAmount = EuroAmount / dbTicker;
                    return Json(Math.Round((decimal)EuroAmount, 8), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}