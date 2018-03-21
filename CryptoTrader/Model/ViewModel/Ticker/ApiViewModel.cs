namespace CryptoTrader.Model.ViewModel
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ApiViewModel
    {
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Created { get; set; } = DateTime.Now;

        public decimal Rate { get; set; }

        public string Currency_src { get; set; } = "Euro";

        public string Currency_trg { get; set; } = "BTC";

    }
}