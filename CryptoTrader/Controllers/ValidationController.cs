namespace CryptoTrader.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using CryptoTrader.Manager;
    using CryptoTrader.Models.DbModel;
    using CryptoTrader.Models.ViewModel;

    public class ValidationController : Controller
    {
        public static object DateTimeHelper { get; private set; }

        /// <summary>
        /// Prüft bei der Registrierung ob Email schon vorhanden
        /// </summary>
        /// <param name = "email" > Input Email</param>
        /// <returns>bool</returns>
        public ActionResult IsMailExistToRegister( string RegisterEmail )
        {
            using( var db = new CryptoTraderEntities() )
            {
                var PersonList = db.Person.ToList();


                bool isExist = PersonList.Where(
                    a => a.email.ToLowerInvariant().Equals( RegisterEmail.ToLower() )
                    ).FirstOrDefault() != null;
                return Json( !isExist, JsonRequestBehavior.AllowGet );
            }
        }


        /// <summary>
        /// Prüft ob EMail und password übereinstimmen
        /// </summary>
        /// <param name = "email" > Input Email</param>
        /// <param name = "password" > Input Password</param>
        /// <returns>bool</returns>
        public ActionResult IsPasswordTrue( string LoginEmail, string LoginPassword )
        {
            bool result = false;

            using( var db = new CryptoTraderEntities() )
            {
                Person dbPerson = db.Person.Where( a => a.email == LoginEmail ).FirstOrDefault();
                if( !string.IsNullOrEmpty( LoginEmail ) && !string.IsNullOrEmpty( LoginPassword ) )
                {
                    if( dbPerson != null )
                    {
                        var personList = db.Person.ToList();
                        string passwordHash = Hashen.HashBerechnen( LoginPassword + dbPerson.salt );

                        //Prüft ob Email und Password überein stimmen
                        result = personList.Where(
                            a => a.email.ToLowerInvariant().Equals( LoginEmail, System.StringComparison.CurrentCultureIgnoreCase ) &&
                            (a.password).Equals( passwordHash ) ).FirstOrDefault() != null;
                        return Json( result, JsonRequestBehavior.AllowGet );
                    }
                }
                else
                {
                    return Json( result, JsonRequestBehavior.AllowGet );
                }
                return Json( result, JsonRequestBehavior.AllowGet );
            }
        }

        public string ShowRate()
        {
            var dbTicker = new Ticker();
            using( var db = new CryptoTraderEntities() )
            {
                dbTicker.created = DateTime.Now;
                dbTicker.currency_src = "Eur";
                dbTicker.currency_trg = "BTC";
                dbTicker.rate = Manager.ApiKraken.GetTicker();
                db.Ticker.Add( dbTicker );
                db.SaveChanges();
            }

            return ApiKraken.GetTicker().ToString();
        }
        public JsonResult LoadChartData()
        {
            List<TickerChartViewModel> result = TickerList();
            return Json( TickerChartViewModel.GetList( result ), JsonRequestBehavior.AllowGet );
        }

        private static List<TickerChartViewModel> TickerList()
        {
            using( var db = new CryptoTraderEntities() )
            {
                List<TickerChartViewModel> getTickerList = new List<TickerChartViewModel>();

                var dbTickerList = db.Ticker.Where( x => x.currency_trg == "BTC" ).ToList();

                foreach( Ticker x in dbTickerList )
                {
                    getTickerList.Add( new TickerChartViewModel {
                        UnixTime = Manager.DateTimeHelper.ConvertToUnixTimeMs( x.created ).ToString(),
                        Value = x.rate.ToString()
                    }
                    );
                }
                return getTickerList;
            }
        }

    }
}
