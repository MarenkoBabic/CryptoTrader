using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CryptoTrader.Manager
{
    public class GeneratorReference
    {

        public static string ReferencGenerator()
        {
            Guid uid = Guid.NewGuid();

            return uid.ToString();
        }
    }
}