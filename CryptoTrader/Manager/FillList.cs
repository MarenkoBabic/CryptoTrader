
namespace CryptoTrader.Manager
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using CryptoTrader.Model.DbModel;
    using CryptoTrader.Model.ViewModel;
    using AutoMapper;
    using System.Data.Entity;

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

        /// <summary>
        /// Befüllt die TradeHistoryList
        /// </summary>
        /// <param name="id">Person Id</param>
        /// <returns>List</returns>
        public static List<TradeViewModel> GetTradeHistoryList(int id)
        {
            using (var db = new CryptoTraderEntities())
            {
                var tradeHistoryList = db.TradeHistory.Where(a => id == a.person_id).Include(a => a.Ticker).ToList();
                List<TradeViewModel> tradeList = Mapper.Map<List<TradeHistory>, List<TradeViewModel>>(tradeHistoryList);
                

                return tradeList;
            }
        }

        /// <summary>
        /// Befüllt die BankTransferHistoryliste 
        /// </summary>
        /// <param name="id">Person Id</param>
        /// <returns>List</returns>
        public static List<BankTransferViewModel> GetBankHistoryList(int id)
        {
            using (var db = new CryptoTraderEntities())
            {
                var bankAccount = db.BankTransferHistory.Where(a => id == a.person_id).ToList();
                List<BankTransferViewModel> historyList = Mapper.Map<List<BankTransferHistory>, List<BankTransferViewModel>>(bankAccount);
                return historyList;
            }
        }

        /// <summary>
        /// Befüllt die PersonenListe mit Personen aus der Datenbank
        /// </summary>
        /// <returns>PeronenListe</returns>
        public static List<AdminViewModel> GetPersonList()
        {
            using (var db = new CryptoTraderEntities())
            {
                List<Person> dbPerson = db.Person.ToList();
                List<AdminViewModel> list = Mapper.Map<List<Person>, List<AdminViewModel>>(dbPerson);
                return list;
            }
        }

    }
}