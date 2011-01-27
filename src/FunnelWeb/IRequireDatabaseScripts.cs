namespace FunnelWeb
{
    public interface IRequireDatabaseScripts
    {
        /// <summary>
        /// For example FunnelWeb.Extensions.MYEXTENSION.Scripts.Script{0}.sql
        /// Script version will have 4 digits.
        /// </summary>
        string ScriptNameFormat { get; }
    }
}
