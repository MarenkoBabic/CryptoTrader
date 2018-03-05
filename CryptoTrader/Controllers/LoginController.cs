namespace CryptoTrader.Controllers
{
    using CryptoTrader.Manager;
    using CryptoTrader.Models.DbModel;
    using CryptoTrader.Models.ViewModel;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Linq;

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
            using( var db = new CryptoEntities() )
            {
                var dbPersonList = new List<Person>();
                foreach( var item in db.Person )
                {
                    dbPersonList.Add( item );
                }
                bool result = LoginManager.Login( vm.LoginEmail, vm.LoginPassword, dbPersonList );
                if( result)
                {
                    return RedirectToAction( "Index", "Home" );
                }
                else
                {
                    ViewBag.Message = "Fehlgeschlagen, entweder Benutzername oder Password falsch , oder ihr Konto ist gesperrt";
                    return View();
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
            return RedirectToAction( "Index", "Home" );
        }
    }
}