using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Helper
{
    public class EmailSettings
    {
        public static void SendEmail(Email email) 
        {
            try
            {
                var client = new SmtpClient("smtp.sendgrid.net", 587);
                client.EnableSsl = true;//Encrypted
                client.Credentials = new NetworkCredential("apikey", "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");
                client.Send("dogaryahmed2017@gmail.com", email.To, email.Title, email.Body);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }  
    }
}
