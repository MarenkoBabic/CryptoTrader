namespace CryptoTrader.Model.ViewModel
{
    using System;
    using System.Collections.Generic;
    using CryptoTrader.Model.DbModel;

    public class BuyBitCoinViewModel
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TickerId { get; set; }
        public decimal TickerRate { get; set; }
        public decimal TradeAmountBTC { get; set; }
        public decimal EuroAmount { get; set; }
        public List<TradeHistory> HistoryList { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public decimal BalanceAmount { get; set; }
        public BuyBitCoinViewModel()
        {
            HistoryList = new List<TradeHistory>();
        }
    }
}