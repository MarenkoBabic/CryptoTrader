namespace CryptoTrader.Model.ViewModel
{
    using CryptoTrader.Model.DbModel;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class TradeBitCoinViewModel
    {
        public int PersonId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Created { get; set; } = DateTime.Now;

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int TickerId { get; set; }

        public decimal TickerRate { get; set; }

        [Display(Name ="BitCoin")]
        public decimal TradeAmountBTC { get; set; }

        [Display(Name ="Euro")]
        public decimal TradeAmountEuro { get; set; }

        public decimal BitCoinAmount { get; set; }

        public decimal BalanceAmount { get; set; }

        public List<TradeHistory> HistoryList { get; set; }

        public TradeBitCoinViewModel()
        {
            HistoryList = new List<TradeHistory>();
        }
    }
}
