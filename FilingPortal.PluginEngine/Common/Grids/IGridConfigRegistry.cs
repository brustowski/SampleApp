using FilingPortal.PluginEngine.GridConfigurations;

namespace FilingPortal.PluginEngine.Common.Grids
{
    /// <summary>
    /// Interface for grid configuration registry
    /// </summary>
    public interface IGridConfigRegistry
    {
        /// <summary>
        /// Gets the grid configuration by specified grid name
        /// </summary>
        /// <param name="gridName">Name of the grid</param>
        IGridConfiguration GetGridConfig(string gridName);
    }
}