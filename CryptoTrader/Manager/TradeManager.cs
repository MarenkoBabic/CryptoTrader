namespace CryptoTrader.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class TradeManager
    {
        /// <summary>
        /// Bitcoin kauf
        /// </summary>
        /// <param name="rate">TickerRate</param>
        /// <param name="amountBTC">BitconAnzahl</param>
        /// <returns>aktuellen Kontostand</returns>
        public static decimal BuyBitCoin(decimal rate, decimal amountBTC)
        {
            decimal amount = rate * amountBTC *(-1);
            return amount;
        }

        /// <summary>
        /// Bitcoin kauf per Euros
        /// </summary>
        /// <param name="rate">TickerRate</param>
        /// <param name="amountMoney">Euros</param>
        /// <returns>aktuellen Kontostand</returns>
        public static decimal BuyBitCoinPerEuro(decimal rate, decimal amountMoney)
        {
            decimal amount = amountMoney / rate;
            return amount;
        }

        /// <summary>
        /// Bitcoin verkaufen
        /// </summary>
        /// <param name="rate">TickerRate</param>
        /// <param name="amountBTC">BitconAnzahl</param>
        /// <returns>aktuellen Kontostand</returns>
        public static decimal SellBitCoin(decimal rate, decimal amountBTC)
        {
            decimal amount = rate * amountBTC;
            return amount;
        }

        /// <summary>
        /// Bitcoin verkaufen an Anzahl an Euros
        /// </summary>
        /// <param name="rate">TickerRate</param>
        /// <param name="amountBTC">BitconAnzahl</param>
        /// <returns>aktuellen Kontostand</returns>
        public static decimal SellBitCoinPerEuro(decimal rate, decimal amountBTC)
        {
            decimal amount = rate * amountBTC;
            return amount;
        }

        /// <summary>
        /// Prüft ob genug geld vorhanden ist 
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="rate"></param>
        /// <param name="amountBTC"></param>
        /// <returns></returns>
        public static bool HaveEnoughMoney(decimal amount, decimal rate, decimal BuyBitCoin)
        {
            if (amount < (rate * BuyBitCoin))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool HaveEnoughBTC(decimal amountBTC,decimal SellBitCoin)
        {
            if (amountBTC < SellBitCoin)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}