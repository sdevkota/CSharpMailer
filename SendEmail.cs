using System;
using System.Net.Mail;
using System.Net;
using System.Collections.Generic;
namespace SendEmail
{
    public class Program
    {
        //Client
        static void Main(string[] args)
        {
            var email = new Email
            {
                from = new MailAddress("mrtorilahure@gmail.com", "Vegan32"),
                body = "<h1>This is a test email</h1><br/><p>You can add stuffs here.<p/>",
                subject = "This is a test email.",
                to = new List<String>()
                {
                    "email3@gmail.com",
                    "email4@gmail.com"
                },
                cc = new List<String>()
                {
                    "email1@gmail.com",
                    "email2@yahoo.com"
                }
            };
            MailSender mail = new MailSender(email);
            Console.WriteLine(mail.Send());
        }
    }
    /// <summary>
    /// This class is a wrapper class for sending out email 
    /// </summary>
    public class MailSender
    {
        private Email _email;
        public MailSender(Email Email)
        {
            _email = Email;

        }
        //send method
        public String Send()
        {
            String message;
            try
            {
                //setup the smtp stuffs
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    //For Web app, it is better to use web.config to get these values instead
                    //ConfigurationManager.AppSettings["HostName"];
                    //ConfigurationManager.AppSettings["Password"];
                    Credentials = new NetworkCredential(_email.from.Address, "AddHostPassword"),
                    Timeout = 20000
                };

                //just to ensure that we do not get an invalid emmail object
                if (_email.to.Count>0 && _email.from!=null)
                {
                    var mail = new MailMessage();
                    mail.From = _email.from;
                    foreach (var email in _email.to)
                    {
                        mail.To.Add(email);
                    }
                    if (_email.cc.Count>0)
                    {
                        foreach (var email in _email.cc)
                        {
                            mail.CC.Add(email);
                        }
                    }
                    mail.Subject = _email.subject;
                    mail.IsBodyHtml = true;
                    mail.Body = _email.body;
                    smtp.Send(mail);
                    message = "We have received your email. Alllow us about 7-10 days to process your request.";

                }
                else{
                    message = "At least one FROM and TO address is required to send email.";
                }
            }
            catch (Exception e)
            {
                //you can implement any sort of Exception mechanism. I am just using the message. 
                message = "We are unable to send email at this moment. Please try again later.";

            }
            return message;

        }
    }
    /// <summary>
    /// This represent an email item 
    /// </summary>
    public class Email
    {
        public String smtpClient { get; set; }
        public MailAddress from { get; set; }
        public List<String> to { get; set; }
        public List<String> cc { get; set; }
        public String subject { get; set; }
        public String body { get; set; }

    }
}