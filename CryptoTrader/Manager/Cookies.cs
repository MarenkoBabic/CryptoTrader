using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace CryptoTrader.Manager
{
    public class Cookies
    {
        public static void CreateCookies( string email, string Role, string firstName, string lastName )
        {
            //Authorisierung vornehmen
            var authTicket = new FormsAuthenticationTicket(
                1,  //Ticketversion
                email, //Useridentifizierung
                DateTime.Now, //Zeitpunkt der Erstellung
                DateTime.Now.AddMinutes( 30 ), //Wann das Ticket ablaeuft
                false,//Persistentes Ticket?
                Role//Userdata
                );


            string encryptedTicket = FormsAuthentication.Encrypt( authTicket );

            var authCookie = new HttpCookie( FormsAuthentication.FormsCookieName, encryptedTicket );
            authCookie.Expires = DateTime.Now.AddMinutes( 30 );
            HttpCookie CookieLastName = new HttpCookie( "lastName" );
            CookieLastName.Value = lastName;
            CookieLastName.Expires = DateTime.Now.AddMinutes( 30 );


            HttpCookie CookieFirstName = new HttpCookie( "firstName" );
            CookieFirstName.Value = firstName;
            CookieFirstName.Expires = DateTime.Now.AddMinutes( 30 );

            HttpContext.Current.Response.Cookies.Add( CookieFirstName );
            HttpContext.Current.Response.Cookies.Add( CookieLastName );
            HttpContext.Current.Response.Cookies.Add( authCookie );
        }

    }
}