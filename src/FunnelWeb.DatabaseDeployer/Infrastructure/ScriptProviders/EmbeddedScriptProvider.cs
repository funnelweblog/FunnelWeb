using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace FunnelWeb.DatabaseDeployer.Infrastructure.ScriptProviders
{
    public delegate string GetEmbeddedScriptNameCallback(int scriptVersionNumber);

    /// <summary>
    /// The default IScriptProvider implementation which retrieves upgrade scripts embedded in an assembly.
    /// </summary>
    public sealed class EmbeddedSqlScriptProvider : IScriptProvider
    {
        private readonly string sourceIdentifier;
        private readonly Assembly assembly;
        private readonly GetEmbeddedScriptNameCallback mapFileNameCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedSqlScriptProvider"/> class.
        /// </summary>
        /// <param name="sourceIdentifier">Display name of script provider</param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="mapFileNameCallback">The map file name callback.</param>
        public EmbeddedSqlScriptProvider(string sourceIdentifier, Assembly assembly, GetEmbeddedScriptNameCallback mapFileNameCallback)
        {
            this.sourceIdentifier = sourceIdentifier;
            this.assembly = assembly;
            this.mapFileNameCallback = mapFileNameCallback;
        }

        /// <summary>
        /// Gets the highest available script version.
        /// </summary>
        /// <returns></returns>
        public int GetHighestScriptVersion()
        {
            var resourceNames = new List<string>();
            resourceNames.AddRange(assembly.GetManifestResourceNames());
            var highestVersionNumber = 0;
            while (true)
            {
                var nextVersionNumber = highestVersionNumber + 1;
                var nextFileName = mapFileNameCallback(nextVersionNumber);
                if (!resourceNames.Contains(nextFileName))
                {
                    break;
                }
                highestVersionNumber++;
            }
            return highestVersionNumber;
        }

        public string SourceIdentifier
        {
            get { return sourceIdentifier; }
        }

        public Assembly SourceAssembly
        {
            get { return assembly; }
        }

        public string DisplayName
        {
            get { return sourceIdentifier; }
        }

        /// <summary>
        /// Gets the script for a given version number.
        /// </summary>
        /// <param name="versionNumber">The version number.</param>
        /// <returns></returns>
        public IScript GetScript(int versionNumber)
        {
            var scriptName = mapFileNameCallback(versionNumber);
            var contents = null as string;
            var resourceStream = assembly.GetManifestResourceStream(scriptName);
            using (var resourceStreamReader = new StreamReader(resourceStream))
            {
                contents = resourceStreamReader.ReadToEnd();
            }

            return new EmbeddedSqlScript(scriptName, contents, versionNumber, SourceIdentifier);
        }
    }
}