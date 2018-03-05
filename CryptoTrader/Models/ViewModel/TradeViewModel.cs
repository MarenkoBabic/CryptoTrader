namespace CryptoTrader.Models.ViewModel
{
    using System;
    using System.Collections.Generic;
    using CryptoTrader.Models.DbModel;

    public class TradeViewModel
    {
        public int PersonId { get; set; }
        public int TickerId { get; set; }
        public decimal TickerRate { get; set; }
        public decimal TradeAmount { get; set; }
        public List<TradeHistory> HistoryList { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public TradeViewModel()
        {
            HistoryList = new List<TradeHistory>();
        }
    }
}