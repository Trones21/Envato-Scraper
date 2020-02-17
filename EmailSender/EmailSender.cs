using System;
using System.Net.Mail;


namespace NS_EmailSender
{
    //PK_Curious - Do I have any kind of error checking... I would likely have this with my
    // service provider, which i will likely need for over 500/day. Maybe they have an API too??
    public class EmailSender
    {
        public string SavedMessageBody;
        public bool IsHTML;
        //public HTMLNode PK_ToDo What object type should I use to hold this? IWS HTMLNode?
        public void SendMail(string Subject, string MessageBody)
        {
            try
            {
                MailMessage mail = new MailMessage();

                var SmtpServer = new SmtpClient("smtp.mail.yahoo.com", 587)
                {
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                    Credentials = new System.Net.NetworkCredential("trones.mailserver@yahoo.com", "myMailServer1"),

                };

                mail.From = new MailAddress("trones.mailserver@yahoo.com");
                mail.To.Add("trones.de@gmail.com");
                mail.Subject = Subject;
                mail.Body = MessageBody;

                SmtpServer.Send(mail);

                Console.WriteLine("Mail Sent");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        /// <summary>
        /// Choose an Email address to send to
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="MessageBody"></param>
        /// <param name="toAddress"></param>
        public void SendMail(string Subject, string MessageBody, string toAddress)
        {
            try
            {
                MailMessage mail = new MailMessage();

                var SmtpServer = new SmtpClient("smtp.mail.yahoo.com", 587)
                {
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                    Credentials = new System.Net.NetworkCredential("trones.mailserver@yahoo.com", "myMailServer1"),

                };

                mail.From = new MailAddress("trones.mailserver@yahoo.com");
                mail.To.Add(toAddress);
                mail.Subject = Subject;
                mail.Body = MessageBody;

                SmtpServer.Send(mail);

                Console.WriteLine("Mail Sent");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        /// <summary>
        /// Consider using Razor in the future
        /// </summary>
        /// <param name="TemplatePath"></param>
        /// <param name="requiredobject"></param>

        public void SendMail(bool IsHTML, string Subject)
        {

        }
        public void SetHTML(Template template, object requiredobject)
        {
            
        }

        public void AddText(string Text)
        {
            SavedMessageBody = SavedMessageBody + Text;
        }
    public enum Template
    {
        //NewAuthor Congrats
        //Rank Congrats
        //Item threshold Congrats - You are in the top X% of authors 
    }
    }
}

