namespace CryptoTrader.Models.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class ApiViewModel
    {
        public DateTime created { get; set; } = DateTime.Now;
        public decimal Rate { get; set; }
        public string Currency_src { get; set; } = "Eur";
        public string Currency_trg { get; set; } = "BTC";

    }
}