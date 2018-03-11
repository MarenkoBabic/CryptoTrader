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
        /// <param name="amount">Kontostand</param>
        /// <param name="rate">TickerRate</param>
        /// <param name="amountBTC">BitconAnzahl</param>
        /// <returns>aktuellen Kontostand</returns>
        public static decimal BuyBitCoin(decimal amount, decimal rate, decimal amountBTC)
        {
            decimal tempAmount = amount -( rate * amountBTC);
            tempAmount = tempAmount * (-1);
            return tempAmount;
        }

        /// <summary>
        /// Bitcoin verkaufen
        /// </summary>
        /// <param name="amount">Kontostand</param>
        /// <param name="rate">TickerRate</param>
        /// <param name="amountBTC">BitconAnzahl</param>
        /// <returns>aktuellen Kontostand</returns>
        public static decimal SellBitCoin(decimal amount, decimal rate, decimal amountBTC)
        {
            amount += rate * amountBTC;
            return amount;
        }
        /// <summary>
        /// Bitcoin kauf per Anzahl an Euros
        /// </summary>
        /// <param name="amount">Kontostand</param>
        /// <param name="rate">TickerRate</param>
        /// <param name="amountBTC">BitconAnzahl</param>
        /// <returns>aktuellen Kontostand</returns>
        public static decimal BuyBitCoinPerEuro(decimal amount, decimal rate, decimal amountBTC)
        {
            amount -= rate * amountBTC;
            return amount;
        }

        /// <summary>
        /// Bitcoin verkaufen an Anzahl an Euros
        /// </summary>
        /// <param name="amount">Kontostand</param>
        /// <param name="rate">TickerRate</param>
        /// <param name="amountBTC">BitconAnzahl</param>
        /// <returns>aktuellen Kontostand</returns>
        public static decimal SellBitCoinPerEuro(decimal amount, decimal rate, decimal amountBTC)
        {
            amount += rate * amountBTC;
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

        public static bool HaveEnoughBTC(decimal SellBitCoin,decimal amountBTC)
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