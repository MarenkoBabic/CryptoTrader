namespace CryptoTrader.Controllers
{
    using CryptoTrader.Models.DbModel;
    using CryptoTrader.Models.ViewModel;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using System.Web.Mvc;
    using CryptoTrader.Models;

    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
    }
}