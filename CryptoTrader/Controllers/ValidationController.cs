﻿using System;
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
        public ActionResult IsPasswordTrue( string LoginEmail, string LoginPassword )
        {
            bool result = false;

            using( var db = new CryptoTraderEntities() )
            {
                Person dbPerson = db.Person.Where( a => a.email == LoginEmail ).FirstOrDefault();
                if( !string.IsNullOrEmpty( LoginEmail ) && !string.IsNullOrEmpty( LoginPassword ) )
                {
                    if( dbPerson != null )
                    {
                        var personList = db.Person.ToList();
                        string passwordHash = Hashen.HashBerechnen( LoginPassword + dbPerson.salt );

                        //Prüft ob Email und Password überein stimmen
                        result = personList.Where(
                            a => a.email.ToLowerInvariant().Equals( LoginEmail, System.StringComparison.CurrentCultureIgnoreCase ) &&
                            (a.password).Equals( passwordHash ) ).FirstOrDefault() != null;
                        return Json( result, JsonRequestBehavior.AllowGet );
                    }
                }
                else
                {
                    return Json( !result, JsonRequestBehavior.AllowGet );
                }
                return Json( !result, JsonRequestBehavior.AllowGet );
            }
        }
    }
}
