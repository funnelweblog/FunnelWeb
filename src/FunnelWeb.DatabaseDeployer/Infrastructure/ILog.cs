using System;
using System.IO;

namespace FunnelWeb.DatabaseDeployer.Infrastructure
{
    public class Log : ILog
    {
        private readonly StringWriter _writer = new StringWriter();

        public void WriteInformation(string format, params object[] args)
        {
            _writer.WriteLine("<li class='log-information'>");
            _writer.WriteLine(format, args);
            _writer.WriteLine("</li>");
        }

        public void WriteError(string format, params object[] args)
        {
            _writer.WriteLine("<li class='log-error'>");
            _writer.WriteLine(format, args);
            _writer.WriteLine("</li>");
        }

        public void WriteWarning(string format, params object[] args)
        {
            _writer.WriteLine("<li class='log-warning'>");
            _writer.WriteLine(format, args);
            _writer.WriteLine("</li>");
        }

        public IDisposable Indent()
        {
            return new IndentedLog(_writer);
        }

        private class IndentedLog : IDisposable
        {
            private readonly StringWriter _writer;

            public IndentedLog(StringWriter writer)
            {
                _writer = writer;
                _writer.WriteLine("<ol>");
            }

            public void Dispose()
            {
                _writer.WriteLine("</ol>");
            }
        }

        public override string ToString()
        {
            return _writer.ToString();
        }
    }

    public interface ILog
    {
        void WriteInformation(string format, params object[] args);
        void WriteError(string format, params object[] args);
        void WriteWarning(string format, params object[] args);
        IDisposable Indent();
    }
}
