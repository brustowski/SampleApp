using System.Collections.Generic;
using FilingPortal.Parts.Recon.Domain.Enums;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.Parts.Recon.Web.DataSources
{
    /// <summary>
    /// Represents the PSA Reason Filter Data provider
    /// </summary>
    public class PsaReasonFilterDataProvider : IFilterDataProvider
    {
        /// <summary>
        /// Gets the collection of filter items
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            return new[]
            {
                new LookupItem() {DisplayValue = "Flagged and filed", Value = PsaReason.Flagged | PsaReason.Filed },
                new LookupItem() {DisplayValue = "Flagged and not filed", Value = PsaReason.Flagged | PsaReason.NotFiled },
                new LookupItem() {DisplayValue = "Not flagged", Value = PsaReason.NotFlagged },
            };
        }
    }
}