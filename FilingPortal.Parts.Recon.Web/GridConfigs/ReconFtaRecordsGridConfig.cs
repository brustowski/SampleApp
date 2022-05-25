using FilingPortal.Parts.Recon.Domain.Config;
using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Web.DataSources;
using FilingPortal.Parts.Recon.Web.Models;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.DataProviders.FilterProviders;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Lookups.Providers;
using FilingPortal.Web.GridConfigurations.FilterProviders;
using Framework.Domain.Paging;

namespace FilingPortal.Parts.Recon.Web.GridConfigs
{
    /// <summary>
    /// Class describing the configuration for the recon report records grid
    /// </summary>
    public class ReconFtaRecordsGridConfig : GridConfiguration<FtaReconViewModel>
    {
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.FtaRecords;

        /// <summary>
        /// Gets the name of the corresponding template file name
        /// </summary>
        public override string TemplateFileName => "recon_template.xlsx";

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn(x => x.Importer).DisplayName("Importer").MinWidth(110);
            AddColumn(x => x.ImporterNo).DisplayName("Importer No").MinWidth(110);
            AddColumn(x => x.Filer).MinWidth(110);
            AddColumn(x => x.EntryNo).DisplayName("Entry No").MinWidth(110);
            AddColumn(x => x.LineNumber7501).DisplayName("7501 Line Number").MinWidth(110);
            AddColumn(x => x.JobNumber).DisplayName("Job Number").MinWidth(120).DefaultSorted();
            AddColumn(x => x.ReconIssue).DisplayName("Recon Issue").MinWidth(110);
            AddColumn(x => x.NaftaRecon).DisplayName("FTA Recon").MinWidth(110);
            AddColumn(x => x.CalculatedClientReconDueDate).DisplayName("Calculated Client Recon Due Date").MinWidth(110);
            AddColumn(x => x.Calculated520DDueDate).DisplayName("Calculated 520D Due Date").MinWidth(110);
            AddColumn(x => x.ExportDate).DisplayName("Export Date").MinWidth(110);
            AddColumn(x => x.ImportDate).DisplayName("Import Date").MinWidth(110);
            AddColumn(x => x.ReleaseDate).DisplayName("Release Date").MinWidth(110);
            AddColumn(x => x.EntryPort).DisplayName("Entry Port").MinWidth(110);
            AddColumn(x => x.DestinationState).DisplayName("Destination State").MinWidth(110);
            AddColumn(x => x.EntryType).DisplayName("Entry Type").MinWidth(110);
            AddColumn(x => x.TransportMode).DisplayName("Transport Mode").MinWidth(110);
            AddColumn(x => x.Vessel).DisplayName("Vessel").MinWidth(110);
            AddColumn(x => x.Voyage).DisplayName("Voyage Flight").MinWidth(110);
            AddColumn(x => x.OwnerRef).DisplayName("Owner Reference").MinWidth(110);
            AddColumn(x => x.Spi).DisplayName("SPI").MinWidth(110);
            AddColumn(x => x.CO).DisplayName("C/O").MinWidth(110);
            AddColumn(x => x.ManufacturerMid).DisplayName("Manufacturer MID").MinWidth(110);
            AddColumn(x => x.Tariff).DisplayName("Tariff").MinWidth(110);
            AddColumn(x => x.GoodsDescription).DisplayName("Goods Description").MinWidth(110);
            AddColumn(x => x.Container).DisplayName("Containers").MinWidth(110);
            AddColumn(x => x.CustomsAttribute1).DisplayName("Custom Attrib 1").MinWidth(110);
            AddColumn(x => x.CustomsQty1).DisplayName("Customs Qty 1").MinWidth(110);
            AddColumn(x => x.CustomsUq1).DisplayName("Customs UQ 1").MinWidth(110);
            AddColumn(x => x.MasterBill).DisplayName("Master Bill").MinWidth(110);
            AddColumn(x => x.LineEnteredValue).DisplayName("Line Entered Value").MinWidth(110);
            AddColumn(x => x.InvoiceLineCharges).DisplayName("Invoice Line Charges Amount").MinWidth(110);
            AddColumn(x => x.Duty).DisplayName("Duty").MinWidth(110);
            AddColumn(x => x.Hmf).DisplayName("HMF").MinWidth(110);
            AddColumn(x => x.Mpf).DisplayName("MPF").MinWidth(110);
            AddColumn(x => x.PayableMpf).DisplayName("Payable MPF").MinWidth(110);
            AddColumn(x => x.PriorDisclosureMisc).DisplayName("Prior Disclosure MISC").MinWidth(110);
            AddColumn(x => x.ProtestPetitionFiledStatMisc).DisplayName("Protest Petition Filed Stat MISC").MinWidth(110);
            AddColumn(x => x.Nafta303ClaimStatMisc).DisplayName("NAFTA 303 Claim Stat Misc").MinWidth(110);
            AddColumn(x => x.FtaEligibility).DisplayName("FTA Eligibility").MinWidth(110);
            AddColumn(x => x.ClientNote).DisplayName("Client Note").MinWidth(110);
            AddColumn(x => x.CreatedDate).DisplayName("Upload Date").MinWidth(110);
            AddColumn(x => x.CreatedUser).DisplayName("Upload User").MinWidth(110);
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            SelectFilterFor(x => x.Importer).Title("Importer Name").DataSourceFrom<ClientFullNameWithEinDataProvider>().SetOperand(FilterOperands.Equal);
            DateRangeFilterFor(x => x.ImportDate).Title("Import date");
            TextFilterFor(x => x.EntryNo).Title("Entry No");
            SelectFilterFor(x => x.TransportMode).Title("Transport Mode").DataSourceFrom<TransportModeCodesDataProvider>().NotSearch().IsMultiSelect();
            SelectFilterFor(x => x.Tariff).Title("Tariff").DataSourceFrom<TariffDataProvider>().SetOperand(FilterOperands.Equal);
            SelectFilterFor(x => x.ReconIssue).Title("Recon Issue").DataSourceFrom<ReconIssueDataProvider>().SetOperand(FilterOperands.Equal).NotSearch().IsMultiSelect();
            SelectFilterFor(x => x.NaftaRecon).Title("FTA Recon").DataSourceFrom<ReconFtaDataProvider>().SetOperand(FilterOperands.Custom).NotSearch();
            SelectFilterFor(x => x.PsaReason).Title("PSA Reason").DataSourceFrom<PsaReasonFilterDataProvider>().SetOperand(FilterOperands.Custom).NotSearch().IsMultiSelect();
            SelectFilterFor(x => x.FtaEligibility).Title("FTA Eligibility").DataSourceFrom<YesNoTextFilterDataProvider>().SetOperand(FilterOperands.Equal).NotSearch();
            SelectFilterFor(x => x.Status).Title("FTA Update Status").DataSourceFrom<StatusFilterDataProvider<FtaReconStatus, int>>().NotSearch().IsMultiSelect();
            DateRangeFilterFor(x => x.CreatedDate).Title("Upload Date");
            SelectFilterFor(x => x.CreatedUser).Title("Upload User").DataSourceFrom<CreatedUserDataProvider<FtaRecon>>().NotSearch();
        }
    }
}
