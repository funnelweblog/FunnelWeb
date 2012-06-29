namespace FunnelWeb.Settings
{
    /// <summary>
    /// This interface provides access to settings that cannot possibly be stored in the database. These usually include 
    /// the database connection string, and a backup username/password that can be used to upgrade the database.
    /// </summary>
    public interface IConfigSettings
    {
        /// <summary>
        /// Gets a setting by the specified name. Returns null if the setting is not set.
        /// </summary>
        /// <param name="name">The setting name.</param>
        /// <returns></returns>
        string Get(string name);

        /// <summary>
        /// Sets the specified setting.
        /// </summary>
        /// <param name="name">The setting name.</param>
        /// <param name="value">The value to set it to.</param>
        void Set(string name, string value);

        /// <summary>
        /// Used to check if the settings file/provider exists
        /// </summary>
        /// <returns></returns>
        bool ConfigFileMissing();

        /// <summary>
        /// Return null if can write to this settings source
        /// </summary>
        string ReadOnlyReason { get; }
    }
}