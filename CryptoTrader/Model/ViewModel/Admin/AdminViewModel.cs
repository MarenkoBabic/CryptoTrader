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

        [MinLength(1,ErrorMessage ="Min. 1 Euro")]
        public decimal Amount { get; set; }

        [StringLength(3)]
        public string Currency { get; set; } = "Eur";

        public List<AdminViewModel> PersonList { get; set; }

        public List<AdminViewModel> FilterList { get; set; }

        public AdminViewModel()
        {
            PersonList = new List<AdminViewModel>();
            FilterList = new List<AdminViewModel>();
        }
    }
}