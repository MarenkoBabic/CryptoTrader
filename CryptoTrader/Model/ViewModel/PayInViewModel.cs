using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CryptoTrader.Model.DbModel;

namespace CryptoTrader.Models.ViewModel
{
    public class PayInViewModel
    {
        public string BankName { get; set; } = "BawagPSK";

        public string ZipBank { get; set; } = "14000";

        public string Bic { get; set; } = "BAWAATWW";

        public string Iban { get; set; } = "AT34 1400 0001 2112 8475";

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