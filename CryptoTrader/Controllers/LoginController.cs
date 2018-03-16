namespace CryptoTrader.Controllers
{
    using CryptoTrader.Manager;
    using CryptoTrader.Model.DbModel;
    using CryptoTrader.Model.ViewModel;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;

    public class LoginController : Controller
    {
        /// <summary>
        /// LoginController
        /// </summary>
        /// <param name="LoginViewModel">ViewModel Login</param>
        /// <returns>Startseite</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel vm)
        {
            using (var db = new CryptoTraderEntities())
            {
                List<Person> dbPersonList = db.Person.ToList();
                bool result = LoginManager.Login(vm.LoginEmail, vm.LoginPassword, dbPersonList);
                if (result)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = "Ihr Konto ist gesperrt melden sich beim Support";
                    return RedirectToAction("Index", "Home");
                }

            }
        }

        /// <summary>
        /// Controller zum Ausloggen
        /// </summary>
        /// <returns>View Startseite</returns>
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}