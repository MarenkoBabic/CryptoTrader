﻿namespace CryptoTrader.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using CryptoTrader.Manager;
    using CryptoTrader.Model.DbModel;
    using CryptoTrader.Models.ViewModel;

    public class RegisterController : Controller
    {
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Speichert den Benutzer wenn die Daten stimmen
        /// </summary>
        /// <param name="vm">ViewModel</param>
        ///<return>ErrorSeite</return>
        /// <returns>Index Home</returns>
        [HttpPost]
        public ActionResult ToRegister(RegisterViewModel vm)
        {
            vm.Salt = Hashen.SaltErzeugen();
            vm.RegisterPassword = Hashen.HashBerechnen(vm.RegisterPassword + vm.Salt);

            Person dbPerson = Mapper.Map<Person>(vm);
            if (ModelState.IsValid)
            {
                using (var db = new JaroshEntities())
                {
                    db.Person.Add(dbPerson);
                    db.SaveChanges();
                }
                object message = TempData["OkMessage"] = "Erfolgreich Regestiert";
                return RedirectToAction("Index", "Home", message);
            }
            return View();
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
        /// Authentifizierungs Controller 
        /// </summary>
        /// <returns>View mit CountryList</returns>
        public ActionResult PersonVerification()
        {
            bool result = ValidationController.IsUserAuthenticated(User.Identity.IsAuthenticated);
            if (result)
            {
                PersonVerificationViewModel personVerificationVM = new PersonVerificationViewModel();
                using (var db = new JaroshEntities())
                {
                    Person dbPerson = db.Person.Where(a => a.email.Equals(User.Identity.Name,StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                    if (dbPerson.status == true)
                    {
                        TempData["ErrorMessage"] = "Sie sind bereits Verifiziert";
                        return RedirectToAction("Index", "Home");
                    }
                    var countries = db.Country.ToList();
                    personVerificationVM.CountryList = FillList.FillCountryList(countries);
                }
                return View(personVerificationVM);
            }
            else
            {
                TempData["ErrorMessage"] = "Sie müssen sich einloggen";
                return RedirectToAction("Index", "Home");
            }

        }

        /// <summary>
        /// Datenerweiterung von User
        /// </summary>
        /// <param name="vm">ViewModel</param>
        /// <returns>View</returns>
        [HttpPost]
        public ActionResult PersonVerification(PersonVerificationViewModel vm)
        {

            Address dbAddress = Mapper.Map<Address>(vm);
            City dbCity = Mapper.Map<City>(vm);
            Upload dbUpload = Mapper.Map<Upload>(vm);
            using (var db = new JaroshEntities())
            {
                Country dbCountry = db.Country.Where(a => a.countryName.Equals(vm.CountryName)).FirstOrDefault();
                Person dbPerson = db.Person.Where(a => a.email.Equals(User.Identity.Name)).FirstOrDefault();

                dbPerson.status = vm.Status;
                dbPerson.reference = GeneratorReference.ReferencGenerator();

                dbCity.country_id = dbCountry.id;
                db.City.Add(dbCity);

                dbAddress.city_id = dbCity.id;
                dbAddress.person_id = dbPerson.id;
                db.Address.Add(dbAddress);

                //dbUpload.created = vm.Created;
                dbUpload.person_id = dbPerson.id;
                dbUpload.path = UploadImage.ImageUploadPath(vm, dbPerson.id);
                db.Upload.Add(dbUpload);

                if (ModelState.IsValid)
                {
                    db.SaveChanges();
                }
                else
                {
                    TempData["ErrorMessage"] = "Fehlgeschlagen";
                    return RedirectToAction("PersonVerification");
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}