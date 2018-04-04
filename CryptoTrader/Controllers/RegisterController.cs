namespace CryptoTrader.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using CryptoTrader.Manager;
    using CryptoTrader.Model.DbModel;
    using CryptoTrader.Model.ViewModel;
    using System.Data.Entity;

    public class RegisterController : Controller
    {
        /// <summary>
        /// Speichert den Benutzer wenn die Daten stimmen
        /// </summary>
        /// <param name="vm">ViewModel</param>
        ///<return>ErrorSeite</return>
        /// <returns>Index bei erfolgreich Registrieren</returns>
        [HttpPost]
        public ActionResult ToRegister(RegisterViewModel vm)
        {
            vm.Salt = Hashen.SaltErzeugen();
            vm.RegisterPassword = Hashen.HashBerechnen(vm.RegisterPassword + vm.Salt);
            Person dbPerson = Mapper.Map<Person>(vm);
            dbPerson.reference = GeneratorReference.ReferencGenerator();
            if (ModelState.IsValid)
            {
                using (var db = new CryptoTraderEntities())
                {
                    db.Person.Add(dbPerson);
                    db.SaveChanges();
                    TempData["ConfirmMessage"] = "Erfolgreich Registiert";
                    var personList = db.Person.ToList();
                    LoginManager.FromRegisterToLogin(dbPerson.email, dbPerson.password, personList);
                    return RedirectToAction("Index", "Home");
                }
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
                using (var db = new CryptoTraderEntities())
                {
                    Person dbPerson = db.Person.Where(a => a.email.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                    var countries = db.Country.ToList();

                    if (dbPerson.status == true)
                    {
                        City dbCity = db.City.Where(a => a.id == dbPerson.id).FirstOrDefault();
                        Country dbCountry = db.Country.Where(a => a.id == dbCity.country_id).FirstOrDefault();
                        Address dbAddress = db.Address.Where(a => a.person_id == dbPerson.id).FirstOrDefault();
                        Upload dbUpload = db.Upload.Where(a => a.person_id == dbPerson.id).FirstOrDefault();

                        personVerificationVM.CityName = dbCity.cityName;
                        personVerificationVM.Zip = dbCity.zip;
                        personVerificationVM.CountryName = dbCountry.countryName;
                        personVerificationVM.Street = dbAddress.street;
                        personVerificationVM.Number = dbAddress.number;
                        personVerificationVM.CountryList = FillList.FillCountryList(countries);

                        return View(personVerificationVM);
                    }
                    personVerificationVM.CountryList = FillList.FillCountryList(countries);
                }
                return View(personVerificationVM);
            }
            else
            {
                TempData["ErrorMessage"] = "Sie müssen eingeloggt sein";
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
            using (var db = new CryptoTraderEntities())
            {
                Country dbCountry = db.Country.Where(a => a.countryName.Equals(vm.CountryName)).FirstOrDefault();
                Person dbPerson = db.Person.Where(a => a.email.Equals(User.Identity.Name)).FirstOrDefault();

                if (dbPerson.status == true)
                {
                    City city = db.City.Where(a => a.country_id == dbCountry.id).FirstOrDefault();
                    dbUpload = db.Upload.Where(a => a.person_id == dbPerson.id).FirstOrDefault();

                    dbAddress.city_id = city.id;
                    dbAddress.person_id = dbPerson.id;

                    db.Entry(dbAddress).State = EntityState.Modified;
                    db.Entry(dbCity).State = EntityState.Modified;
                    db.Entry(dbUpload).State = EntityState.Modified;

                    //db.Upload.Add(dbUpload);
                }
                else
                {
                    dbCity.country_id = dbCountry.id;
                    db.City.Add(dbCity);

                    dbAddress.city_id = dbCity.id;
                    dbAddress.person_id = dbPerson.id;
                    db.Address.Add(dbAddress);

                    //dbUpload.created = vm.Created;  
                    dbUpload.path = UploadImage.ImageUploadPath(vm, dbPerson.id);
                    dbUpload.person_id = dbPerson.id;
                    db.Upload.Add(dbUpload);
                }
                if (ModelState.IsValid)
                {
                    dbPerson.status = true;
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
