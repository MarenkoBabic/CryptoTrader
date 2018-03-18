namespace CryptoTrader.Model.ViewModel
{
    using CryptoTrader.Manager;
    using CryptoTrader.Model.DbModel;
    using System.Collections.Generic;

    public class TradeHistoryListViewModel
    {
        public List<TradeHistory> HistoryList { get; set; }

        public TradeHistoryListViewModel()
        {
            HistoryList = new List<TradeHistory>();
        }
    }
}