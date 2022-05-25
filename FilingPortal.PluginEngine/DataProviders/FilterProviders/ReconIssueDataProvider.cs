using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;
using System.Collections.Generic;

namespace FilingPortal.PluginEngine.DataProviders.FilterProviders
{
    /// <summary>
    /// Provider for Recon Issue data
    /// </summary>
    public class ReconIssueDataProvider : IFilterDataProvider, ILookupDataProvider
    {
        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            return new[]
            {
                new LookupItem { DisplayValue = "NA - Not Applicable", Value = "NA"},
                new LookupItem { DisplayValue = "98 - 9802 Recon", Value = "98" },
                new LookupItem { DisplayValue = "C9 - Class/9802 Recon", Value = "C9" },
                new LookupItem { DisplayValue = "CL - Class Recon", Value = "CL" },
                new LookupItem { DisplayValue = "NF - Trade Agreement", Value = "NF" },
                new LookupItem { DisplayValue = "V9 - Value/9802 Recon", Value = "V9" },
                new LookupItem { DisplayValue = "AL - Value/Class/9802 Recon", Value = "AL" },
                new LookupItem { DisplayValue = "VC - Value/Class Recon", Value = "VC" },
                new LookupItem { DisplayValue = "VL - Value Recon", Value = "VL" },
            };
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.ReconIssue;
    }
}