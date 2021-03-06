﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace CryptoTrader.Manager
{
    public class Cookies
    {
        public static void CreateCookies(string email, string Role, string firstName, string lastName)
        {
            //Authorisierung vornehmen
            var authTicket = new FormsAuthenticationTicket(
                1,  //Ticketversion
                email, //Useridentifizierung
                DateTime.Now, //Zeitpunkt der Erstellung
                DateTime.Now.AddMinutes(0), //Wann das Ticket ablaeuft
                false,//Persistentes Ticket?
                Role//Userdata
                );

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            HttpCookie CookieLastName = new HttpCookie("lastName")
            {
                Value = lastName
            };
            HttpCookie CookieFirstName = new HttpCookie("firstName")
            {
                Value = firstName
            };
            HttpContext.Current.Response.Cookies.Add(CookieFirstName);
            HttpContext.Current.Response.Cookies.Add(CookieLastName);
            HttpContext.Current.Response.Cookies.Add(authCookie);
        }

    }
}