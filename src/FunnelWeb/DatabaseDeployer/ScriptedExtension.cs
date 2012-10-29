using System.Reflection;
using DbUp.Engine;

namespace FunnelWeb.DatabaseDeployer
{
    public class ScriptedExtension
    {
        private readonly string sourceIdentifier;
        private readonly Assembly sourceAssembly;
        private readonly IScriptProvider scriptProvider;

        public ScriptedExtension(string sourceIdentifier, Assembly sourceAssembly, IScriptProvider scriptProvider)
        {
            this.sourceIdentifier = sourceIdentifier;
            this.sourceAssembly = sourceAssembly;
            this.scriptProvider = scriptProvider;
        }

        public Assembly SourceAssembly
        {
            get { return sourceAssembly; }
        }

        public string SourceIdentifier
        {
            get { return sourceIdentifier; }
        }

        public IScriptProvider ScriptProvider
        {
            get { return scriptProvider; }
        }
    }
}
