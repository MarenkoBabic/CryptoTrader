namespace CryptoTrader.Model.ViewModel
{
    using CryptoTrader.Model.DbModel;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class PayInViewModel
    {
        public string BankName { get; set; } = "BawagPSK";

        public string ZipBank { get; set; } = "14000";

        public string Bic { get; set; } = "BAWAATWW";

        public string Iban { get; set; } = "AT34 1400 0001 2112 8475";

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Created { get; set; } = DateTime.Now;

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Reference { get; set; }

        public List<BankTransferHistory> BankHistoryList { get; set; }

        public PayInViewModel()
        {
            BankHistoryList = new List<BankTransferHistory>();
        }
    }
}