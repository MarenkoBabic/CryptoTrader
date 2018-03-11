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

        [Required]
        [MinLength(2, ErrorMessage = "Minbetrag 10 Euro")]
        [MaxLength(9, ErrorMessage = "Max 1 Milliarde")]
        public decimal Amount
        {
            get { return amount; }
            set
            {

                string pattern = "^[0-9]+$";
                if (Regex.IsMatch(amount.ToString(), pattern))
                {
                    amount = value;
                }
            }
        }

        [StringLength(3, ErrorMessage = "Maximale Länge", MinimumLength = 2)]
        public string Currency { get; set; }

        private decimal amount;
    }
}