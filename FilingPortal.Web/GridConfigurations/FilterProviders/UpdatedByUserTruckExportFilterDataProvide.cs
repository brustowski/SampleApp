using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Repositories.TruckExport;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;
using Framework.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Web.GridConfigurations.FilterProviders
{
    /// <summary>
    /// Provider for Truck Export Updated by user filter
    /// </summary>
    public class UpdatedByUserTruckExportFilterDataProvide : IFilterDataProvider
    {
        /// <summary>
        /// The search request factory
        /// </summary>
        private readonly ISearchRequestFactory _searchRequestFactory;

        /// <summary>
        /// The Truck Export record repository
        /// </summary>
        private readonly ITruckExportReadModelRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdatedByUserTruckExportFilterDataProvide"/> class
        /// </summary>
        /// <param name="searchRequestFactory">The search request factory</param>
        /// <param name="repository">The data repository</param>
        public UpdatedByUserTruckExportFilterDataProvide(ISearchRequestFactory searchRequestFactory, ITruckExportReadModelRepository repository)
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
            const string propertyName = nameof(TruckExportReadModel.ModifiedUser);

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

            SearchRequest searchRequest = _searchRequestFactory.Create<TruckExportReadModel>(searchModel);

            var users = _repository.GetUsers(searchRequest).ToArray();
            var result = new List<LookupItem>(users.Count() + 1) { new LookupItem { Value = null, DisplayValue = "All" } };
            result.AddRange(users.Select(x => new LookupItem { DisplayValue = x, Value = x }));
            return result;
        }
    }
}