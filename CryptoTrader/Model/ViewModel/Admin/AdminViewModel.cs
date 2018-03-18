namespace CryptoTrader.Model.ViewModel
{
    using CryptoTrader.Model.DbModel;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

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

        [Range(100,10000,ErrorMessage ="Min. 100 Euro")]
        public decimal Amount { get; set; }

        [StringLength(3)]
        public string Currency { get; set; } = "Eur";

        public List<Person> PersonList { get; set; }

        public List<Person> FilterList { get; set; }

        public AdminViewModel()
        {
            PersonList = new List<Person>();
        }
    }
}