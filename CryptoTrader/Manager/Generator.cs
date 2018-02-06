using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CryptoTrader.Manager
{
    public class Generator
    {
        public static string ReferencGenerator(int id)
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
    }
}