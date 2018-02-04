using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CryptoTrader.Manager;
using CryptoTrader.Models.DbModel;
namespace CryptoTrader.Controllers
{
    public class ValidationController : Controller
    {
        /// <summary>
        /// Prüft bei der Registrierung ob Email schon vorhanden
        /// </summary>
        /// <param name="email">Input Email</param>
        /// <returns>bool</returns>
        public ActionResult IsMailExistToRegister( string RegisterEmail )
        {
            using( var db = new CryptoTraderEntities() )
            {
                var PersonList = db.Person.ToList();


                bool isExist = PersonList.Where(
                    a => a.email.ToLowerInvariant().Equals( RegisterEmail.ToLower() )
                    ).FirstOrDefault() != null;
                return Json( !isExist, JsonRequestBehavior.AllowGet );
            }
        }


        /// <summary>
        /// Prüft ob EMail und password übereinstimmen
        /// </summary>
        /// <param name="email">Input Email</param>
        /// <param name="password">Input Password</param>
        /// <returns>bool</returns>
        public ActionResult IsPasswordTrue( string email, string password )
        {
            using( var db = new CryptoTraderEntities() )
            {
                Person dbPerson = db.Person.Where( a => a.email == email ).FirstOrDefault();
                var personList = db.Person.ToList();
                string passwordHash = Hashen.HashBerechnen( password + dbPerson.salt );

                //Prüft ob Email und Password überein stimmen
                bool isTrue = personList.Where(
                    a => a.email.ToLowerInvariant().Equals( email, System.StringComparison.CurrentCultureIgnoreCase ) &&
                    (a.password).Equals( passwordHash ) ).FirstOrDefault() != null;
                return Json( isTrue, JsonRequestBehavior.AllowGet );
            }
        }
    }
}
