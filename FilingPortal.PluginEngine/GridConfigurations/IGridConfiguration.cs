using System.Collections.Generic;
using FilingPortal.PluginEngine.GridConfigurations.Columns;
using FilingPortal.PluginEngine.GridConfigurations.Filters;

namespace FilingPortal.PluginEngine.GridConfigurations
{
    /// <summary>
    /// Interface for grid configuration
    /// </summary>
    public interface IGridConfiguration
    {
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        string GridName { get; }
        
        /// <summary>
        /// Gets the name of the corresponding template file name
        /// </summary>
        string TemplateFileName { get; }

        /// <summary>
        /// Configures the grid
        /// </summary>
        void Configure();
        
        /// <summary>
        /// Gets the column configurations
        /// </summary>
        IEnumerable<ColumnConfig> GetColumns();

        /// <summary>
        /// Gets the filters configurations
        /// </summary>
        IEnumerable<FilterConfig> GetFilters();

        /// <summary>
        /// Gets the filter configuration by field name
        /// </summary>
        /// <param name="fieldName">Name of the field</param>
        FilterConfig GetFilterConfig(string fieldName);

        /// <summary>
        /// Gets the column configuration by field name
        /// </summary>
        /// <param name="fieldName">Name of the field</param>
        ColumnConfig GetColumnConfig(string fieldName);
    }
}