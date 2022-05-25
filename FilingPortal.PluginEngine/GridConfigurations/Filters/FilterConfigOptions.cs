using Framework.Domain.Specifications;
using System;
using System.Linq.Expressions;

namespace FilingPortal.PluginEngine.GridConfigurations.Filters
{
    /// <summary>
    /// builder for <see cref="FilterConfig"/>
    /// </summary>
    public class FilterConfigOptions : IFilterConfigOptions
    {
        /// <summary>
        /// The filter configuration
        /// </summary>
        private readonly FilterConfig _filterConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterConfigOptions"/> class
        /// </summary>
        /// <param name="filterConfig">The filter configuration</param>
        public FilterConfigOptions(FilterConfig filterConfig)
        {
            _filterConfig = filterConfig;
        }

        /// <summary>
        /// Set the specified title
        /// </summary>
        /// <param name="title">The title</param>
        public IFilterConfigOptions Title(string title)
        {
            _filterConfig.Title = title;
            return this;
        }

        /// <summary>
        /// Types the specified type
        /// </summary>
        /// <param name="type">The type</param>
        public IFilterConfigOptions Type(string type)
        {
            _filterConfig.Type = type;
            return this;
        }

        /// <summary>
        /// Defines the filter as non-searchable for values
        /// </summary>
        public IFilterConfigOptions NotSearch()
        {
            _filterConfig.IsSearch = false;
            return this;
        }

        /// <summary>
        /// Sets the specified operand
        /// </summary>
        /// <param name="operand">The operand</param>
        public IFilterConfigOptions SetOperand(string operand)
        {
            _filterConfig.Operand = operand;
            return this;
        }

        /// <summary>
        /// Defines the source of filter data provider
        /// </summary>
        public IFilterConfigOptions DataSourceFrom<T>() where T: IFilterDataProvider
        {
            var name = typeof (T);

            _filterConfig.DataSourceType = name;

            return this;
        }

        /// <summary>
        /// Defines the dependency of filter on another filter by field name
        /// </summary>
        /// <param name="getter">The field name getter</param>
        public IFilterConfigOptions DependsOn<TModel>(Expression<Func<TModel, object>> getter)
        {
            var propertyName = PropertyExpressionHelper.GetPropertyName(getter);

            _filterConfig.DependOn = propertyName;

            return this;
        }

        /// <summary>
        /// Sets the maximum length
        /// </summary>
        /// <param name="maxLength">The maximum length</param>
        public IFilterConfigOptions SetMaxLength(int maxLength)
        {
            _filterConfig.MaxLength = maxLength;
            return this;
        }

        /// <summary>
        /// Marks filter as update filter
        /// </summary>
        public IFilterConfigOptions IsUpdateFilter()
        {
            _filterConfig.IsUpdateFilter = true;
            return this;
        }

        /// <summary>
        /// Marks filter as multi-select
        /// </summary>
        public IFilterConfigOptions IsMultiSelect()
        {
            _filterConfig.IsMultiSelect = true;
            return this;
        }

        /// <summary>
        /// Sets Advanced property
        /// </summary>
        public IFilterConfigOptions Advanced()
        {
            _filterConfig.Advanced = true;
            return this;
        }
    }
}