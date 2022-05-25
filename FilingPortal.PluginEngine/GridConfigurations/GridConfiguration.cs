using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FilingPortal.PluginEngine.GridConfigurations.Columns;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using Framework.Domain.Paging;
using Framework.Domain.Specifications;

namespace FilingPortal.PluginEngine.GridConfigurations
{
    /// <summary>
    /// Class for grid configuration
    /// </summary>
    public abstract class GridConfiguration<TModel> : IGridConfiguration where TModel:class
    {
        /// <summary>
        /// The collection of column configs
        /// </summary>
        private readonly List<ColumnConfig> _columnConfigs = new List<ColumnConfig>();

        /// <summary>
        /// The collection of filter configs
        /// </summary>
        private readonly List<FilterConfig> _filterConfigs = new List<FilterConfig>();

        /// <summary>
        /// Adds the column with the name according to the property getter expression
        /// </summary>
        /// <param name="getterExpression">The field name getter expression</param>
        protected virtual IColumnBuilder<TModel> AddColumn<TValue>(Expression<Func<TModel, TValue>> getterExpression)
        {
            var propertyName = PropertyExpressionHelper.GetPropertyName(getterExpression);
            return AddColumn(propertyName);
        }
        /// <summary>
        /// Adds the column with custom name
        /// </summary>
        /// <param name="propertyName">Column property name</param>
        protected virtual IColumnBuilder<TModel> AddColumn(string propertyName)
        {
            var columnConfig = new ColumnConfig(propertyName);
            _columnConfigs.Add(columnConfig);
            var columnBuilder = new ColumnBuilder<TModel>(columnConfig);
            columnBuilder.Resizable();
            return columnBuilder;
        }

        /// <summary>
        /// Adds columns range to configuration
        /// </summary>
        /// <param name="columns"></param>
        protected void AddColumnRange(IEnumerable<ColumnConfig> columns)
        {
            _columnConfigs.AddRange(columns);
        }

        /// <summary>
        /// Adds the text filter for the field name according to the property getter expression
        /// </summary>
        /// <param name="getterExpression">The field name getter expression</param>
        protected IFilterConfigOptions TextFilterFor<TValue>(Expression<Func<TModel, TValue>> getterExpression)
        {
            var propertyName = PropertyExpressionHelper.GetPropertyName(getterExpression);
            var filterConfig = new FilterConfig(propertyName);
            _filterConfigs.Add(filterConfig);
            var filterBuilder = new FilterConfigOptions(filterConfig).Type("text").Title(propertyName).SetOperand(FilterOperands.Contains).NotSearch();
            return filterBuilder;
        }

        /// <summary>
        /// Adds the number filter for the field name according to the property getter expression
        /// </summary>
        /// <param name="getterExpression">The field name getter expression</param>
        protected IFilterConfigOptions NumberFilterFor<TValue>(Expression<Func<TModel, TValue>> getterExpression)
        {
            var propertyName = PropertyExpressionHelper.GetPropertyName(getterExpression);
            var filterConfig = new FilterConfig(propertyName);
            _filterConfigs.Add(filterConfig);
            var filterBuilder = new FilterConfigOptions(filterConfig).Type("number").Title(propertyName);
            return filterBuilder;
        }

        /// <summary>
        /// Adds the floating point number filter for the field name according to the property getter expression
        /// </summary>
        /// <param name="getterExpression">The field name getter expression</param>
        protected IFilterConfigOptions FloatNumberFilterFor<TValue>(Expression<Func<TModel, TValue>> getterExpression)
        {
            var propertyName = PropertyExpressionHelper.GetPropertyName(getterExpression);
            var filterConfig = new FilterConfig(propertyName);
            _filterConfigs.Add(filterConfig);
            var filterBuilder = new FilterConfigOptions(filterConfig).Type("floatNumber").Title(propertyName);
            return filterBuilder;
        }

