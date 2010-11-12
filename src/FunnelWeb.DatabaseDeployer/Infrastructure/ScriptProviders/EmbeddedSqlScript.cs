
namespace FunnelWeb.DatabaseDeployer.Infrastructure.ScriptProviders
{
    /// <summary>
    /// Represents a SQL Server script that comes from an embedded resource in an assembly. 
    /// </summary>
    public sealed class EmbeddedSqlScript : IScript
    {
        private readonly string contents;
        private readonly string name;
        private readonly int versionNumber;
        private readonly string sourceIdentifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedSqlScript"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="contents">The contents.</param>
        /// <param name="versionNumber">The version number.</param>
        /// <param name="sourceIdentifier">The source identifier.</param>
        public EmbeddedSqlScript(string name, string contents, int versionNumber, string sourceIdentifier)
        {
            this.name = name;
            this.contents = contents;
            this.versionNumber = versionNumber;
            this.sourceIdentifier = sourceIdentifier;
        }

        /// <summary>
        /// Gets the contents of the script.
        /// </summary>
        /// <value></value>
        public string Contents
        {
            get { return contents; }
        }

        /// <summary>
        /// Gets the name of the script.
        /// </summary>
        /// <value></value>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the numeric version number for this script.
        /// </summary>
        /// <value></value>
        public int VersionNumber
        {
            get { return versionNumber; }
        }

        /// <summary>
        /// An identifier that uniquely identifies the source of the script. This may be the full name of
        /// the assembly the script was embedded in, the path to the script, or some other source.
        /// </summary>
        /// <value></value>
        public string SourceIdentifier
        {
            get { return sourceIdentifier; }
        }
    }
}