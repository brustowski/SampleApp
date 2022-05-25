namespace FilingPortal.PluginEngine.Common
{
    /// <summary>
    /// Used to initialize database structure on application start
    /// </summary>
    public interface IPluginDatabaseInit
    {
        /// <summary>
        /// Initializes database and applies migrations
        /// </summary>
        void Init();
    }
}
