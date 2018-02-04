using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CryptoTrader.Manager;
using CryptoTrader.Models.ViewModel;

namespace CryptoTrader.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// LoginController
        /// </summary>
        /// <param name="LoginViewModel">ViewModel Login</param>
        /// <returns>Startseite</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login( LoginViewModel vm )
        {
            bool result = LoginManager.Login( vm.Email, vm.Password );
            if( result )
            {
                return RedirectToAction( "Index", "Home" );
            }
            else
            {
                ModelState.AddModelError( "LogOnError", "Benutzername oder Password falsch" );
                return View();
            }
        }

        /// <summary>
        /// Controller zum Ausloggen
        /// </summary>
        /// <returns>View Startseite</returns>
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction( "Index", "Home" );
        }
    }
}