using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CryptoTrader.Models.ViewModel
{
    public class RegisterViewModel
    {
        public int Person_id { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        [Required( AllowEmptyStrings = false )]
        [MinLength( 4, ErrorMessage = "Min 4 Zeichen" )]
        public string FirstName { get; set; }


        [Required( AllowEmptyStrings = false )]
        [MinLength( 4, ErrorMessage = " Min 4 Zeichen" )]
        public string LastName { get; set; }


        [Required( AllowEmptyStrings = false )]
        [EmailAddress]
        [Remote( "IsMailExistToRegister", "Validation", ErrorMessage = "Email schon vorhanden" )]
        public string Email { get; set; }


        [Required( AllowEmptyStrings = false )]
        [MinLength( 8, ErrorMessage = "Min 8 Zeichen" )]
        [DataType( DataType.Password )]
        public string Password { get; set; }


        public string Salt { get; set; }


        public bool Active { get; set; } = true;


        public string Role { get; set; } = "User";


        public decimal Status { get; set; } = 0;

    }
}