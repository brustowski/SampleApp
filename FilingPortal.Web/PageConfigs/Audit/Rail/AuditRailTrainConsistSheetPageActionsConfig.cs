using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Audit.Rail
{
    /// <summary>
    /// Class for Pipeline Rule Record Configuration
    /// </summary>
    public class AuditRailTrainConsistSheetPageActionsConfig : PageConfiguration<PageConfigurationModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditRailTrainConsistSheetPageActionsConfig"/> class.
        /// </summary>
        public AuditRailTrainConsistSheetPageActionsConfig()
        {
            PageName = PageConfigNames.AuditRailTrainConsistSheetPageActions;
        }

        /// <summary>
        /// Configures actions for Rail Parsed View Item
        /// </summary>
        public override void Configure()
        {
            AddAction("Import").AvailabilityRulesFrom<AuditRailTrainConsistSheetPageImport>();
            AddAction("Template").AvailabilityRulesFrom<AuditRailTrainConsistSheetPageImport>();
        }
    }
}