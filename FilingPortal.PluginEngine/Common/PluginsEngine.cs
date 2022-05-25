using System.Collections.Generic;

namespace FilingPortal.PluginEngine.Common
{
    /// <summary>
    /// Initialize plugins
    /// </summary>
    public class PluginsEngine
    {
        /// <summary>
        /// initialize a new instance of the <see cref="PluginsEngine"/> class.
        /// </summary>
        /// <param name="pluginDatabaseInits">Database structure initializer collection</param>
        public PluginsEngine(IEnumerable<IPluginDatabaseInit> pluginDatabaseInits)
        {
            foreach (var pluginDatabaseInit in pluginDatabaseInits)
            {
                pluginDatabaseInit.Init();
            }
        }
    }
}
