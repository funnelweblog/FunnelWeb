using System.Reflection;

namespace FunnelWeb.DatabaseDeployer.Infrastructure.ScriptProviders
{
    /// <summary>
    /// Provides scripts to be executed and tracks whether they have been executed.
    /// </summary>
    public interface IScriptProvider
    {
        /// <summary>
        /// Gets the highest available script version.
        /// </summary>
        int GetHighestScriptVersion();

        /// <summary>
        /// The source identifier for the script provider
        /// </summary>
        string SourceIdentifier { get; }

        Assembly SourceAssembly { get; }

        /// <summary>
        /// The display name for the script provider
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Gets the script for a given version number.
        /// </summary>
        /// <param name="versionNumber">The version number.</param>
        /// <returns></returns>
        IScript GetScript(int versionNumber);
    }
}