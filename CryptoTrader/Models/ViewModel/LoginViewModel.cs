using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CryptoTrader.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage ="Email nicht vorhanden")]
        public string LoginEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Remote( "IsPasswordTrue", "Validation", ErrorMessage = "Benutzername oder Kennwort nicht korrekt", AdditionalFields = "LoginEmail" )]
        public string LoginPassword { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }
    }
}