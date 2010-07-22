using System;

namespace FunnelWeb.Web.Application.Pingbacks
{
    public class XmlRpcFaultException : Exception
    {
        private readonly int _faultCode;
        private readonly string _faultMessage;

        public XmlRpcFaultException(int faultCode, string faultMessage)
        {
            _faultCode = faultCode;
            _faultMessage = faultMessage;
        }

        public string FaultMessage
        {
            get { return _faultMessage; }
        }

        public int FaultCode
        {
            get { return _faultCode; }
        }
    }
}