﻿namespace CryptoTrader.Manager
{
    using System;
    using System.Collections.Generic;
    using Jayrock.Json;
    using KrakenClient;
    using Newtonsoft.Json.Linq;

    public class ApiKraken
    {
        /// <summary>
        /// Holt sich aus der Api den Ticker raus
        /// </summary>
        /// <returns>Ticker als json object</returns>
        public static JsonObject TickerInfo()
        {
            KrakenClient client = new KrakenClient();
            JsonObject ticker = client.GetTicker( new List<string> { "XXBTZEUR" } );
            return ticker;
        }

        public static void TradeHistory()
        {
            var client = new KrakenClient();

            JsonObject tradesHistory = client.GetTradesHistory( string.Empty );
        }

        /// <summary>
        /// Holt sich den Letzen wert vom BTC-Kurs
        /// </summary>
        /// <returns>BTC -Kurs</returns>
        public static decimal GetTicker()
        {
            dynamic test = 0.0;
            JsonObject apiTicker = ApiKraken.TickerInfo();

            var getTicker = JObject.Parse( apiTicker.ToString() );

            foreach( JToken item in getTicker["result"] )
            {
                test = item.Last["c"][0];
            }
            return test;
        }

    }
}