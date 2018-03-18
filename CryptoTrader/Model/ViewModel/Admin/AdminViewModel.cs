namespace CryptoTrader.Model.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using CryptoTrader.Model.DbModel;

    public class AdminViewModel
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Created { get; set; } = DateTime.Now;

        public int PersonId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Reference { get; set; }

        public bool Active { get; set; }

        public decimal Amount { get; set; }

        [StringLength(3)]
        public string Currency { get; set; } = "Eur";

        public List<Person> PersonList { get; set; }

        public AdminViewModel()
        {
            PersonList = new List<Person>();
        }
    }
}