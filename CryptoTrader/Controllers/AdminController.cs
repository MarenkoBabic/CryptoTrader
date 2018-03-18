namespace CryptoTrader.Controllers
{
    using AutoMapper;
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
            //Prüft ob er eingeloggt ist
            bool result = ValidationController.IsUserAuthenticated(User.Identity.IsAuthenticated);
            if (result)
            {
                AdminViewModel vm = new AdminViewModel();
                using (var db = new CryptoTraderEntities())
                {
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
        public ActionResult AdminView(int? id, AdminViewModel vm)
        {
            ViewBag.Message = "Abgeschickt";
            using (var db = new CryptoTraderEntities())
            {
                Person dbPerson = db.Person.Find(id);
                Balance dbBalance = db.Balance.Where(a => a.person_id == dbPerson.id).FirstOrDefault();

                BankTransferHistory dbHistory = Mapper.Map<BankTransferHistory>(vm);
                dbHistory.person_id = dbPerson.id;
                db.BankTransferHistory.Add(dbHistory);

                if (dbBalance == null)
                {
                    Balance balance = Mapper.Map<Balance>(vm);
                    balance.person_id = dbPerson.id;
                    db.Balance.Add(balance);
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