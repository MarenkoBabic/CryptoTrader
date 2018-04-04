namespace CryptoTrader.Controllers
{
    using AutoMapper;
    using CryptoTrader.Manager;
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
                        BankTransferViewModel bankTransferVM = Mapper.Map<BankTransferViewModel>(dbBankAccount);

                        bankTransferVM.FirstName = dbPerson.firstName;
                        bankTransferVM.LastName = dbPerson.lastName;
                        bankTransferVM.Reference = dbPerson.reference;
                        bankTransferVM.BankHistoryList = FillList.GetBankHistoryList(dbPerson.id);

                        return View(bankTransferVM);
                    }
                    else
                    {
                        return RedirectToAction("PersonVerification", "Register");
                    }
                }

            }
            else
            {
                TempData["ErrorMessage"] = "Sie müssen eingeloggt sein";
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
                BankTransferHistory dbBankTransferHistory = Mapper.Map<BankTransferHistory>(vm);
                Person dbPerson = db.Person.Where(a => a.email == User.Identity.Name).FirstOrDefault();
                Balance dbBalance = db.Balance.Where(a => a.person_id == dbPerson.id).First();



                dbBankTransferHistory.person_id = dbPerson.id;
                dbBankTransferHistory.amount = decimal.Parse(vm.Amount) * (-1);
                dbBankTransferHistory.currency = "Eur";
                db.BankTransferHistory.Add(dbBankTransferHistory);

                dbBalance.amount -= decimal.Parse(vm.Amount);
                db.Entry(dbBalance).State = EntityState.Modified;

                if (ModelState.IsValid)
                {
                    db.SaveChanges();
                }
            }
            TempData["ConfirmMessage"] = "Auftrag erteilt";
            return RedirectToAction("BankIndex", vm);
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
