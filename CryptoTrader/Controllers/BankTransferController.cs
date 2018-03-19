namespace CryptoTrader.Controllers
{
    using AutoMapper;
    using CryptoTrader.Model.DbModel;
    using CryptoTrader.Model.ViewModel;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    public class BankTransferController : Controller
    {
        /// <summary>
        /// CryptoTrader-Kontodaten Anzeigen oder Verifiziern falls nicht vorhanden
        /// </summary>
        /// <param name="vm">ViewModel Bankdaten</param>
        /// <returns>View Index</returns>
        public ActionResult BankIndex()
        {
            bool result = ValidationController.IsUserAuthenticated(User.Identity.IsAuthenticated);
            if (result)
            {
                using (var db = new CryptoTraderEntities())
                {
                    Person dbPerson = db.Person.Where(a => a.email == User.Identity.Name).FirstOrDefault();
                    BankAccount dbBankAccount = db.BankAccount.Where(a => a.person_id == dbPerson.id).FirstOrDefault();

                    if (dbPerson.status == true)
                    {
                        BankTransferViewModel vm = Mapper.Map<BankTransferViewModel>(dbPerson);
                        vm.BankHistoryList = db.BankTransferHistory.Where(a => a.person_id == dbPerson.id).ToList();
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

        /// <summary>
        /// Person-Kontodaten angeben oder Nur betrag falls schon in der Datenbank vorhanden
        /// </summary>
        /// <returns>View mit ViewModel</returns>
        public ActionResult PayOut()
        {
            bool result = ValidationController.IsUserAuthenticated(User.Identity.IsAuthenticated);
            if (result)
            {
                BankTransferViewModel vm = new BankTransferViewModel();
                using (var db = new CryptoTraderEntities())
                {
                    Person dbPerson = db.Person.Where(a => a.email.Equals(User.Identity.Name)).FirstOrDefault();
                    BankAccount dbBankAccount = db.BankAccount.Where(a => a.person_id == dbPerson.id).FirstOrDefault();
                    bool checkBankAccount = db.BankAccount.Any(a => a.person_id == dbPerson.id);
                    vm.FirstName = dbPerson.firstName;
                    vm.LastName = dbPerson.lastName;
                    if (checkBankAccount)
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
        public ActionResult PayOut(BankTransferViewModel vm)
        {
            using (var db = new CryptoTraderEntities())
            {
                Person dbPerson = new Person();
                BankAccount BankAccountModel = Mapper.Map<BankAccount>(vm);
                BankTransferViewModel PersonModel = Mapper.Map<BankTransferViewModel>(dbPerson);
                BankTransferHistory dbBankTransferHistory = Mapper.Map<BankTransferHistory>(vm);


                dbPerson = db.Person.Where(a => a.email == User.Identity.Name).FirstOrDefault();
                BankAccount dbBankAccount = db.BankAccount.Where(a => a.person_id == dbPerson.id).FirstOrDefault();
                Balance dbBalance = db.Balance.Where(a => a.person_id == dbPerson.id).FirstOrDefault();
                if (dbBankAccount == null)
                {
                    BankAccountModel.person_id = dbPerson.id;
                    db.BankAccount.Add(BankAccountModel);
                }
                else
                {
                    vm.FirstName = dbPerson.firstName;
                    vm.LastName = dbPerson.lastName;
                    vm.PersonBic = dbBankAccount.bic;
                    vm.PersonIban = dbBankAccount.iban;
                }

                dbBankTransferHistory.person_id = dbPerson.id;
                dbBankTransferHistory.amount = vm.Amount * (-1);
                dbBankTransferHistory.currency = "Eur";
                db.BankTransferHistory.Add(dbBankTransferHistory);

                dbBalance.amount -= dbBankTransferHistory.amount * (-1);
                db.Entry(dbBalance).State = EntityState.Modified;

                if (ModelState.IsValid)
                {
                    db.SaveChanges();
                }
            }
            TempData["ConfirmMessage"] = "Auftrag erteilt";
            return View("PayOut", vm);
        }

        /// <summary>
        /// KontostandAnzeige
        /// </summary>
        /// <returns>Kontostand oder "00.00"</returns>
        public string ShowBalance()
        {
            using (var db = new CryptoTraderEntities())
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
