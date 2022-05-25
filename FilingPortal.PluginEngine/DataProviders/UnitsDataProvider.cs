using System.Collections.Generic;
using FilingPortal.Domain.Repositories.Common;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;
using Framework.Infrastructure.Extensions;

namespace FilingPortal.PluginEngine.DataProviders
{
    /// <summary>
    /// Represents units of measure data provider
    /// </summary>
    public class UnitsDataProvider: HandbookDataProviderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitsDataProvider"/> class
        /// </summary>
        /// <param name="repository">Handbook table repository</param>
        public UnitsDataProvider(IHandbookRepository repository): base(repository)
        {
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public override string Name => DataProviderNames.Units;

        /// <summary>
        /// Gets handbook name
        /// </summary>
        public override string HandbookName => "units";

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public override IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            IList<LookupItem<string>> results = HandbookRepository.GetOptions(HandbookName, searchInfo.Search, searchInfo.Limit);
            results.ForEach(x =>
                {
                    if (x.DisplayValue == null)
                        x.DisplayValue = x.Value?.ToString();
                });
            return results;
        }
    }
}