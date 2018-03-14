namespace CryptoTrader.Controllers
{
    using CryptoTrader.Model.DbModel;
    using CryptoTrader.Model.ViewModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    public class AdminController : Controller
    {
        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            bool result = ValidationController.IsUserAuthenticated(User.Identity.IsAuthenticated);
            if (result)
            {
                AdminViewModel vm = new AdminViewModel();
                using (var db = new CryptoTraderEntities())
                {
                    var dbPerson = new Person();
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

        [HttpPost]
        public ActionResult Index(int? id, AdminViewModel vm)
        {
            using (var db = new CryptoTraderEntities())
            {
                Person dbPerson = db.Person.Find(id);
                Balance dbBalance = db.Balance.Where(a => a.person_id == dbPerson.id).FirstOrDefault();

                BankTransferHistory dbHistory = new BankTransferHistory
                {
                    created = vm.Created,
                    person_id = dbPerson.id,
                    amount = vm.Amount,
                    currency = vm.Currency
                };
                db.BankTransferHistory.Add(dbHistory);

                if (dbBalance == null)
                {
                    dbBalance = new Balance{
                        currency = vm.Currency,
                        amount = vm.Amount,
                        created = vm.Created,
                        person_id = dbPerson.id,
                    };
                    db.Balance.Add(dbBalance);
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