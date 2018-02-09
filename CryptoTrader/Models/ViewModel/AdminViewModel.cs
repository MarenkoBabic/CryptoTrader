using CryptoTrader.Models.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptoTrader.Models.ViewModel
{
    public class AdminViewModel
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Reference { get; set; }
        public bool Active { get; set; }

        public int BalanceId { get; set; }
        public decimal BalanceAmount { get; set; }
        public string BalanceCurrency { get; set; }

        public int BankTransferHistoryId { get; set; }
        public decimal BankTransferHistoryAmount { get; set; }
        public string BankTransferHistoryCurrency { get; set; }


    }
}