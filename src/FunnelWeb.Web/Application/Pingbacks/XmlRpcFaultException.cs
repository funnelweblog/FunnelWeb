using System;

namespace FunnelWeb.Web.Application.Pingbacks
{
    public class XmlRpcFaultException : Exception
    {
        private readonly int faultCode;
        private readonly string faultMessage;

        public XmlRpcFaultException(int faultCode, string faultMessage)
        {
            this.faultCode = faultCode;
            this.faultMessage = faultMessage;
        }

        public string FaultMessage
        {
            get { return faultMessage; }
        }

        public int FaultCode
        {
            get { return faultCode; }
        }
    }
}