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
        /// Kontodaten Anzeigen oder Verifiziern falls nicht vorhanden
        /// </summary>
        /// <param name="vm">ViewModel Bankdaten</param>
        /// <returns>View Index</returns>
        public ActionResult Index()
        {

            BankDataViewModel vm = new BankDataViewModel();
            using( var db = new CryptoTraderEntities() )
            {
                BankAccount bankAccount = new BankAccount();
                Person dbPerson = db.Person.Where( a => a.email == User.Identity.Name ).FirstOrDefault();
                BankAccount dbBankAccount = db.BankAccount.Where( a => a.person_id == dbPerson.id ).FirstOrDefault();
                if( dbPerson.status == true )
                {
                    vm.BankName = vm.BankName;
                    vm.FirstName = dbPerson.firstName;
                    vm.LastName = dbPerson.lastName;
                    vm.Reference = dbPerson.reference;
                    vm.PersonBic = bankAccount.bic;
                    vm.PersonIban = bankAccount.iban;
                    return View( vm );
                }
                else
                {
                    return RedirectToAction( "PersonVerification", "Register" );
                }
            }
        }

        /// <summary>
        /// Geld auszahlung
        /// </summary>
        /// <param name="vm">ViewModel BankData</param>
        /// <returns>View mit ViewModel</returns>
        [HttpPost]
        public ActionResult Index( BankDataViewModel vm )
        {
            using( var db = new CryptoTraderEntities() )
            {
                BankTransferHistory dbBankTransferHistory = new BankTransferHistory();
                Person dbPerson = db.Person.Where( a => a.email == User.Identity.Name ).FirstOrDefault();
                BankAccount dbBankAccount = db.BankAccount.Where( a => a.person_id == dbPerson.id ).FirstOrDefault();

                dbBankAccount.created = vm.Created;
                dbBankAccount.iban = vm.PersonIban;
                dbBankAccount.bic = vm.PersonBic;
                db.BankAccount.Add( dbBankAccount );

                dbBankTransferHistory.created = vm.Created;
                dbBankTransferHistory.person_id = dbPerson.id;
                dbBankTransferHistory.currency = vm.Currency;
                dbBankTransferHistory.amount = vm.Amount;
                db.BankTransferHistory.Add( dbBankTransferHistory );

                db.SaveChanges();
            }
            ViewBag.Message = "Auftrag erteilt";
            return View();
        }
    }
}
