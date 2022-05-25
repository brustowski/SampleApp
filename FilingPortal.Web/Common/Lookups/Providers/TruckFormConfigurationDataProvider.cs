using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Repositories.Truck;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provider for tables name
    /// </summary>
    public class TruckFormConfigurationDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Default value repository
        /// </summary>
        private readonly ITruckDefValuesReadModelRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckFormConfigurationDataProvider"/> class
        /// </summary>
        /// <param name="repository">Default value repository</param>
        public TruckFormConfigurationDataProvider(ITruckDefValuesReadModelRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.TruckFormConfiguration;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            if (searchInfo.SearchByKey && int.TryParse(searchInfo.Search, out var id))
            {
                TruckDefValueReadModel model = _repository.Get(id);
                if (model != null)
                {
                    return new[] { Convert(model) };
                }
            }

            IEnumerable<TruckDefValueReadModel> data = _repository.GetAll();

            if (!string.IsNullOrWhiteSpace(searchInfo.DependOn))
            {
                data = data.Where(x => x.TableName == searchInfo.DependValue);
            }

            if (!string.IsNullOrWhiteSpace(searchInfo.Search))
            {
                var searched = searchInfo.Search.ToLower();
                data = data.Where(x => x.ColumnName.ToLower().Contains(searched));
            }

            if (searchInfo.Limit > 0)
            {
                data = data.Take(searchInfo.Limit);
            }

            return data.Select(Convert);
        }

        private static LookupItem Convert(TruckDefValueReadModel source)
        {
            return new LookupItem { DisplayValue = source.ColumnName, Value = source.Id };
        }
    }
}