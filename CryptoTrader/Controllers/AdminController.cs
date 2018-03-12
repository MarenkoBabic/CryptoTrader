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
        [Authorize(Roles ="Admin")]
        public ActionResult Index()
        {
            bool result = ValidationController.IsUserAuthenticated(User.Identity.IsAuthenticated);
            if (result)
            {
                AdminViewModel vm = new AdminViewModel();
                using (var db = new JaroshEntities())
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
            using (var db = new JaroshEntities())
            {
                Person dbPerson = db.Person.Find(id);
                Balance dbBalance = db.Balance.Where(a => a.id ==dbPerson.id).FirstOrDefault();

                BankTransferHistory dbHistory = new BankTransferHistory
                {
                    created = vm.Created,
                    person_id = dbPerson.id,
                    amount = vm.Amount,
                    currency = vm.Currency
                };
                db.BankTransferHistory.Add(dbHistory);
                dbBalance.amount += vm.Amount;
                dbBalance.created = vm.Created;
                db.Entry(dbBalance).State = EntityState.Modified;
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
            using (var db = new JaroshEntities())
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