using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Framework.Domain.Paging
{
    /// <summary>
    /// Represents Filter builder
    /// </summary>
    public class FilterBuilder
    {
        private readonly Filter _filter;

        /// <summary>
        /// Initialize a new instance of the <see cref="FilterBuilder"/> class
        /// </summary>
        /// <param name="fieldName">Filter field name</param>
        private FilterBuilder(string fieldName)
        {
            _filter = new Filter { FieldName = fieldName };
        }

        /// <summary>
        /// Creates a new instance of the <see cref="FilterBuilder"/> class for specified model and field
        /// </summary>
        /// <typeparam name="TModel">Type of the model</typeparam>
        /// <param name="property">Field expression</param>
        public static FilterBuilder CreateFor<TModel>(Expression<Func<TModel, object>> property)
        {
            return new FilterBuilder(PropertyExpressionHelper.GetPropertyName(property));
        }

        /// <summary>
        /// Sets the filter operand
        /// </summary>
        /// <param name="operand">Filter operand</param>
        public FilterBuilder Operand(string operand)
        {
            _filter.Operand = operand;
            return this;
        }

        /// <summary>
        /// Adds filter value
        /// </summary>
        /// <param name="value">Filter value</param>
        /// <param name="displayValue">Filter display value</param>
        public FilterBuilder AddValue(object value, string displayValue)
        {
            _filter.Values.Add(new LookupItem { Value = value, DisplayValue = displayValue });
            return this;
        }

        /// <summary>
        /// Adds filter value
        /// </summary>
        /// <param name="value">Filter value</param>
        public FilterBuilder AddValue(object value)
        {
            return AddValue(value, string.Empty);
        }

        /// <summary>
        /// Build the filter
        /// </summary>
        public Filter Build()
        {
            return _filter;
        }

        /// <summary>
        /// Adds a collection of values
        /// </summary>
        /// <typeparam name="TValue">The value type</typeparam>
        /// <param name="values">Collection of values</param>
        public FilterBuilder AddValues<TValue>(IEnumerable<TValue> values)
        {
            foreach (TValue value in values)
            {
                AddValue(value);
            }
            return this;
        }
    }
}