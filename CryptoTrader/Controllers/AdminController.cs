namespace CryptoTrader.Controllers
{
    using AutoMapper;
    using CryptoTrader.Model.DbModel;
    using CryptoTrader.Model.ViewModel;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    public class AdminController : Controller
    {
        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult AdminView()
        {
            //Prüft ob er eingeloggt ist
            bool result = ValidationController.IsUserAuthenticated(User.Identity.IsAuthenticated);
            if (result)
            {
                AdminViewModel vm = new AdminViewModel();
                using (var db = new CryptoTraderEntities())
                {
                    vm.PersonList = db.Person.ToList();
                }
                return View(vm);
            }
            else
            {
                TempData["ErrorMessage"] = "Sie müssen ein einloggen";
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Befüllt die Datenbank aus dem ViewModel
        /// </summary>
        /// <param name="id">Person_Id</param>
        /// <param name="vm">AdminViewModel</param>
        /// <returns>GetView</returns>
        [HttpPost]
        public ActionResult AdminView(int? id, AdminViewModel vm)
        {
            using (var db = new CryptoTraderEntities())
            {
                Person dbPerson = db.Person.Find(id);
                Balance dbBalance = db.Balance.Where(a => a.person_id == dbPerson.id).FirstOrDefault();

                BankTransferHistory dbHistory = Mapper.Map<BankTransferHistory>(vm);
                dbHistory.person_id = dbPerson.id;
                db.BankTransferHistory.Add(dbHistory);

                if (dbBalance == null)
                {
                    Balance balance = Mapper.Map<Balance>(vm);
                    balance.person_id = dbPerson.id;
                    db.Balance.Add(balance);
                }
                else
                {
                    dbBalance.amount += vm.Amount;
                    dbBalance.created = vm.Created;
                    db.Entry(dbBalance).State = EntityState.Modified;
                }

                if (ModelState.IsValid)
                {
                    db.SaveChanges();
                    return RedirectToAction("AdminView");
                }
            }

            TempData["ErrorMessage"] = "Fehlgeschlagen bitte versuchen sie es erneut";
            return RedirectToAction("AdminView");
        }

        /// <summary>
        /// Setzt den User auf Inaktiv oder Activ
        /// </summary>
        /// <param name="id">Person_id</param>
        /// <returns>Get View</returns>
        public ActionResult ChangeActive(int? id)
        {
            using (var db = new CryptoTraderEntities())
            {
                Person dbPerson = db.Person.Find(id);

                if (dbPerson.active == true)
                {
                    dbPerson.active = false;
                }
                else
                {
                    dbPerson.active = true;
                }
                db.Entry(dbPerson).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("AdminView");
        }

        /// <summary>
        /// Filterd die PersonenListe
        /// </summary>
        /// <param name="submit">Input Button</param>
        /// <param name="vm">AdminViewModel</param>
        /// <returns>Get View</returns>
        public ActionResult FilterList(string submit, AdminViewModel vm)
        {
            using (var db = new CryptoTraderEntities())
            {
                if (submit == "Filtern")
                {
                    vm.FilterList = db.Person.ToList();
                    if (vm.PersonId > 0)
                    {
                        vm.FilterList = vm.FilterList.Where(a => a.id == vm.PersonId).ToList();
                    }

                    if (HasValue(vm.FirstName))
                    {
                        vm.FilterList = vm.FilterList.Where(a => a.firstName.StartsWith(vm.FirstName,StringComparison.CurrentCultureIgnoreCase)).ToList();
                    }
                    if (HasValue(vm.LastName))
                    {
                        vm.FilterList = vm.FilterList.Where(a => a.lastName.Equals(vm.LastName, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    }
                    if (HasValue(vm.Reference))
                    {
                        vm.FilterList = vm.FilterList.Where(a => a.reference.Equals(vm.Reference, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    }
                }
            }
            return View("AdminView", vm);
        }

        /// <summary>
        /// Validiert den mitgebenen String
        /// </summary>
        /// <param name="s">ViewModel property</param>
        /// <returns>bool</returns>
        private bool HasValue(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                return true;
            }
            return false;
        }
    }
}