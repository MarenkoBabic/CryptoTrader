using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptoTrader.Models.ViewModel
{
    public class TickerChartViewModel
    {
        public string UnixTime { get; set; }
        public string Value { get; set; }

        public static List<decimal[]> GetList( List<TickerChartViewModel> list )
        {
            List<decimal[]> result = new List<decimal[]>();
            int i = 0;
            foreach( TickerChartViewModel item in list )
            {
                result.Add( new decimal[2] );
                result.Last()[0] = decimal.Parse( item.UnixTime );
                result.Last()[1] = decimal.Parse( item.Value );
                i++;
            }
            return result;
        }
    }
}