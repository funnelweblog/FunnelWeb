namespace FunnelWeb
{
    public interface IRequireDatabaseScripts
    {
        /// <summary>
        /// This identifier is used to figure out which of your scripts have already been run. Once you set this, 
        /// never, never, never change it. 
        /// </summary>
        string SourceIdentifier { get; }

        /// <summary>
        /// For example FunnelWeb.Extensions.MYEXTENSION.Scripts.Script{0}.sql
        /// Script version will have 4 digits.
        /// </summary>
        string ScriptNameFormat { get; }
    }
}
