using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Domain.Paging;
using Framework.Domain.Specifications;
using Framework.Infrastructure;

namespace FilingPortal.Domain.Common
{
    public class SpecificationBuilder : ISpecificationBuilder
    {
       private readonly ISpecificationFactory _specificationFactory;

        public SpecificationBuilder(ISpecificationFactory specificationFactory)
        {
            _specificationFactory = specificationFactory;
        }

        public ISpecification Build<TModel>(FiltersSet filtersSet) where TModel: class 
        {
            ISpecification<TModel> resultSpecification = new DefaultSpecification<TModel>();

            var propertyNames = typeof(TModel).GetProperties().Select(x=>x.Name).ToList();

            foreach (var filter in filtersSet.Filters)
            {
                if (filter.Operand != FilterOperands.Custom)
                {
                    if (!IsPropertyExists(filter.FieldName, propertyNames)) continue;
                }

                if (IsFilterValueIsNull(filter)) continue;

                try
                {
                    var specification = _specificationFactory.Create<TModel>(filter);
                    resultSpecification = resultSpecification.And(specification);
                }
                catch (Exception ex)
                {
                    AppLogger.Error(ex, $"An error occured while creating specification from filter: {filter}");
                }
            }
            return resultSpecification;
        }

        private bool IsFilterValueIsNull(Filter filter)
        {
            return filter.Values is null || !filter.Values.Any() || filter.Values.Any((x=>x.Value is null));
        }

        private bool IsPropertyExists(string filterFieldName, IEnumerable<string> propertyNames)
        {
            return propertyNames.Contains(filterFieldName);
        }

    }
}