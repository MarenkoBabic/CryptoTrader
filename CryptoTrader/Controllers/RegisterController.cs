namespace CryptoTrader.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using CryptoTrader.Manager;
    using CryptoTrader.Model.DbModel;
    using CryptoTrader.Model.ViewModel;

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
                using (var db = new CryptoTraderEntities())
                {
                    db.Person.Add(dbPerson);
                    db.SaveChanges();
                    TempData["ConfirmMessage"] = "Erfolgreich Regestiert";
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
                    City dbCity = db.City.Where(a => a.id == dbPerson.id).FirstOrDefault();
                    Country dbCountry = db.Country.Where(a => a.id == dbCity.country_id).FirstOrDefault();
                    Address dbAddress = db.Address.Where(a => a.person_id == dbPerson.id).FirstOrDefault();
                    var countries = db.Country.ToList();

                    if (dbPerson.status == true)
                    {
                        personVerificationVM.CityName = dbCity.cityName;
                        personVerificationVM.Zip = dbCity.zip;
                        personVerificationVM.CountryName = dbCountry.countryName;
                        personVerificationVM.Street = dbAddress.street;
                        personVerificationVM.Number = dbAddress.number;
                        personVerificationVM.CountryList = FillList.FillCountryList(db.Country.ToList());

                        return View(personVerificationVM);
                    }
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
            using (var db = new CryptoTraderEntities())
            {
                Country dbCountry = db.Country.Where(a => a.countryName.Equals(vm.CountryName)).FirstOrDefault();
                Person dbPerson = db.Person.Where(a => a.email.Equals(User.Identity.Name)).FirstOrDefault();

                if (dbPerson.status == true)
                {
                    dbUpload.person_id = dbPerson.id;
                    dbUpload.path = UploadImage.ImageUploadPath(vm, dbPerson.id);
                    db.Upload.Add(dbUpload);

                    if (ModelState.IsValid)
                    {
                        db.SaveChanges();
                    }
                }
                else
                {

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
            }

            return RedirectToAction("Index", "Home");
        }
    }
}