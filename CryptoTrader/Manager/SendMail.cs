namespace CryptoTrader.Manager
{
    using CryptoTrader.Model.ViewModel;
    using System.Net;
    using System.Net.Mail;

    public class SendMail
    {
        /// <summary>
        /// Sendet eine Bestätigung mail 
        /// </summary>
        /// <param name="toAddress">An wem  </param>
        /// <param name="fromAddress">Von wem </param>
        /// <param name="subject">Betreff</param>
        public static void SendEmail(string toAddress)
        {
            EmailSendViewModel emailSendData = new EmailSendViewModel();
            try
            {
                using (var mail = new MailMessage())
                {
                    //Inhalt in der Mail
                    mail.Body = emailSendData.MailBody;
                    mail.IsBodyHtml = true;
                    //Betreff Mail
                    mail.Subject = emailSendData.Subject;
                    //Von wem wird mitgeben Parameter
                    mail.From = new MailAddress(emailSendData.FromAddress);
                    //Beim Regestrieren angebenen Mail
                    mail.To.Add(new MailAddress(toAddress));

                    try
                    {
                        //Welchen Client er verwenden soll
                        using (var smtpClient = new SmtpClient("smtp.gmail.com", 465))
                        {
                            //Ob die verbindung verschlüsst sein soll
                            //smtpClient.EnableSsl = true;
                            smtpClient.UseDefaultCredentials = false;
                            //Ruft eigenes konto auf 
                            smtpClient.Credentials = new NetworkCredential("Deine EMAIL ALS SENDER", "DEIN PASSWORD VON EMAIL");
                            //Sendet mail
                            smtpClient.Send(mail);
                        }
                    }
                    finally
                    {
                        //Mail wieder löschen 
                        mail.Dispose();
                    }

                }
            }
            catch (SmtpFailedRecipientsException ex)
            {
                foreach (SmtpFailedRecipientException t in ex.InnerExceptions)
                {
                    SmtpStatusCode status = t.StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {

                        System.Threading.Thread.Sleep(5000);
                        //resend
                        //smtpClient.Send(message);
                    }
                }
            }

        }

    }
}