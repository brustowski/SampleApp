using FilingPortal.Domain.Services;
using FilingPortal.Parts.Inbond.Domain.Config;
using FilingPortal.Parts.Inbond.Domain.Entities;
using FilingPortal.Parts.Inbond.Web.Common.Providers;
using FilingPortal.Parts.Inbond.Web.Models;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.DataProviders.Cargowise;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Lookups.Providers;
using Framework.Domain.Paging;

namespace FilingPortal.Parts.Inbond.Web.GridConfigs
{
    /// <summary>
    /// Provides configuration for the FIRMs Code Rule grid
    /// </summary>
    public class RuleEntryGridConfig : RuleGridConfigurationWithUniqueFields<RuleEntryViewModel, RuleEntry>
    {
        /// <summary>
        /// Creates a new instance of <see cref="GridConfigurationWithUniqueFields{TModel, TEntity}"/>
        /// <param name="keyFieldsService">Key Fields Service</param>
        /// </summary>
        public RuleEntryGridConfig(IKeyFieldsService keyFieldsService) : base(keyFieldsService)
        {
        }

        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.RuleEntry;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureEditableColumns()
        {
            AddColumn(x => x.FirmsCode).IsKeyField().DisplayName("FIRMs Code").DefaultSorted()
                .EditableLookup().DataSourceFrom<CargowiseFirmsCodesDataProvider>().KeyField(x => x.FirmsCodeId);
            AddColumn(x => x.Importer).IsKeyField()
                .EditableLookup().DataSourceFrom<AllClientsDataProvider>().KeyField(x => x.ImporterId);
            AddColumn(x => x.Carrier).DisplayName("In-Bond Carrier")
                .EditableLookup().DataSourceFrom<InBondCarrierDataProvider>();
            AddColumn(x => x.Consignee).IsKeyField()
                .EditableLookup().DataSourceFrom<AllClientsDataProvider>().KeyField(x => x.ConsigneeId);
            AddColumn(x => x.UsPortOfDestination).DisplayName("US Port of Destination")
                .EditableLookup().DataSourceFrom<DomesticPortDataProvider>();
            AddColumn(x => x.EntryType).DisplayName("In-Bond Entry Type")
                .EditableLookup().DataSourceFrom<EntryTypeDataProvider>();
            AddColumn(x => x.Shipper).EditableLookup().DataSourceFrom<AllClientsDataProvider>().KeyField(x => x.ShipperId);
            AddColumn(x => x.Tariff).EditableLookup().DataSourceFrom<TariffDataProvider>();
            AddColumn(x => x.PortOfPresentation).DisplayName("Port Of Presentation")
                .EditableLookup().DataSourceFrom<DomesticPortDataProvider>();
            AddColumn(x => x.ForeignDestination).DisplayName("Foreign Destination").EditableText();
            AddColumn(x => x.TransportMode).DisplayName("Transport Mode")
                .EditableLookup().DataSourceFrom<TransportModeNumberDataProvider>();
            AddColumn(x => x.ImporterAddress).DisplayName("Importer Address")
                .EditableLookup().DataSourceFrom<ClientAddressDataProvider>()
                .DependsOn<RuleEntryViewModel>(x => x.Importer)
                .KeyField(x => x.ImporterAddressId);
            AddColumn(x => x.ConsigneeAddress).DisplayName("Consignee Address")
                .EditableLookup().DataSourceFrom<ClientAddressDataProvider>()
                .DependsOn<RuleEntryViewModel>(x => x.Consignee)
                .KeyField(x => x.ConsigneeAddressId);
            AddColumn(x => x.ConfirmationNeeded).DisplayName("Confirmation Needed").EditableBoolean();
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.FirmsCode).Title("FIRMs Code");
            TextFilterFor(x => x.Importer);
            TextFilterFor(x => x.Carrier).Title("In-Bond Carrier");
            TextFilterFor(x => x.Consignee);
            TextFilterFor(x => x.UsPortOfDestination).Title("US Port of Destination");
            TextFilterFor(x => x.EntryType).Title("In-Bond Entry Type"); // todo: [cbdev-2940] check filter type (start with or contains or equal)
            TextFilterFor(x => x.Consignee);
            TextFilterFor(x => x.Tariff);
            TextFilterFor(x => x.PortOfPresentation).Title("Port of Presentation");
            TextFilterFor(x => x.ForeignDestination).Title("Foreign Destination");
            TextFilterFor(x => x.TransportMode).Title("Transport Mode").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.ConsigneeAddress).Title("Consignee Address");
            DateRangeFilterFor(x => x.CreatedDate).Title("Created Date");
        }
    }
}