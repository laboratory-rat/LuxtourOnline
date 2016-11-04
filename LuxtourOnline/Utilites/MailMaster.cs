using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace LuxtourOnline.Utilites
{
    public static class MailMaster
    {
        public static readonly string Server = "mail.madrat-studio.tk";
        public static readonly int Port = 25;
        public static readonly string User = "info@madrat-studio.tk";
        public static readonly string Pass = "os9090()";
        public static readonly string DefaultHeading = "Luxtour Online Info";


        private static SmtpClient _client { get; set; } = null;
        public static SmtpClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = new SmtpClient(Server, Port);

                    _client.UseDefaultCredentials = false;

                    _client.Credentials = new NetworkCredential(User, Pass);

                    _client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    _client.EnableSsl = false;
                }

                return _client;
            }

            set
            {
                _client = value;
            }
        }

        public static void SendRegisterEmail(string to, string homeLink)
        {

        }

        public async static Task SendMailAsync(List<string> to, string heading, string subject, string body)
        {
            foreach(string s in to)
            {
                await SendMailAsync(s, heading, subject, body);
                await Task.Delay(100);
            }
        }

        public static async Task SendMailAsync(string to, string heading, string subject, string body, string CCAddr = "")
        {
            if (string.IsNullOrEmpty(to))
                throw new ArgumentNullException("argument 'to' is empty");

            var client = Client;

            if (string.IsNullOrEmpty(heading))
                heading = DefaultHeading;

            MailMessage mail = new MailMessage();

            //Setting From , To and CC
            mail.From = new MailAddress(User, heading);
            mail.To.Add(new MailAddress(to));

            mail.Subject = subject;
            mail.Body = body;

            mail.IsBodyHtml = true;

            if (!string.IsNullOrEmpty(CCAddr))
                mail.CC.Add(new MailAddress(CCAddr));

            await client.SendMailAsync(mail);
        }


        public static void SendMail(string to, string heading, string subject, string body, string CCAddr = "")
        {
            if (string.IsNullOrEmpty(to))
                throw new ArgumentNullException("argument 'to' is empty");

            var client = Client;

            if (string.IsNullOrEmpty(heading))
                heading = DefaultHeading;

            MailMessage mail = new MailMessage();

            //Setting From , To and CC
            mail.From = new MailAddress(User, heading);
            mail.To.Add(new MailAddress(to));

            mail.Subject = subject;
            mail.Body = body;

            mail.IsBodyHtml = true;

            if (!string.IsNullOrEmpty(CCAddr))
                mail.CC.Add(new MailAddress(CCAddr));

             client.Send(mail);
            
        }
    }
}