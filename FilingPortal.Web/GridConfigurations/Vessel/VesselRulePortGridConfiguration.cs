using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.DataProviders.Cargowise;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Lookups.Providers;
using FilingPortal.Web.Models.Vessel;

namespace FilingPortal.Web.GridConfigurations.Vessel
{
    /// <summary>
    /// Provides the configuration for the Vessel Port Rule grid
    /// </summary>
    public class VesselRulePortGridConfiguration : RuleGridConfigurationWithUniqueFields<VesselRulePortViewModel, VesselRulePort>
    {
        /// <summary>
        /// Creates a new instance of <see cref="GridConfigurationWithUniqueFields{TModel, TEntity}"/>
        /// <param name="keyFieldsService">Key Fields Service</param>
        /// </summary>
        public VesselRulePortGridConfiguration(IKeyFieldsService keyFieldsService) : base(keyFieldsService)
        {
        }

        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.VesselRulePort;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureEditableColumns()
        {
            AddColumn(x => x.FirmsCode).DisplayName("FIRMs Code").MinWidth(150).EditableLookup()
                .DataSourceFrom<CargowiseFirmsCodesDataProvider>().KeyField(x => x.FirmsCodeId).DefaultSorted();
            AddColumn(x => x.EntryPort).DisplayName("Entry Port").MinWidth(150).EditableText();
            AddColumn(x => x.DischargePort).DisplayName("Discharge Port").MinWidth(150).EditableText();
            AddColumn(x => x.HMF).DisplayName("HMF").MinWidth(150).EditableLookup().DataSourceFrom<YesNoLookUpDataProvider>();
            AddColumn(x => x.DestinationCode).DisplayName("Destination Code").EditableText();
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.FirmsCode).Title("FIRMs Code");
            TextFilterFor(x => x.EntryPort).Title("Entry Port");
            TextFilterFor(x => x.DischargePort).Title("Discharge Port");
            TextFilterFor(x => x.HMF).Title("HMF");
            TextFilterFor(x => x.DestinationCode).Title("Destination Code");
        }
    }
}
