namespace CryptoTrader.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using CryptoTrader.Model.DbModel;
    using CryptoTrader.Models.ViewModel;

    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {

            AdminViewModel vm = new AdminViewModel();
            using (var db = new CryptoTraderEntities())
            {
                var dbPerson = new Person();
                vm.PersonList = db.Person.ToList();
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(int? id, AdminViewModel vm)
        {
            using (var db = new CryptoTraderEntities())
            {
                Person dbPerson = db.Person.Find(id);

                BankTransferHistory history = new BankTransferHistory
                {
                    created = vm.Created,
                    person_id = dbPerson.id,
                    amount = vm.Amount,
                    currency = vm.Currency
                };
                db.BankTransferHistory.Add(history);

                Balance dbBalance = new Balance
                {
                    created = vm.Created,
                    person_id = dbPerson.id,
                    currency = vm.Currency,
                    amount = vm.Amount
                };
                db.Balance.Add(dbBalance);

                if (ModelState.IsValid)
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }



        public ActionResult Modify(int? id, AdminViewModel vm)
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
            return RedirectToAction("Index", vm.PersonList);
        }
    }
}