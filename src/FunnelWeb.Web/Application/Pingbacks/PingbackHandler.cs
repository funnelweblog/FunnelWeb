using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Autofac;
using FunnelWeb.Model;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Web.Application.Spam;
using NHibernate;

namespace FunnelWeb.Web.Application.Pingbacks
{
    public class PingbackHandler : XmlRpcHandler
    {
        

        protected override string ProcessRequest(string methodName, List<string> parameters)
        {
            Uri sourceUri;
            Uri targetUri;

            if (!Uri.TryCreate(parameters.First(), UriKind.Absolute, out sourceUri))
                throw new XmlRpcFaultException(16, "The sourceURI was not provided or was not in a valid format.");
            if (!Uri.TryCreate(parameters.Last(), UriKind.Absolute, out targetUri))
                throw new XmlRpcFaultException(32, "The targetURI was not provided or was not in a valid format.");

            var pageName = targetUri.AbsolutePath.Substring(1);
            if (pageName.LastIndexOf('/') > 2)
            {
                pageName = pageName.Substring(0, pageName.LastIndexOf('/'));
            }

            var session = DependencyResolver.Current.GetService<ISession>();
            var entryRepository = DependencyResolver.Current.GetService<IEntryRepository>();
            var spamChecker = DependencyResolver.Current.GetService<ISpamChecker>();
            var transaction = session.BeginTransaction(IsolationLevel.Serializable);
            try
            {
                // Ensure the link is for a page that exists
                var entry = entryRepository.GetEntry(pageName);
                if (entry == null)
                {
                    throw new XmlRpcFaultException(32, "The targetURI refers to a page that does not exist.");
                }

                // Ensure a pingback doesn't already exist
                var existing = entry.Pingbacks.FirstOrDefault(x => x.TargetUri.ToLowerInvariant() == sourceUri.ToString().ToLowerInvariant());
                if (existing != null)
                {
                    throw new XmlRpcFaultException(48, "A pingback for this URI has already been registered.");
                }

                var pingback = new Pingback();
                pingback.Entry = entry;
                pingback.TargetUri = sourceUri.ToString().ToLowerInvariant();
                pingback.TargetTitle = string.Empty;

                // Check the calling page
                try
                {
                    var request = WebRequest.Create(sourceUri);
                    var response = request.GetResponse();
                    var responseBody = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    if (!responseBody.Contains(parameters.Last()))
                    {
                        throw new XmlRpcFaultException(17, "The document at the sourceURI does not contain a link to the targetURI");
                    }

                    var find = Regex.Match(responseBody, "\\<title.*?\\>(.*?)\\</title\\>");
                    pingback.TargetTitle = find.Success ? HttpContext.Current.Server.HtmlDecode(find.Groups[1].Value) : sourceUri.ToString().ToLower();
                }
                catch (XmlRpcFaultException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new XmlRpcFaultException(50, string.Format("The server encountered problems attempting to connect to the sourceURI. {0}", ex.Message));
                }

                // Save the pingback
                spamChecker.Verify(pingback);
                entry.Pingbacks.Add(pingback);

                session.Flush();
                transaction.Commit();
            }
            catch (Exception) 
            {
                transaction.Rollback();
                throw;
            }
            
            return "Thanks, your pingback has been received.";
        }
    }
}
