namespace FunnelWeb.DatabaseDeployer.Infrastructure.ScriptProviders
{
    /// <summary>
    /// An interface implemented by objects that represent database upgrade scripts.
    /// </summary>
    public interface IScript
    {
        /// <summary>
        /// Gets the contents of the script.
        /// </summary>
        string Contents { get; }

        /// <summary>
        /// Gets the name of the script.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the numeric version number for this script.
        /// </summary>
        int VersionNumber { get; }

        /// <summary>
        /// An identifier that uniquely identifies the source of the script. This may be the full name of 
        /// the assembly the script was embedded in, the path to the script, or some other source.
        /// </summary>
        string SourceIdentifier { get; }
    }
}