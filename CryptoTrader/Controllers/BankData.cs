namespace CryptoTrader.Controllers
{
    using AutoMapper;
    using CryptoTrader.Models.DbModel;
    using CryptoTrader.Models.ViewModel;
    using System.Linq;
    using System.Web.Mvc;

    public class BankTransferController : Controller
    {
        /// <summary>
        /// Kontodaten Anzeigen
        /// </summary>
        /// <param name="vm">ViewModel Bankdaten</param>
        /// <returns>View Index</returns>
        public ActionResult Index()
        {
            BankDataViewModel vm = new BankDataViewModel();
            using (var db = new CryptoEntities())
            {
                Person dbPerson = db.Person.Where(a => a.email == User.Identity.Name).FirstOrDefault();
                if (dbPerson.status == true)
                {
                    vm.FirstName = dbPerson.firstName;
                    vm.LastName = dbPerson.lastName;
                    vm.Reference = dbPerson.reference;
                    return View(vm);
                }
                else
                {
                    return RedirectToAction("PersonVerification", "Register");
                }
            }
        }
        [HttpPost]
        public ActionResult Index(BankDataViewModel vm)
        {
            using(var db = new CryptoEntities())
            {
                BankTransferHistory dbBankTransferHistory = new BankTransferHistory();
                BankAccount dbBankAccount = new BankAccount();
                Person dbPerson = db.Person.Where(a => a.email == User.Identity.Name).FirstOrDefault();

                dbBankAccount.created = vm.Created;
                dbBankAccount.person_id = dbPerson.id;
                dbBankAccount.iban = vm.PersonIban;
                dbBankAccount.bic = vm.PersonBic;
                db.BankAccount.Add(dbBankAccount);

                dbBankTransferHistory.created = vm.Created;
                dbBankTransferHistory.person_id = dbPerson.id;
                dbBankTransferHistory.currency = vm.Currency;
                dbBankTransferHistory.amount = vm.Amount;
                db.BankTransferHistory.Add(dbBankTransferHistory);

                db.SaveChanges();
            }
            ViewBag.Message = "Auftrag erteilt";
            return View();
        }
    }
}
