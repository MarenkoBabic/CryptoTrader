using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CryptoTrader.Manager;
using CryptoTrader.Models.ViewModel;
using CryptoTrader.Models.DbModel;

namespace CryptoTrader.Controllers
{
    public class RegisterController : Controller
    {
        /// <summary>
        /// Speichert den Benutzer wenn die Daten stimmen
        /// </summary>
        /// <param name="vm">ViewModel</param>
        ///<return>ErrorSeite</return>
        /// <returns>Index Home</returns>
        [HttpPost]
        public ActionResult ToRegister( RegisterViewModel vm )
        {
            Person dbperson = new Person();
            dbperson.created = vm.Created;
            dbperson.firstname = vm.FirstName;
            dbperson.lastname = vm.LastName;
            dbperson.email = vm.Email;
            dbperson.salt = Hashen.SaltErzeugen();
            dbperson.password = Hashen.HashBerechnen( vm.Password + dbperson.salt );
            dbperson.active = vm.Active;
            dbperson.role = vm.Role;
            dbperson.status = vm.Status;
            //SendMail.SendEmail(vm.Email);
            if( ModelState.IsValid )
            {
                using( var db = new CryptoTraderEntities() )
                {
                    db.Person.Add( dbperson );
                    db.SaveChanges();
                }
                TempData["Message"] = "Regestrieren erfolgreich";
                return RedirectToAction( "Index", "Home" );
            }
            return View( "_Register" );
        }

        /// <summary>
        /// View zum email versenden
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Sent()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult PersonVerification()
        {
            PersonVerificationViewModel personVerificationVM = new PersonVerificationViewModel();
            personVerificationVM.CountryList = CountryList.FilCountryList();
            return View( personVerificationVM );
        }
        /// <summary>
        /// Datenerweiter von Person
        /// </summary>
        /// <param name="vm">ViewModel</param>
        /// <returns>View</returns>
        [HttpPost]
        public ActionResult PersonVerification( PersonVerificationViewModel vm )
        {
            Address dbAddress = new Address();
            City dbCity = new City();
            using( var db = new CryptoTraderEntities() )
            {
                Country dbCountry = db.Country.Where( a => a.name == vm.Country_Name ).FirstOrDefault();
                Person dbPerson = db.Person.Where( a => a.email == User.Identity.Name ).FirstOrDefault();
                vm.Person_id = dbPerson.id;

                dbCity.city1 = vm.CityName;
                dbCity.country_id = dbCountry.id;
                dbCity.created = vm.City_created;
                dbCity.zip = vm.Zip;
                db.City.Add( dbCity );

                dbAddress.created = vm.Address_created;
                dbAddress.person_id = vm.Person_id;
                dbAddress.street = vm.Street;
                dbAddress.numbers = vm.Number;
                dbAddress.city_id = dbCity.id;

                db.Address.Add( dbAddress );

                dbPerson.status = vm.Status;
                db.SaveChanges();
            }
            return RedirectToAction( "Index", "Home" );
        }
    }
}