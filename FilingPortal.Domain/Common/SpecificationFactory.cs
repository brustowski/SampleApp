using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Domain.Paging;
using Framework.Domain.Specifications;
using Framework.Infrastructure;

namespace FilingPortal.Domain.Common
{
    /// <summary>
    /// This class builds SQL request based on known specification types
    /// </summary>
    internal class SpecificationFactory : ISpecificationFactory
    {
        /// <summary>
        /// Specifications registry
        /// </summary>
        private readonly ICustomSpecificationsRegistry _customSpecificationsRegistry;

        /// <summary>
        /// Mappings between operands and specifications
        /// </summary>
        private readonly Dictionary<string, Type> _specificationTypesMap = new Dictionary<string, Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecificationFactory"/> class.
        /// </summary>
        /// <param name="customSpecificationsRegistry">Specifications registry</param>
        public SpecificationFactory(ICustomSpecificationsRegistry customSpecificationsRegistry)
        {
            _customSpecificationsRegistry = customSpecificationsRegistry;

            _specificationTypesMap.Add(FilterOperands.Equal, typeof(EqualsSpecification<>));
            _specificationTypesMap.Add(FilterOperands.NotEqual, typeof(NotEqualsSpecification<>));
            _specificationTypesMap.Add(FilterOperands.Contains, typeof(ContainsSpecification<>));
            _specificationTypesMap.Add(FilterOperands.DateFrom, typeof(DateFromSpecification<>));
            _specificationTypesMap.Add(FilterOperands.DateTo, typeof(DateToSpecification<>));
            _specificationTypesMap.Add(FilterOperands.DateBetween, typeof(InRangeSpecification<>));
        }

        /// <summary>
        /// Validates request and returns specification by filter
        /// </summary>
        /// <typeparam name="T">Entity under filtering</typeparam>
        /// <param name="filter">Filter</param>
        public ISpecification<T> Create<T>(Filter filter) where T : class
        {
            Check.NotNull(filter, nameof(filter));
            if (filter.Operand == FilterOperands.Custom)
            {
                return _customSpecificationsRegistry.GetSpecification<T>(filter);
            }
            if (_specificationTypesMap.ContainsKey(filter.Operand))
            {
                Type type = _specificationTypesMap[filter.Operand];

                return CreateSpecificationForFilter<T>(filter, type);
            }

            throw new KeyNotFoundException($"The specification not registered for {filter.Operand} operand");
        }

        /// <summary>
        /// Creates specification by filter
        /// </summary>
        /// <typeparam name="T">Entity under filtering</typeparam>
        /// <param name="filter">The filter</param>
        /// <param name="type">Specification type</param>
        private static ISpecification<T> CreateSpecificationForFilter<T>(Filter filter, Type type) where T : class
        {
            if (filter.Values.Count == 1)
                return MakeSpecification<T>(type, filter.FieldName, filter.Values[0].Value);

            return new InArraySpecification<T>(filter.Values.Select(x => x.Value), filter.FieldName);
        }

        /// <summary>
        /// Creates specification for each value in filter
        /// </summary>
        /// <typeparam name="TModel">Entity under filtering</typeparam>
        /// <param name="type">Specification type</param>
        /// <param name="fieldName">Field name</param>
        /// <param name="value">Filter value</param>
        private static ISpecification<TModel> MakeSpecification<TModel>(Type type, string fieldName, object value)
            where TModel : class
        {
            Type genericType = type.MakeGenericType(typeof(TModel));
            Type fieldType = typeof(TModel).GetProperty(fieldName).PropertyType;
            var args = new List<object>();
            if (value is IEnumerable<object> arr)
            {
                foreach (object obj in arr)
                {
                    object convertedValue = Convert.ChangeType(obj, fieldType);
                    args.Add(convertedValue);
                }
            }
            else
            {
                if (value is string s) value = s.Trim();
                object convertedValue = AutoMapper.Mapper.Map(value, value.GetType(), fieldType);
                args.Add(convertedValue);
            }
            args.Add(fieldName);
            object instance = Activator.CreateInstance(genericType, args.ToArray());
            return (ISpecification<TModel>)instance;
        }
    }
}
