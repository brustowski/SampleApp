using FilingPortal.Parts.Common.Domain.AgileSettings;
using FilingPortal.Parts.Zones.Ftz214.Domain.Config;
using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;
using FilingPortal.PluginEngine.GridConfigurations;

namespace FilingPortal.Parts.Zones.Ftz214.Web.GridConfigs
{
    /// <summary>
    /// Defines the configuration for single-filing grid
    /// </summary>
    public class FilingGridConfig : AgileGridConfiguration<DefValue>
    {
        /// <summary>
        /// Creates configuration
        /// </summary>
        /// <param name="repository">The repository configuration based on"/></param>
        public FilingGridConfig(IAgileConfiguration<DefValue> repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets the GridName
        /// </summary>
        public override string GridName => GridNames.FilingGrid;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn("Applicant").DisplayName("Applicant").DefaultSorted();
            AddColumn("Ein").DisplayName("EIN");
            AddColumn("FtzOperator").DisplayName("FTZ Operator");
            AddColumn("ZoneId").DisplayName("Zone Id");
            AddColumn("AdmissionType").DisplayName("Admission Type");

            base.ConfigureColumns();
        }
    }
}
