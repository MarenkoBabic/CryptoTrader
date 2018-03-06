using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptoTrader.Models.ViewModel
{
    public class EmailSendViewModel
    {
        public string FromAddress { get; set; }
        public string MailBody { get; set; }
        public string Subject { get; set; }


        public EmailSendViewModel()
        {
            //Inhalt in der Mail
            this.MailBody = "<a href=http://www.yourwebsitename.com/verificationpage.aspx?custid=Session> click here to verify</a> Test";
            //Betreff Mail
            this.Subject = "Verification";
            //Von wem wird mitgeben Parameter
            FromAddress = "Marenko.Babic@qualifizierung.or.at";
        }
    }
}