namespace CryptoTrader.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using CryptoTrader.Model.DbModel;
    using CryptoTrader.Models.ViewModel;

    public class BankTransferController : Controller
    {
        /// <summary>
        /// Kontodaten Anzeigen oder Verifiziern falls nicht vorhanden
        /// </summary>
        /// <param name="vm">ViewModel Bankdaten</param>
        /// <returns>View Index</returns>
        public ActionResult PayIn()
        {
            bool result = ValidationController.IsUserAuthenticated(User.Identity.IsAuthenticated);
            if (result)
            {
                using (var db = new JaroshEntities())
                {
                    Person dbPerson = db.Person.Where(a => a.email == User.Identity.Name).FirstOrDefault();
                    BankAccount dbBankAccount = db.BankAccount.Where(a => a.person_id == dbPerson.id).FirstOrDefault();

                    if (dbPerson.status == true)
                    {
                        PayInViewModel vm = Mapper.Map<PayInViewModel>(dbPerson);

                        if (dbBankAccount != null)
                        {
                            vm.PersonBic = dbBankAccount.bic;
                            vm.PersonIban = dbBankAccount.iban;
                        }
                        return View(vm);
                    }
                    else
                    {
                        return RedirectToAction("PersonVerification", "Register");
                    }
                }

            }
            else
            {
                TempData["ErrorMessage"] = "Sie müssen ein einloggen";
                return RedirectToAction("Index", "Home");
            }

        }


        public ActionResult PayOut()
        {
            bool result = ValidationController.IsUserAuthenticated(User.Identity.IsAuthenticated);
            if (result)
            {
                PayOutViewModel vm = new PayOutViewModel();
                using (var db = new JaroshEntities())
                {
                    Person dbPerson = db.Person.Where(a => a.email.Equals(User.Identity.Name)).FirstOrDefault();
                    BankAccount dbBankAccount = db.BankAccount.Where(a => a.person_id == dbPerson.id).FirstOrDefault();

                    vm.FirstName = dbPerson.firstName;
                    vm.LastName = dbPerson.lastName;
                    if (db.BankAccount.Any())
                    {
                        vm.PersonIban = dbBankAccount.iban;
                        vm.PersonBic = dbBankAccount.bic;
                    }
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
        /// Geld auszahlung
        /// </summary>
        /// <param name="vm">ViewModel Payout</param>
        /// <returns>View mit ViewModel</returns>
        [HttpPost]
        public ActionResult PayOut(PayOutViewModel vm)
        {
            using (var db = new JaroshEntities())
            {
                Person dbPerson = new Person();
                BankAccount BankAccountModel = Mapper.Map<BankAccount>(vm);
                PayOutViewModel PersonModel = Mapper.Map<PayOutViewModel>(dbPerson);
                BankTransferHistory dbBankTransferHistory = Mapper.Map<BankTransferHistory>(vm);


                dbPerson = db.Person.Where(a => a.email == User.Identity.Name).FirstOrDefault();
                BankAccount dbBankAccount = db.BankAccount.Where(a => a.person_id == dbPerson.id).FirstOrDefault();
                Balance dbBalance = db.Balance.Where(a => a.person_id == dbPerson.id).FirstOrDefault();
                if (dbBankAccount == null)
                {
                    db.BankAccount.Add(BankAccountModel);
                }
                else
                {
                    vm.FirstName = dbPerson.firstName;
                    vm.LastName = dbPerson.lastName;
                    vm.PersonBic = dbBankAccount.bic;
                    vm.PersonIban = dbBankAccount.iban;
                }
                dbBalance.created = DateTime.Now;
                dbBalance.amount = -vm.Amount;
                dbBankTransferHistory.person_id = dbPerson.id;
                db.BankTransferHistory.Add(dbBankTransferHistory);


                if (ModelState.IsValid)
                {
                    db.SaveChanges();
                }
            }
            TempData["ConfirmMessage"] = "Auftrag erteilt";
            return View("Index", vm);
        }

        public string ShowBalance()
        {
            using (var db = new JaroshEntities())
            {
                Person dbPerson = db.Person.Where(a => a.email == User.Identity.Name).FirstOrDefault();
                bool result = db.Balance.Any(a => a.person_id == dbPerson.id);
                if (result)
                {
                    Balance dbBalance = db.Balance.Where(a => a.person_id == dbPerson.id).FirstOrDefault();
                    decimal amount = dbBalance.amount;
                    return Math.Round((decimal)amount, 2).ToString();
                }
                return "00,00";
            }
        }

    }
}
