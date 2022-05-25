using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Models.Vessel;

namespace FilingPortal.Web.GridConfigurations.Vessel
{

    /// <summary>
    /// Provides the configuration for the Vessel Importer Rule grid
    /// </summary>
    public class VesselRuleImporterGridConfiguration : RuleGridConfigurationWithUniqueFields<VesselRuleImporterViewModel, VesselRuleImporter>
    {
        /// <summary>
        /// Creates a new instance of <see cref="GridConfigurationWithUniqueFields{TModel, TEntity}"/>
        /// <param name="keyFieldsService">Key Fields Service</param>
        /// </summary>
        public VesselRuleImporterGridConfiguration(IKeyFieldsService keyFieldsService) : base(keyFieldsService)
        {
        }

        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.VesselRuleImporter;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureEditableColumns()
        {
            AddColumn(x => x.Importer).DisplayName("IOR").MinWidth(200).DefaultSorted().EditableText();
            AddColumn(x => x.CWImporter).DisplayName("CW IOR").MinWidth(150).EditableText();
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.Importer).Title("IOR");
            TextFilterFor(x => x.CWImporter).Title("CW IOR");
        }
    }
}
