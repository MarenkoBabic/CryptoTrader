
namespace CryptoTrader.Manager
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using CryptoTrader.Model.DbModel;

    public class FillList
    {
        /// <summary>
        /// Befüllt die Liste für land
        /// </summary>        
        /// <param name="auswahl">Ausgewähltes Land</param>        
        /// <returns>Liste mit Länder</returns>
        public static List<SelectListItem> FillCountryList(List<Country> Countries)
        {
            List<SelectListItem> countryList = new List<SelectListItem>();
            foreach (Country country in Countries)
            {
                countryList.Add(new SelectListItem() { Text = country.countryName });
            }
            return countryList;
        }

        public static List<TradeHistory> FillTradeHistoryList()
        {
            using (var db = new CryptoTraderEntities())
            {
                return db.TradeHistory.ToList();
            }
        }
    }
}