namespace CryptoTrader.Controllers
{
    using AutoMapper;
    using CryptoTrader.Manager;
    using CryptoTrader.Model.DbModel;
    using CryptoTrader.Model.ViewModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    public class AdminController : Controller
    {
        /// <summary>
        /// Gibt die Personenliste an den View weiter
        /// </summary>
        /// <returns>Personenliste</returns>
        [Authorize(Roles = "Admin")]
        public ActionResult AdminView()
        {
            //Prüft ob er eingeloggt ist
            bool result = ValidationController.IsUserAuthenticated(User.Identity.IsAuthenticated);
            if (result)
            {
                AdminViewModel adminVM = new AdminViewModel
                {
                    PersonList = FillList.GetPersonList()
                };
                return View(adminVM);
            }
            else
            {
                TempData["ErrorMessage"] = "Sie müssen eingeloggt sein";
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Befüllt die Datenbank aus dem ViewModel
        /// </summary>
        /// <param name="id">Person_Id</param>
        /// <param name="vm">AdminViewModel</param>
        /// <returns>GetView</returns>
        [HttpPost]
        public ActionResult AdminView(int id, AdminViewModel adminVM)
        {

            using (var db = new CryptoTraderEntities())
            {
                Person dbPerson = db.Person.Find(id);
                Balance dbBalance = db.Balance.Where(a => a.person_id == dbPerson.id).FirstOrDefault();

                BankTransferHistory dbHistory = Mapper.Map<BankTransferHistory>(adminVM);
                dbHistory.person_id = dbPerson.id;
                db.BankTransferHistory.Add(dbHistory);

                if (dbBalance == null)
                {
                    Balance balance = Mapper.Map<Balance>(adminVM);
                    balance.person_id = dbPerson.id;
                    db.Balance.Add(balance);
                }
                else
                {
                    dbBalance.amount += adminVM.Amount;
                    dbBalance.created = adminVM.Created;
                    db.Entry(dbBalance).State = EntityState.Modified;
                }

                if (ModelState.IsValid)
                {
                    db.SaveChanges();
                    TempData["ConfirmMessage"] = "Der Betrag wurde Gutgeschrieben ";
                    return RedirectToAction("AdminView");
                }
                else
                {
                    TempData["ErrorMessage"] = "Fehlgeschlagen bitte versuchen sie es erneut";
                    return RedirectToAction("AdminView");
                }
            }
        }

        /// <summary>
        /// Setzt den User auf Inaktiv oder Activ
        /// </summary>
        /// <param name="id">Person_id</param>
        /// <returns>Get View</returns>
        public ActionResult ChangeActive(int id)
        {
            AdminManager.ChangeActiveStatus(id);
            return RedirectToAction("AdminView");
        }

        /// <summary>
        /// Filterd die PersonenListe
        /// </summary>
        /// <param name="submit">Input Button</param>
        /// <param name="vm">AdminViewModel</param>
        /// <returns>View</returns>
        public ActionResult FilterList(string submit, AdminViewModel adminVM)
        {
            using (var db = new CryptoTraderEntities())
            {
                adminVM.FilterList = AdminManager.FilterThePersonList(adminVM.PersonId, adminVM.FirstName, adminVM.LastName, adminVM.Reference);
                return View("AdminView", adminVM);
            }
        }

        /// <summary>
        /// Übernimmt den User
        /// </summary>
        /// <param name="id">User_id</param>
        /// <returns>View mit Userdaten</returns>
        public ActionResult UserLogin(int id)
        {
            AdminManager.GetUserLogin(id);
            return RedirectToAction("Index", "Home");
        }
    }
}