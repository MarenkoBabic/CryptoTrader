namespace CryptoTrader.Manager
{
    using System;

    public static class DateTimeHelper
    {
        ///https://www.fluxbytes.com/csharp/convert-datetime-to-unix-time-in-c/
        /// <summary>
        /// Convert a date time object to Unix time representation.
        /// </summary>
        /// <param name="datetime">The datetime object to convert to Unix time stamp.</param>
        /// <returns>Returns a numerical representation (Unix time) of the DateTime object.</returns>
        public static long ConvertToUnixTimeMs( DateTime datetime )
        {
            DateTime sTime = new DateTime( 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc );

            return (long)(datetime - sTime).TotalSeconds * 1000;
        }
        /// <summary>
        /// Convert Unix time value to a DateTime object.
        /// </summary>
        /// <param name="unixtime">The Unix time stamp you want to convert to DateTime.</param>
        /// <returns>Returns a DateTime object that represents value of the Unix time.</returns>
        public static DateTime UnixTimeMsToDateTime( long unixtime )
        {
            DateTime sTime = new DateTime( 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc );
            return sTime.AddMilliseconds( unixtime );
        }
    }
}
