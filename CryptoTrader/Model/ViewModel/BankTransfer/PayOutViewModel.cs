namespace CryptoTrader.Model.ViewModel
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PayOutViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Created { get; set; } = DateTime.Now;


        [Required]
        [Display(Name = "Iban")]
        public string PersonIban { get; set; }

        [Required]
        [Display(Name = "Bic")]
        public string PersonBic { get; set; }

        [Required]
        [Display(Name = "Betrag")]
        [Range(100, 1000000, ErrorMessage = "Min. 100 Euro")]
        public decimal Amount { get; set; }

        [DataType(DataType.Currency)]
        public string Currency { get; set; } = "Eur";
    }
}