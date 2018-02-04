
namespace CryptoTrader.Manager
{using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CryptoTrader.Models.DbModel;

    public class CountryList
    {
        /// <summary>
        /// Befüllt die Liste für land
        /// </summary>        
        /// <param name="auswahl">Ausgewähltes Land</param>        
        /// <returns>Liste mit Länder</returns>
        public static List<SelectListItem> FilCountryList()
        {
            List<SelectListItem> countryList = new List<SelectListItem>();
            using( var db = new CryptoTraderEntities() )
            {
                var dbCountry = db.Country.ToList();

                foreach( Country country in dbCountry )
                {
                    countryList.Add( new SelectListItem() { Text = country.countryName } );
                }
            }
            return countryList;
        }

    }
}