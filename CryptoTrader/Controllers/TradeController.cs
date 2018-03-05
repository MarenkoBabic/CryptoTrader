namespace CryptoTrader.Controllers
{
    using System.Web.Mvc;
    using CryptoTrader.Models.DbModel;
    using System.Linq;
    using CryptoTrader.Models.ViewModel;

    public class TradeController : Controller
    {
        // GET: Trade
        public ActionResult Index()
        {
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

                dbtradeHistroy.amount= vm.SellAmount;
                dbBalanace.currency = vm.Currency_Src_Buy;
                


                return View();
            
            }
        }
    }
}