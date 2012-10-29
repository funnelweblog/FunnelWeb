using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.XPath;

namespace FunnelWeb.Web.Application.Pingbacks
{
    public abstract class XmlRpcHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var requestMessage = new XPathDocument(context.Request.InputStream);
                var requestNavigator = requestMessage.CreateNavigator();
                var methodName = requestNavigator.SelectSingleNode("/methodCall/methodName").Value;
                var parameters = requestNavigator.Select("/methodCall/params/param/value/string");
                var values = (parameters.Cast<XPathNavigator>().Select(param => param.Value)).ToList();

                var response = ProcessRequest(methodName, values);

                context.Response.Clear();
                context.Response.Write(string.Format(@"<?xml version='1.0'?>
<methodResponse>
 <params>
  <param>
   <value><string>{0}</string></value>
  </param>
 </params>
</methodResponse>", HttpContext.Current.Server.HtmlEncode(response)));

            }
            catch (XmlRpcFaultException ex)
            {
                WriteFault(context, ex);
            } 
            catch (Exception ex)
            {
                WriteFault(context, new XmlRpcFaultException(0, string.Format("The request could not be processed. It may not have been a valid XML-RPC request. {0}", ex.Message)));
            }
        }

        private void WriteFault(HttpContext context, XmlRpcFaultException exception)
        {
            context.Response.Clear();
            context.Response.Write(string.Format(@"<?xml version='1.0'?>
<methodResponse>
   <fault>
      <value>
         <struct>
            <member>
               <name>faultCode</name>
               <value><int>{0}</int></value>
               </member>
            <member>
               <name>faultString</name>
               <value><string>{1}</string></value>
               </member>
            </struct>
         </value>
      </fault>
   </methodResponse>", exception.FaultCode, HttpContext.Current.Server.HtmlEncode(exception.FaultMessage)));
        }

        protected abstract string ProcessRequest(string methodName, List<string> parameters);

        public bool IsReusable
        {
            get { return true; }
        }
    }
}