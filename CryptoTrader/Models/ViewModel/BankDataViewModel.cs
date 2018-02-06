using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptoTrader.Models.ViewModel
{
    public class BankDataViewModel
    {

        public string BankName { get; set; } = "BawagPSK";

        public string ZipBank { get; set; } = "14000";

        public string Bic { get; set; } = "BAWAATWW";

        public string Iban { get; set; } = "AT34 1400 0001 2112 8475";

        public DateTime Created { get; set; } = DateTime.Now;

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Reference { get; set; }

        public string PersonIban { get; set; }

        public string PersonBic { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

    }
}