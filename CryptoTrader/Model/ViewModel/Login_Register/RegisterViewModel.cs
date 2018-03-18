namespace CryptoTrader.Model.ViewModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class RegisterViewModel
    {
        public int Person_id { get; set; }

        public string Salt { get; set; }

        public bool Active { get; set; } = true;

        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required(AllowEmptyStrings = false)]
        [StringLength(12,MinimumLength =4,ErrorMessage = "Min. 4 Zeichen")]
        public string FirstName { get; set; }


        [Required(AllowEmptyStrings = false)]
        [StringLength(12,MinimumLength =4, ErrorMessage = "Min. 4 Zeichen")]
        public string LastName { get; set; }


        [DataType(DataType.EmailAddress)]
        [Remote("IsMailExistToRegister", "Validation", ErrorMessage = "Email schon vergeben")]
        public string RegisterEmail { get; set; }


        [Required(AllowEmptyStrings = false)]
        [StringLength(15,MinimumLength =8,ErrorMessage = "Min 8 Zeichen")]
        [DataType(DataType.Password)]
        public string RegisterPassword { get; set; }

    }
}