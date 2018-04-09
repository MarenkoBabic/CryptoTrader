namespace CryptoTrader.Model.ViewModel
{
    using System;
    using System.Collections.Generic;

    public class AdminViewModel
    {
        public DateTime Created { get; set; } = DateTime.Now;

        public int PersonId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Reference { get; set; }

        public bool Active { get; set; }

        public decimal Amount { get; set; }

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