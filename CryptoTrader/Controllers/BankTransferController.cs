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
                        BankTransferViewModel bankTransferVM = new BankTransferViewModel();

                        if (db.BankAccount.Any(a => a.person_id == dbPerson.id))
                        {
                            bankTransferVM = Mapper.Map<BankTransferViewModel>(dbBankAccount);

                            bankTransferVM.FirstName = dbPerson.firstName;
                            bankTransferVM.LastName = dbPerson.lastName;
                            bankTransferVM.Reference = dbPerson.reference;
                            bankTransferVM.BankHistoryList = FillList.GetBankHistoryList(dbPerson.id);
                        }
                        else
                        {

                            bankTransferVM.FirstName = dbPerson.firstName;
                            bankTransferVM.LastName = dbPerson.lastName;
                            bankTransferVM.Reference = dbPerson.reference;
                        }

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
        public ActionResult PayOut(BankTransferViewModel bankTransferVM)
        {
            using (var db = new CryptoTraderEntities())
            {
                Person dbPerson = db.Person.Where(a => a.email == User.Identity.Name).FirstOrDefault();
                var dbBalance = new Balance();
                decimal.TryParse(bankTransferVM.Amount, out decimal tempAmount);
                if ( tempAmount > 0 && !string.IsNullOrEmpty(bankTransferVM.PersonBic) && !string.IsNullOrEmpty(bankTransferVM.PersonIban))
                {
                    BankTransferHistory dbBankTransferHistory = Mapper.Map<BankTransferHistory>(bankTransferVM);

                    //Prüfen ob Konto vorhanden 
                    if (!db.Balance.Any(a => a.person_id == dbPerson.id))
                    {
                        dbBalance.amount = 0;
                        dbBalance.created = DateTime.Now;
                        dbBalance.currency = "Eur";
                        dbBalance.person_id = dbPerson.id;
                        db.Balance.Add(dbBalance);
                    }
                    else
                    {
                        dbBalance = db.Balance.Where(a => a.person_id == dbPerson.id).FirstOrDefault();
                    }
                    // Kontostand abfragen 
                    if (dbBalance.amount > decimal.Parse(bankTransferVM.Amount))
                    {
                        //Falls kein Bankaccount vorhanden erstelle ein neuen
                        if (!db.BankAccount.Any(a => a.person_id == dbPerson.id))
                        {
                            BankAccount dbBankAccount = new BankAccount()
                            {
                                created = DateTime.Now,
                                person_id = dbPerson.id,
                                iban = bankTransferVM.PersonIban,
                                bic = bankTransferVM.PersonBic
                            };
                            db.BankAccount.Add(dbBankAccount);
                        }

                        dbBankTransferHistory.person_id = dbPerson.id;
                        dbBankTransferHistory.amount = decimal.Parse(bankTransferVM.Amount) * (-1);
                        dbBankTransferHistory.currency = "Eur";

                        dbBalance.amount -= decimal.Parse(bankTransferVM.Amount);
                        db.Entry(dbBalance).State = EntityState.Modified;
                        TempData["ConfirmMessage"] = "Auftrag erteilt";

                        if (ModelState.IsValid)
                        {
                            db.BankTransferHistory.Add(dbBankTransferHistory);
                            db.SaveChanges();
                        }

                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Limit überschritten";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Bitte alle Felder ausfüllen";
                }
            }
            return RedirectToAction("BankIndex", bankTransferVM);
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
