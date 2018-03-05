﻿namespace CryptoTrader.Controllers
{
    using CryptoTrader.Manager;
    using CryptoTrader.Models.ViewModel;
    using Jayrock.Json;
    using Jayrock.Json.Conversion;
    using Newtonsoft.Json.Linq;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {


            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}