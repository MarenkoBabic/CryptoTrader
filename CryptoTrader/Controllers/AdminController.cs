namespace CryptoTrader.Controllers
{
    using AutoMapper;
    using CryptoTrader.Manager;
    using CryptoTrader.Model.DbModel;
    using CryptoTrader.Model.ViewModel;
    using CryptoTrader.ModelMapper;
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
                AdminViewModel adminVM = new AdminViewModel
                {
                    PersonList = AdminMapper.PersonList()
                };
                return View(adminVM);
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
        public ActionResult ChangeActive(int id)
        {
            AdminMapper.ChangeActiveStatus(id);
            return RedirectToAction("AdminView");
        }

        /// <summary>
        /// Filterd die PersonenListe
        /// </summary>
        /// <param name="submit">Input Button</param>
        /// <param name="vm">AdminViewModel</param>
        /// <returns>Get View</returns>
        public ActionResult FilterList(string submit,AdminViewModel adminVM)
        {
            using (var db = new CryptoTraderEntities())
            {
                adminVM.FilterList = AdminMapper.FilterThePersonList(adminVM.PersonId, adminVM.FirstName, adminVM.LastName, adminVM.Reference);
                return View("AdminView", adminVM);
            }
        }


        public ActionResult UserLogin(int? id)
        {
            using (var db = new CryptoTraderEntities())
            {
                Person dbPerson = db.Person.Find(id);

                string email = dbPerson.email;
                string firstName = dbPerson.firstName;
                string lastName = dbPerson.lastName;
                string role = dbPerson.role;
                Cookies.CreateCookies(email, role, firstName, lastName);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}