namespace CryptoTrader.Models.ViewModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    public class PayOutViewModel
    {
        public DateTime Created { get; set; } = DateTime.Now;

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PersonIban { get; set; }

        public string PersonBic { get; set; }
        
        public decimal Amount { get; set; }

        public string Currency { get; set; } = "Eur";
    }
}