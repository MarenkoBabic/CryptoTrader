namespace CryptoTrader.Manager
{
    using Jayrock.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using KrakenClient;

    public class ApiKraken
    {
        public static JsonObject TickerInfo()
        {
            KrakenClient client = new KrakenClient();
            JsonObject ticker = client.GetTicker(new List<string> { "XXBTZEUR" });
            return ticker;
        }

        public static void TradeHistory()
        {
            var client = new KrakenClient();

            JsonObject tradesHistory = client.GetTradesHistory(string.Empty);
        }

    }
}