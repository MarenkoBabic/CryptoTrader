namespace CryptoTrader.Manager
{
    public class TradeManager
    {
        /// <summary>
        /// Bitcoin kauf
        /// </summary>
        /// <param name="rate">TickerRate</param>
        /// <param name="amountBTC">BitconAnzahl</param>
        /// <returns>aktuellen Kontostand</returns>
        public static decimal TradeAmountByBTC(decimal rate, decimal amountBTC)
        {
            return rate * amountBTC;
        }

        /// <summary>
        /// Bitcoin verkaufen an Anzahl an Euros
        /// </summary>
        /// <param name="rate">TickerRate</param>
        /// <param name="amountBTC">BitconAnzahl</param>
        /// <returns>aktuellen Kontostand</returns>
        public static decimal GetBTCAmount(decimal rate, decimal amountEuro)
        {
            return amountEuro / rate;
        }

        /// <summary>
        /// Prüft ob genug Geld am Konto vorhanden ist
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="rate"></param>
        /// <param name="amountBTC"></param>
        /// <returns>Result</returns>
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

        /// <summary>
        /// Prüft ob genug Bitcoin am Konto vorhanden ist
        /// </summary>
        /// <param name="amountBTC"></param>
        /// <param name="SellBitCoin"></param>
        /// <returns> Result</returns>
        public static bool HaveEnoughBTC(decimal amountBTC, decimal SellBitCoin)
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