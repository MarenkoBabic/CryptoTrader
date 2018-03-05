namespace CryptoTrader.Models.ViewModel
{
    using System.Collections.Generic;
    using CryptoTrader.Models.DbModel;

    public class TradeViewModel
    {
        public int PersonId { get; set; }
        public int TickerId { get; set; }
        public decimal TickerRate { get; set; }
        public string Currency_src { get; set; }
        public string Currency_trg { get; set; }
        public int MyProperty { get; set; }
        public decimal TradeAmount { get; set; }
        public List<TradeHistory> HistoryList { get; set; }

        public TradeViewModel()
        {
            HistoryList = new List<TradeHistory>();
        }
    }
}