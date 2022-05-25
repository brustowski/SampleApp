using System.Collections.Generic;
using System.Linq;
using Framework.Infrastructure;

namespace FilingPortal.Domain.Common.Import
{
    /// <summary>
    /// Provides the template configuration registry
    /// </summary>
    public class ImportConfigurationRegistry : IImportConfigurationRegistry
    {
        /// <summary>
        /// Template configurations dictionary
        /// </summary>
        private readonly IDictionary<string, IImportConfiguration> _configurations;

        /// <summary>
        /// Creates a new instance of the <see cref="ImportConfigurationRegistry"/>
        /// </summary>
        public ImportConfigurationRegistry(IEnumerable<IImportConfiguration> configurations)
        {
            _configurations = configurations.ToDictionary(rc => rc.Name);
        }
        /// <summary>
        /// Gets the template configuration
        /// </summary>
        /// <param name="name">The name</param>
        public IImportConfiguration GetConfiguration(string name)
        {
            var key = name;
            if (!_configurations.ContainsKey(key))
            {
                AppLogger.Error($"A template configuration with name '{key}' was not found in the configuration registry.");
                throw new KeyNotFoundException($"A template configuration with name '{key}' was not found in the configuration registry.");
            }

            return _configurations[key];
        }
    }
}
