using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Parts.Isf.Domain.Entities;
using FilingPortal.Parts.Isf.Domain.Repositories;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Isf.Web.Common.Providers
{
    /// <summary>
    /// Created users data provider
    /// </summary>
    public class CreatedUsersDataProvider : IFilterDataProvider
    {
        /// <summary>
        /// The search request factory
        /// </summary>
        private readonly ISearchRequestFactory _searchRequestFactory;

        /// <summary>
        /// The Truck Export record repository
        /// </summary>
        private readonly IInboundReadModelRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreatedUsersDataProvider"/> class
        /// </summary>
        /// <param name="searchRequestFactory">The search request factory</param>
        /// <param name="repository">The data repository</param>
        public CreatedUsersDataProvider(ISearchRequestFactory searchRequestFactory, IInboundReadModelRepository repository)
        {
            _searchRequestFactory = searchRequestFactory;
            _repository = repository;
        }

        /// <summary>
        /// Gets the collection of the filter data
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            const string propertyName = nameof(InboundReadModel.CreatedUser);

            var searchModel = new SearchRequestModel
            {
                SortingSettings = { Field = propertyName, SortOrder = SortOrder.Asc }
            };

            if (!string.IsNullOrWhiteSpace(searchInfo.Search))
            {
                var filter = new Filter
                {
                    FieldName = propertyName,
                    Operand = FilterOperands.Contains
                };
                filter.Values.Add(new LookupItem { Value = searchInfo.Search, });
                searchModel.FilterSettings.Filters.Add(filter);
            }

            SearchRequest searchRequest = _searchRequestFactory.Create<InboundReadModel>(searchModel);

            var users = _repository.GetUsers(searchRequest);
            var result = new List<LookupItem>(users.Count + 1) { new LookupItem { Value = null, DisplayValue = "All" } };
            result.AddRange(users.Select(x => new LookupItem { DisplayValue = x, Value = x }));
            return result;
        }
    }
}