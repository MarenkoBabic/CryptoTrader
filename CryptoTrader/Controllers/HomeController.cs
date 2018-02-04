using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CryptoTrader.Models.DbModel;
using CryptoTrader.Models.ViewModel;

namespace CryptoTrader.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            test t = new test();
            ViewBag.Message = "Your application description page.";

            using(var db = new CryptoTraderEntities() )
            {
                Upload u = new Upload();
                t.testt = u.path;
            }

            return View(t);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Admin()
        {
            return View();
        }
    }
}