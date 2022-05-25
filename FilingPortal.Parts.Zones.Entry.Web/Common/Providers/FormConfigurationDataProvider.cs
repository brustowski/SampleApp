using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using FilingPortal.Parts.Zones.Entry.Web.Configs;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Parts.Zones.Entry.Web.Common.Providers
{
    /// <summary>
    /// Provider for tables name
    /// </summary>
    public class FormConfigurationDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Default value repository
        /// </summary>
        private readonly IDefValuesReadModelRepository<DefValueReadModel> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormConfigurationDataProvider"/> class
        /// </summary>
        /// <param name="repository">Default value repository</param>
        public FormConfigurationDataProvider(IDefValuesReadModelRepository<DefValueReadModel> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.FormConfiguration;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            if (searchInfo.SearchByKey && int.TryParse(searchInfo.Search, out var id))
            {
                DefValueReadModel model = _repository.Get(id);
                if (model != null)
                {
                    return new[] { Convert(model) };
                }
            }

            IEnumerable<DefValueReadModel> data = _repository.GetAll();

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

        private static LookupItem Convert(DefValueReadModel source)
        {
            return new LookupItem { DisplayValue = source.ColumnName, Value = source.Id };
        }
    }
}