using System;
using System.Collections.Generic;
using FilingPortal.PluginEngine.Common.Grids;
using FilingPortal.PluginEngine.GridConfigurations;

namespace FilingPortal.Web.Common.Grids
{
    /// <summary>
    /// Class for grid configuration registry
    /// </summary>
    public class GridConfigRegistry : IGridConfigRegistry
    {
        /// <summary>
        /// The collection of grid configurations
        /// </summary>
        private readonly Dictionary<string, IGridConfiguration> _gridConfigurations = new Dictionary<string, IGridConfiguration>();

        /// <summary>
        /// Initializes a new instance of the <see cref="GridConfigRegistry"/> class
        /// </summary>
        /// <param name="configurations">The grid configurations</param>
        public GridConfigRegistry(IEnumerable<IGridConfiguration> configurations)
        {
            foreach (var gridConfiguration in configurations)
            {
                gridConfiguration.Configure();
                _gridConfigurations.Add(gridConfiguration.GridName, gridConfiguration);
            }
        }

        /// <summary>
        /// Gets the grid configuration by specified name grid
        /// </summary>
        /// <param name="gridName">Name of the grid</param>
        /// <exception cref="InvalidOperationException"></exception>
        public IGridConfiguration GetGridConfig(string gridName)
        {
            if (_gridConfigurations.ContainsKey(gridName))
            {
                return _gridConfigurations[gridName];
            }
            throw new InvalidOperationException("The Grid config was not found for " + gridName);
        }
    }
}