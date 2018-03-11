namespace CryptoTrader.Model.ViewModel
{
    using System;
    using System.Collections.Generic;
    using CryptoTrader.Model.DbModel;

    public class SellBitCoinViewModel
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TickerId { get; set; }
        public decimal TickerRate { get; set; }
        public decimal TradeAmount { get; set; }
        public decimal EuroAmount { get; set; }
        public decimal BitCoinAmount { get; set; }
        public List<TradeHistory> HistoryList { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public decimal BalanceAmount { get; set; }

        public SellBitCoinViewModel()
        {
            HistoryList = new List<TradeHistory>();
        }
    }
}