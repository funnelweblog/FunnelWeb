using System;
using System.Runtime.Serialization;

namespace FunnelWeb
{
    [Serializable]
    public class FunnelWebExtensionLoadException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public FunnelWebExtensionLoadException()
        {
        }

        public FunnelWebExtensionLoadException(string message) : base(message)
        {
        }

        public FunnelWebExtensionLoadException(string message, Exception inner) : base(message, inner)
        {
        }

        protected FunnelWebExtensionLoadException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}