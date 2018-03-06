namespace CryptoTrader.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using CryptoTrader.Model.DbModel;
    using CryptoTrader.Model.ViewModel;

    public class TradeController : Controller
    {
        // GET: Trade
        public ActionResult Index()
        {
            Ticker dbTicker = new Ticker();
            TradeViewModel vm = new TradeViewModel();
            using (var db = new CryptoEntities())
            {
                Person dbPerson = db.Person.Where(a => a.email == User.Identity.Name).FirstOrDefault();
                vm.HistoryList = db.TradeHistory.Where(a => a.person_id == dbPerson.id).ToList();
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(TradeViewModel vm)
        {
            using (var db = new CryptoEntities())
            {
                var dbPerson = db.Person.Where(a => a.email.Equals(User.Identity.Name)).FirstOrDefault();
                var dbBalanace = db.Balance.Where(a => a.person_id == dbPerson.id).FirstOrDefault();
                var dbtradeHistroy = new TradeHistory();

                


                return View();
            
            }
        }
    }
}