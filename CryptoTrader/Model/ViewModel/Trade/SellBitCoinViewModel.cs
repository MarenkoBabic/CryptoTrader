﻿namespace CryptoTrader.Model.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using CryptoTrader.Model.DbModel;

    public class SellBitCoinViewModel
    {
        public int PersonId { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int TickerId { get; set; }

        public decimal TickerRate { get; set; }

        [Remote("IsOnlyNumber", "Validation", ErrorMessage = "Nur Zahlen erlaubt")]
        public decimal SellTradeAmountBTC { get; set; }

        [Remote("IsOnlyNumber", "Validation", ErrorMessage = "Nur Zahlen erlaubt")]
        public decimal SellTradeAmountEuro { get; set; }

        public decimal BitCoinAmount { get; set; }

        public decimal BalanceAmount { get; set; }

        public List<TradeHistory> HistoryList { get; set; }

        public SellBitCoinViewModel()
        {
            HistoryList = new List<TradeHistory>();
        }
    }
}