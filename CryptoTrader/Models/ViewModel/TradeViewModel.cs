using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptoTrader.Models.ViewModel
{
    public class TradeViewModel
    {
        public int PersonId { get; set; }
        public decimal TickerRate { get; set; }
        public int TickerId { get; set; }
        public DateTime Created { get; set; }
        public decimal BuyAmount { get; set; }
        public decimal SellAmount { get; set; }
        public string Currency_Src_Buy{ get; set; }
        public string Currency_Trg_Buy { get; set; }

        public string Currency_Src_Sell { get; set; }
        public string Currency_Trg_Sell { get; set; }

    }
}