//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CryptoTrader.Models.DbModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class TradeHistory
    {
        public int id { get; set; }
        public System.DateTime created { get; set; }
        public int person_id { get; set; }
        public decimal amount { get; set; }
        public int ticker_id { get; set; }
    
        public virtual Person Person { get; set; }
        public virtual Ticker Ticker { get; set; }
    }
}
