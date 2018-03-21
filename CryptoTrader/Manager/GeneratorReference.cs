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
            Random rnd = new Random();
            string temp = "";

            for (int i = 0; i < 10; i++)
            {
                temp += rnd.Next(1,10).ToString();
            }
            string reference = temp;
            return reference;
        }
        public static string ReferencGenerator2()
        {
            Guid uid = Guid.NewGuid();

            return uid.ToString();
        }
    }
}