using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptoTrader.Manager
{
    using CryptoTrader.Model.DbModel;
    public class BankTransferManager
    {
        public static bool HaveHistory(int id)
        {
            using(var db = new CryptoTraderEntities())
            {
            }
            return false;
        }
    }
}