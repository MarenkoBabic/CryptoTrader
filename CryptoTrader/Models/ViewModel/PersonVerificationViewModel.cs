using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CryptoTrader.Models.ViewModel
{
    public class PersonVerificationViewModel
    {
        public int Person_id { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public int Address_id { get; set; }

        [Required( AllowEmptyStrings = false )]
        public string Street { get; set; }

        [Required( AllowEmptyStrings = false )]
        public string Number { get; set; }

        public int City_id { get; set; }

        [Required( AllowEmptyStrings = false )]
        public string Zip { get; set; }

        [Required( AllowEmptyStrings = false )]
        public string CityName { get; set; }

        public int Country_id { get; set; }

        public string CountryName { get; set; }

        public List<SelectListItem> CountryList { get; set; }

        public string Country_Iso { get; set; }

        public bool Status { get; set; } = true;

        public HttpPostedFileBase Path { get; set; }

        public string Reference { get; set; }


    }
}