        /// <summary>
        /// Adds the select filter for the field name according to the property getter expression
        /// </summary>
        /// <param name="getterExpression">The field name getter expression</param>
        protected IFilterConfigOptions SelectFilterFor<TValue>(Expression<Func<TModel, TValue>> getterExpression)
        {
            var propertyName = PropertyExpressionHelper.GetPropertyName(getterExpression);
            var filterConfig = new FilterConfig(propertyName);
            _filterConfigs.Add(filterConfig);
            var filterBuilder = new FilterConfigOptions(filterConfig).Type("select").Title(propertyName);
            return filterBuilder;
        }

        /// <summary>
        /// Adds the date filter for the field name according to the property getter expression
        /// </summary>
        /// <param name="getterExpression">The field name getter expression</param>
        protected IFilterConfigOptions DateFilterFor<TValue>(Expression<Func<TModel, TValue>> getterExpression)
        {
            var propertyName = PropertyExpressionHelper.GetPropertyName(getterExpression);
            var filterConfig = new FilterConfig(propertyName);
            _filterConfigs.Add(filterConfig);
            var filterBuilder = new FilterConfigOptions(filterConfig).Type("date").Title(propertyName);
            return filterBuilder;
        }

        /// <summary>
        /// Adds the date range filter for the field name according to the property getter expression
        /// </summary>
        /// <param name="getterExpression">The field name getter expression</param>
        protected IFilterConfigOptions DateRangeFilterFor<TValue>(Expression<Func<TModel, TValue>> getterExpression)
        {
            var propertyName = PropertyExpressionHelper.GetPropertyName(getterExpression);
            var filterConfig = new FilterConfig(propertyName);
            _filterConfigs.Add(filterConfig);
            var filterBuilder = new FilterConfigOptions(filterConfig).Type("date-range").Title(propertyName).SetOperand(FilterOperands.DateBetween);
            return filterBuilder;
        }

        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public abstract string GridName { get;}

        /// <summary>
        /// Gets the name of the corresponding template file name
        /// </summary>
        public virtual string TemplateFileName => string.Empty;

        /// <summary>
        /// Configures the grid
        /// </summary>
        public void Configure()
        {
            Reset();
            ConfigureColumns();
            ConfigureFilters();
        }

        /// <summary>
        /// Reset grid configuration
        /// </summary>
        protected virtual void Reset()
        {
            _columnConfigs.Clear();
            _filterConfigs.Clear();
        }

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected abstract void ConfigureColumns();

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected abstract void ConfigureFilters();

        /// <summary>
        /// Gets the column configurations
        /// </summary>
        public virtual IEnumerable<ColumnConfig> GetColumns()
        {
            return _columnConfigs;
        }

        /// <summary>
        /// Gets the filters configurations
        /// </summary>
        public IEnumerable<FilterConfig> GetFilters()
        {
            return _filterConfigs;
        }

        /// <summary>
        /// Gets the filter configuration by field name
        /// </summary>
        /// <param name="fieldName">Name of the field</param>
        public FilterConfig GetFilterConfig(string fieldName)
        {
            return _filterConfigs.First(x => x.FieldName == fieldName);
        }

        /// <summary>
        /// Gets ths column configuration by field name
        /// </summary>
        /// <param name="fieldName">Name of the field</param>
        public ColumnConfig GetColumnConfig(string fieldName)
        {
            return FindConfiguration(fieldName, _columnConfigs) ?? throw new ArgumentOutOfRangeException(nameof(fieldName), $"Column with name '{fieldName}' not found.");

        }

        private static ColumnConfig FindConfiguration(string fieldName, IEnumerable<ColumnConfig> configs)
        {
            foreach (ColumnConfig columnConfig in configs)
            {
                if (columnConfig.FieldName == fieldName)
                {
                    return columnConfig;
                }

                if (columnConfig.Columns == null) continue;

                ColumnConfig config = FindConfiguration(fieldName, columnConfig.Columns);
                if (config != null)
                {
                    return config;
                }
            }

            return null;
        }
    }
}