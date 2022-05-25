using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Rail
{
    /// <summary>
    /// Provides filterd rail records available actions
    /// </summary>
    public class FilteredRailRecordsActionsConfig : PageConfiguration<InboundRecordValidationResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilteredRailRecordsActionsConfig"/> class.
        /// </summary>
        public FilteredRailRecordsActionsConfig()
        {
            PageName = PageConfigNames.FilteredRailRecordsActionsConfig;
        }

        /// <summary>
        /// Configures available actions
        /// </summary>
        public override void Configure()
        {
            AddAction("SelectAll").AvailabilityRulesFrom<FilteredRailRecordsSelectAllRule>();
        }
    }
}