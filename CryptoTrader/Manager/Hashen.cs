using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CryptoTrader.Manager
{
    public static class Hashen
    {
        /// <summary>
        /// Erzeugt ein Salt für Password
        /// </summary>
        /// <returns>Salt</returns>
        public static string SaltErzeugen()
        {
            byte[] salt = new byte[32];
            //Random Number Generator 
            var rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes( salt );
            return HexString( salt );
        }

        /// <summary>
        /// Hash berechnen für Login
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string HashBerechnen( string s )
        {
            if( !string.IsNullOrEmpty( s ) )
            {
                //Daten (in unserem Fall ein String) in ein ByteArray umwandeln 
                byte[] bytes = Encoding.UTF8.GetBytes( s );
                using( SHA256 sha = new SHA256Managed() )
                {
                    byte[] hash = sha.ComputeHash( bytes );
                    return HexString( hash );
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static string HexString( byte[] bytes )
        {
            var hashSB = new StringBuilder( 64 );
            foreach( byte b in bytes )
            {
                hashSB.Append( b.ToString( "X2" ) );
            }
            return hashSB.ToString();
        }


    }
}