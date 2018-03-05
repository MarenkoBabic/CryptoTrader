namespace CryptoTrader.Manager
{
    using CryptoTrader.Models.DbModel;
    using System;
    using System.Linq;
    public class TickerManager
    {
        /// <summary>
        /// Holt den letzten Wert vom Ticker
        /// </summary>
        /// <returns>Tickerwert</returns>
        public static decimal GetTicker()
        {
            using (var db = new CryptoEntities())
            {
                var ticker = db.Ticker.OrderByDescending(a => a.id).Select(a => a.rate).First();
                return Math.Round(ticker, 2);
            }
        }
   }
}
