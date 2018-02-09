namespace CryptoTrader.Controllers
{
    using CryptoTrader.Manager;
    using CryptoTrader.Models.ViewModel;
    using Jayrock.Json;
    using Jayrock.Json.Conversion;
    using Newtonsoft.Json.Linq;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {


            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Test()
        {
            ApiViewModel vm = new ApiViewModel();
            JsonObject cTicker = ApiKraken.TickerInfo();

            var test1 = JObject.Parse( cTicker.ToString() );

            foreach( JToken item in test1["result"] )
            {
                vm.TickerC = item.Last["c"].ToString();
            }


            return PartialView( "_Header", vm );
        }
    }
}