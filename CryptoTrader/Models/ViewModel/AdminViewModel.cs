using CryptoTrader.Models.DbModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CryptoTrader.Models.ViewModel
{
    public class AdminViewModel
    {
        public DateTime Created { get; set; } = DateTime.Now;
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Reference { get; set; }
        public bool Active { get; set; }
        public decimal Amount { get; set; }
        [StringLength(3)]
        public string Currency { get; set; } = "Eur";

        public List<Person> personList { get; set; }

        public AdminViewModel()
        {
            personList = new List<Person>();
        }

    }
}