using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web;
using FunnelWeb.Eventing;
using FunnelWeb.Model;
using FunnelWeb.Settings;

namespace FunnelWeb.Extensions.CommentNotification
{
    public class CommentPostedListener : IEventListener
    {
        private readonly ISettingsProvider settingsProvider;

        public CommentPostedListener(ISettingsProvider settingsProvider)
        {
            this.settingsProvider = settingsProvider;
        }

        public void Handle(Event payload)
        {
            var commentDetails = payload as CommentPostedEvent;
            if (commentDetails == null)
                return;

            var settings = settingsProvider.GetSettings();
            if (!settings.CommentNotification)
            {
                return;
            }

            ThreadPool.QueueUserWorkItem(
                delegate
                    {
                        SendEmail(settings, commentDetails);
                    });
        }

        private void SendEmail(Settings.Settings settings, CommentPostedEvent commentDetails)
        {
            try
            {
                var client = new SmtpClient();
                client.EnableSsl = settings.SmtpUseSsl;
                client.Port = settings.SmtpPort;
                client.Host = settings.SmtpServer;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                if (!string.IsNullOrEmpty(settings.SmtpPassword))
                {
                    client.Credentials = new NetworkCredential(settings.SmtpUsername, settings.SmtpPassword);
                }

                var message = new MailMessage(settings.SmtpFromEmailAddress, settings.SmtpToEmailAddress);
                message.Subject = 
                    (commentDetails.Comment.IsSpam ? "[Spam] " : "") 
                    + "Comment on " + commentDetails.Entry.Title;

                message.Body = BuildMessageBody(commentDetails.Entry, commentDetails.Comment);
                message.IsBodyHtml = true;

                client.Send(message);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        private static string BuildMessageBody(Entry entry, Comment comment)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("<p>A comment was posted to the entry: {0}</p>", entry.Title).AppendLine();
            builder.AppendFormat("<p>Author: {0}</p>",  Encode(comment.AuthorName)).AppendLine();
            builder.AppendFormat("<p>Email: {0}</p>",  Encode(comment.AuthorEmail)).AppendLine();
            builder.AppendFormat("<p>Blog: {0}</p>",  Encode(comment.AuthorUrl)).AppendLine();
            builder.AppendFormat("<p>Comment:</p><pre>{0}</pre>", Encode(comment.Body)).AppendLine();
            return builder.ToString();
        }

        private static string Encode(string x)
        {
            return HttpUtility.HtmlEncode(x);
        }
    }
}
