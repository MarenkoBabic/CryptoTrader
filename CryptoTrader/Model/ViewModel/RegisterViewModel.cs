namespace CryptoTrader.Models.ViewModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class RegisterViewModel
    {
        public int Person_id { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        [Required(AllowEmptyStrings = false)]
        [MinLength(4, ErrorMessage = "Min 4 Zeichen")]
        public string FirstName { get; set; }


        [Required(AllowEmptyStrings = false)]
        [MinLength(4, ErrorMessage = " Min 4 Zeichen")]
        public string LastName { get; set; }


        [EmailAddress]
        [Remote("IsMailExistToRegister", "Validation", ErrorMessage = "Email schon vorhanden")]
        public string RegisterEmail { get; set; }


        [Required(AllowEmptyStrings = false)]
        [MinLength(8, ErrorMessage = "Min 8 Zeichen")]
        [DataType(DataType.Password)]
        public string RegisterPassword { get; set; }


        public string Salt { get; set; }

        public bool Active { get; set; } = true;

    }
}