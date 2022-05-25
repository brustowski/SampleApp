using System;
using System.Linq.Expressions;

namespace FilingPortal.PluginEngine.GridConfigurations.Filters
{
    /// <summary>
    /// Interface for builder for <see cref="FilterConfig"/>
    /// </summary>
    public interface IFilterConfigOptions
    {
        /// <summary>
        /// Set the specified title
        /// </summary>
        /// <param name="title">The title</param>
        IFilterConfigOptions Title(string title);
        /// <summary>
        /// Defines the filter as non-searchable for values
        /// </summary>
        IFilterConfigOptions NotSearch();
        /// <summary>
        /// Sets the specified operand
        /// </summary>
        /// <param name="operand">The operand</param>
        IFilterConfigOptions SetOperand(string operand);
        /// <summary>
        /// Defines the source of filter data provider
        /// </summary>
        IFilterConfigOptions DataSourceFrom<T>() where T : IFilterDataProvider;
        /// <summary>
        /// Sets filter section to Advanced
        /// </summary>
        IFilterConfigOptions Advanced();
        /// <summary>
        /// Defines the dependency of filter on another filter by field name
        /// </summary>
        /// <param name="getter">The field name getter</param>
        IFilterConfigOptions DependsOn<TModel>(Expression<Func<TModel, object>> getter);
        /// <summary>
        /// Sets the maximum length
        /// </summary>
        /// <param name="maxLength">The maximum length</param>
        IFilterConfigOptions SetMaxLength(int maxLength);
        /// <summary>
        /// Marks filter as update filter
        /// </summary>
        IFilterConfigOptions IsUpdateFilter();
        /// <summary>
        /// Marks filter as multi-select
        /// </summary>
        IFilterConfigOptions IsMultiSelect();
    }
}