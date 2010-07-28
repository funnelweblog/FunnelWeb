
namespace FunnelWeb.DatabaseDeployer.Infrastructure.ScriptProviders
{
    /// <summary>
    /// Represents a SQL Server script that comes from an embedded resource in an assembly. 
    /// </summary>
    public sealed class EmbeddedSqlScript : IScript
    {
        private readonly string _contents;
        private readonly string _name;
        private readonly int _versionNumber;
        private readonly string _sourceIdentifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedSqlScript"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="contents">The contents.</param>
        /// <param name="versionNumber">The version number.</param>
        /// <param name="sourceIdentifier">The source identifier.</param>
        public EmbeddedSqlScript(string name, string contents, int versionNumber, string sourceIdentifier)
        {
            _name = name;
            _contents = contents;
            _versionNumber = versionNumber;
            _sourceIdentifier = sourceIdentifier;
        }

        /// <summary>
        /// Gets the contents of the script.
        /// </summary>
        /// <value></value>
        public string Contents
        {
            get { return _contents; }
        }

        /// <summary>
        /// Gets the name of the script.
        /// </summary>
        /// <value></value>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the numeric version number for this script.
        /// </summary>
        /// <value></value>
        public int VersionNumber
        {
            get { return _versionNumber; }
        }

        /// <summary>
        /// An identifier that uniquely identifies the source of the script. This may be the full name of
        /// the assembly the script was embedded in, the path to the script, or some other source.
        /// </summary>
        /// <value></value>
        public string SourceIdentifier
        {
            get { return _sourceIdentifier; }
        }
    }
}