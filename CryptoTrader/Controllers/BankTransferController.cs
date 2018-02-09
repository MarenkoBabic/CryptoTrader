namespace CryptoTrader.Controllers
{
    using AutoMapper;
    using CryptoTrader.Manager;
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
            ApiKraken.TickerInfo();
            BankDataViewModel vm = new BankDataViewModel();
            using (var db = new CryptoEntities())
            {
                Person dbPerson = db.Person.Where(a => a.email == User.Identity.Name).FirstOrDefault();
                BankAccount dbBankAccount = db.BankAccount.Where(a => a.person_id == dbPerson.id).FirstOrDefault();

                if (dbPerson.status == true)
                {
                    if (dbBankAccount != null)
                    {
                        vm.FirstName = dbPerson.firstName;
                        vm.LastName = dbPerson.lastName;
                        vm.Reference = dbPerson.reference;
                        vm.PersonBic = dbBankAccount.bic;
                        vm.PersonIban = dbBankAccount.iban;
                    }
                    ViewBag.Message = "HUHUHU";
                    return View(vm);
                }
                else
                {
                    return RedirectToAction("PersonVerification", "Register");
                }
            }
        }

        /// <summary>
        /// Geld auszahlung
        /// </summary>
        /// <param name="vm">ViewModel BankData</param>
        /// <returns>View mit ViewModel</returns>
        [HttpPost]
        public ActionResult Index(BankDataViewModel vm)
        {
            using (var db = new CryptoEntities())
            {
                Person dbPerson = new Person();
                var BankAccountModel = Mapper.Map<BankAccount>(vm);
                var PersonModel = Mapper.Map<BankDataViewModel>(dbPerson);
                BankTransferHistory dbBankTransferHistory = Mapper.Map<BankTransferHistory>(vm);

                dbPerson = db.Person.Where(a => a.email == User.Identity.Name).FirstOrDefault();
                BankAccount dbBankAccount = db.BankAccount.Where(a => a.person_id == dbPerson.id).FirstOrDefault();

                if (dbBankAccount == null)
                {
                    db.BankAccount.Add(BankAccountModel);
                }
                else
                {
                    vm.PersonBic = dbBankAccount.bic;
                    vm.PersonIban = dbBankAccount.iban;
                }
                dbBankTransferHistory.person_id = dbPerson.id;
                db.BankTransferHistory.Add(dbBankTransferHistory);


                if (ModelState.IsValid)
                {
                    db.SaveChanges();
                }
            }
            ViewBag.Message = "Auftrag erteilt";
            return View("Index", vm);
        }
    }
}
