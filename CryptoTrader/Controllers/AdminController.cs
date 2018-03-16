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
        public ActionResult AdminView()
        {
            bool result = ValidationController.IsUserAuthenticated(User.Identity.IsAuthenticated);
            if (result)
            {
                AdminViewModel vm = new AdminViewModel();
                using (var db = new CryptoTraderEntities())
                {
                    var dbPerson = new Person();
                    vm.PersonList = db.Person.ToList();
                    TempData["QuestionMessage"] = "Wirklich Geldbetrag überweisen";
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
        public ActionResult AdminView(int? id, AdminViewModel vm, bool confirm_value = true)
        {
            if (confirm_value == true)
            {
                ViewBag.Message = "Abgeschickt";
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
                        dbBalance = new Balance
                        {
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
                        return RedirectToAction("AdminView");
                    }
                }
            }

            ViewBag.Message = "Abgebrochen";
            return RedirectToAction("AdminView");
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