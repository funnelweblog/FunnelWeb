using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web.Management;
using Bindable;
using System.Web;

namespace FunnelWeb.Web.Application.HealthMonitoring
{
    public class SimpleMailWithSslWebEventProvider : WebEventProvider
    {
        private string _smtpHost;
        private int _smtpPort;
        private string _username;
        private string _password;
        private string _from;
        private string _to;
        private bool _useSSL;

        public SimpleMailWithSslWebEventProvider()
        {

        }

        public override void Initialize(string name, NameValueCollection config)
        {
            _smtpHost = config.GetAndRemove("host", "");
            _smtpPort = config.GetAndRemove("port", 0);
            _useSSL = config.GetAndRemove("enableSSL", false);
            _username = config.GetAndRemove("userName", "");
            _password = config.GetAndRemove("password", "");
            _from = config.GetAndRemove("from", "");
            _to = config.GetAndRemove("to", "");
        }

        public override void Flush()
        {
        }

        public override void ProcessEvent(WebBaseEvent raisedEvent)
        {
            var email = new MailMessage(_from, _to);
            email.Body = GenerateMessage(raisedEvent);
            email.IsBodyHtml = true;
            email.Subject = GenerateSubject(raisedEvent);

            var client = new SmtpClient();
            if (!string.IsNullOrEmpty(_smtpHost)) client.Host = _smtpHost;
            if (_smtpPort != 0) client.Port = _smtpPort;
            client.EnableSsl = _useSSL;
            if (!string.IsNullOrEmpty(_username) || !string.IsNullOrEmpty(_password)) client.Credentials = new NetworkCredential(_username, _password);

            ThreadPool.QueueUserWorkItem(thread => client.SendAsync(email, null));
        }

        private string GenerateSubject(WebBaseEvent raisedEvent)
        {
            var errorEvent = raisedEvent as WebBaseErrorEvent;
            var result = errorEvent != null ? errorEvent.ErrorException.Message : raisedEvent.Message;
            if (result.Contains('\n'))
            {
                return result.Split('\n').Select(x => x.Trim()).First();
            }
            return result;
        }

        private string GenerateMessage(WebBaseEvent raisedEvent)
        {
            var context = HttpContext.Current;
            var writer = new StringBuilder();
            writer.AppendLine("<strong><big>Health Monitoring</big></strong>");
            writer.AppendLine("<p>").AppendLine(raisedEvent.Message).AppendLine("</p>");
            writer.AppendLine("<table>");
            if (context != null)
            {
                WriteRow(writer, "Request", context.Request.Url);
                WriteRow(writer, "Referred by", context.Request.Headers["Referer"]);
                WriteRow(writer, "User Agent", context.Request.UserAgent);
                WriteRow(writer, "User Host Name", context.Request.UserHostName);
                WriteRow(writer, "User Host Address", context.Request.UserHostAddress);
            }
            foreach (var property in raisedEvent.GetType().GetProperties())
            {
                WriteRow(writer, property.Name, ReadValue(() => property.GetValue(raisedEvent, null)));
            }
            writer.AppendLine("</table>");
            return writer.ToString();
        }

        private static void WriteRow(StringBuilder writer, object key, object value)
        {
            writer.AppendLine("<tr>");
            writer.AppendLine("<td style='font-weight: bold'>" + key + "</td>");
            writer.AppendLine("<td>" + value + "</td>");
            writer.AppendLine("</tr>");
        }

        private string ReadValue(Func<object> evaluate)
        {
            try
            {
                return evaluate().ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }


        public override void Shutdown()
        {
        }
    }
}