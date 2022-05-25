using FilingPortal.Parts.Recon.Domain.Config;
using FilingPortal.Parts.Recon.Web.DataSources;
using FilingPortal.Parts.Recon.Web.Models;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.DataProviders.FilterProviders;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Lookups.Providers;
using Framework.Domain.Paging;

namespace FilingPortal.Parts.Recon.Web.GridConfigs
{
    /// <summary>
    /// Class describing the configuration for the import records grid
    /// </summary>
    public class CargoWiseRecordsGridConfig : GridConfiguration<InboundReadModelViewModel>
    {
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.CargoWiseRecords;

        /// <summary>
        /// Gets the name of the corresponding template file name
        /// </summary>
        public override string TemplateFileName => "recon_template.xlsx";
        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn(x => x.JobNumber).DisplayName("Job Number").MinWidth(120).DefaultSorted();
            AddColumn(x => x.Importer).DisplayName("Importer").MinWidth(110);
            AddColumn(x => x.ImporterNo).DisplayName("Importer No").MinWidth(110);
            AddColumn(x => x.BondType).DisplayName("Bond Type").MinWidth(110);
            AddColumn(x => x.SuretyCode).DisplayName("Surety Code").MinWidth(110);
            AddColumn(x => x.EntryType).DisplayName("Entry Type").MinWidth(110);
            AddColumn(x => x.Filer).MinWidth(110);
            AddColumn(x => x.EntryNo).DisplayName("Entry No").MinWidth(110);
            AddColumn(x => x.LineNo).DisplayName("Line No").MinWidth(110);
            AddColumn(x => x.LineNumber7501).DisplayName("7501 Line Number").MinWidth(110);
            AddColumn(x => x.ReconIssue).DisplayName("Recon Issue").MinWidth(110);
            AddColumn(x => x.ReconJobNumbersVl).DisplayName("Recon Job Numbers VL").MinWidth(110);
            AddColumn(x => x.NaftaRecon).DisplayName("FTA Recon").MinWidth(110);
            AddColumn(x => x.ReconJobNumbersNf).DisplayName("Recon JobNumbers NF").MinWidth(110);
            AddColumn(x => x.MainReconIssues).DisplayName("Main Recon Issues").MinWidth(110);
            AddColumn(x => x.EntryPort).DisplayName("Entry Port").MinWidth(110);
            AddColumn(x => x.DestinationState).DisplayName("Destination State").MinWidth(110);
            AddColumn(x => x.Vessel).DisplayName("Vessel").MinWidth(110);
            AddColumn(x => x.Voyage).DisplayName("Voyage").MinWidth(110);
            AddColumn(x => x.OwnerRef).DisplayName("Owner Ref").MinWidth(110);
            AddColumn(x => x.EnsStatus).DisplayName("ENS Status").MinWidth(110);
            AddColumn(x => x.EnsStatusDescription).DisplayName("ENS Status Description").MinWidth(110);
            AddColumn(x => x.GoodsDescription).DisplayName("Goods Description").MinWidth(110);
            AddColumn(x => x.Container).DisplayName("Container").MinWidth(110);
            AddColumn(x => x.CustomsAttribute1).DisplayName("Customs Attribute 1").MinWidth(110);
            AddColumn(x => x.MasterBill).DisplayName("Master Bill").MinWidth(110);
            AddColumn(x => x.InvoiceLineCharges).DisplayName("Invoice Line Charges").MinWidth(110);
            AddColumn(x => x.CalculatedValueReconDueDate).DisplayName("Calculated Value Recon Due Date").MinWidth(110);
            AddColumn(x => x.Calculated520DDueDate).DisplayName("Calculated 520D Due Date").MinWidth(110);
            AddColumn(x => x.CalculatedClientReconDueDate).DisplayName("Calculated Client Recon Due Date").MinWidth(110);
            AddColumn(x => x.FtaReconFiling).DisplayName("FTA Recon Filing").MinWidth(110);
            AddColumn(x => x.FtaDeadline).DisplayName("FTA Deadline").MinWidth(110);
            AddColumn(x => x.OtherDeadline).DisplayName("Other Deadline").MinWidth(110);
            AddColumn(x => x.CO).DisplayName("C/O").MinWidth(110);
            AddColumn(x => x.Spi).DisplayName("SPI").MinWidth(110);
            AddColumn(x => x.ManufacturerMid).DisplayName("Manufacturer MID").MinWidth(110);
            AddColumn(x => x.Tariff).DisplayName("Tariff").MinWidth(110);
            AddColumn(x => x.CustomsQty1).DisplayName("Customs Qty 1").MinWidth(110);
            AddColumn(x => x.CustomsUq1).DisplayName("Customs UQ 1").MinWidth(110);
            AddColumn(x => x.LineEnteredValue).DisplayName("Line Entered Value").MinWidth(110);
            AddColumn(x => x.Duty).DisplayName("Duty").MinWidth(110);
            AddColumn(x => x.Mpf).DisplayName("MPF").MinWidth(110);
            AddColumn(x => x.PayableMpf).DisplayName("Payable MPF").MinWidth(110);
            AddColumn(x => x.ImportDate).DisplayName("Import Date").MinWidth(110);
            AddColumn(x => x.Cancellation).DisplayName("Cancellation").MinWidth(110);
            AddColumn(x => x.PsaReason).DisplayName("PSA Reason").MinWidth(110);
            AddColumn(x => x.PsaFiledDate).DisplayName("PSA Filed Date").MinWidth(110);
            AddColumn(x => x.PsaReason520d).DisplayName("PSA Reason 520d").MinWidth(110);
            AddColumn(x => x.PsaFiledDate520d).DisplayName("PSA Filed Date 520d").MinWidth(110);
            AddColumn(x => x.PsaFiledBy).DisplayName("PSA Filed By").MinWidth(110);
            AddColumn(x => x.PscExplanation).DisplayName("PSC Explanation").MinWidth(110);
            AddColumn(x => x.PscReasonCodesHeader).DisplayName("PSC Reason Codes (Header)").MinWidth(110);
            AddColumn(x => x.PscReasonCodesLine).DisplayName("PSC Reason Codes (Line)").MinWidth(110);
            AddColumn(x => x.LiqDate).DisplayName("Liq. Date").MinWidth(110);
            AddColumn(x => x.LiqType).DisplayName("Liq. Type").MinWidth(110);
            AddColumn(x => x.DutyLiquidated).DisplayName("Duty Liquidated").MinWidth(110);
            AddColumn(x => x.TotalLiquidatedFees).DisplayName("Total Liquidated Fees").MinWidth(110);
            AddColumn(x => x.CbpForm28Action).DisplayName("CBP Form 28 Action").MinWidth(110);
            AddColumn(x => x.CbpForm29Action).DisplayName("CBP Form 29 Action").MinWidth(110);
            AddColumn(x => x.PriorDisclosureMisc).DisplayName("Prior Disclosure MISC").MinWidth(110);
            AddColumn(x => x.ProtestPetitionFiledStatMisc).DisplayName("Protest Petition Filed Stat MISC").MinWidth(110);
            AddColumn(x => x.TransportMode).DisplayName("Transport Mode").MinWidth(110);
            AddColumn(x => x.PreliminaryStatementDate).DisplayName("PSD").MinWidth(110);
            AddColumn(x => x.ExportDate).DisplayName("Export Date").MinWidth(110);
            AddColumn(x => x.ReleaseDate).DisplayName("Release Date").MinWidth(110);
            AddColumn(x => x.DutyPaidDate).DisplayName("Duty Paid Date").MinWidth(110);
            AddColumn(x => x.PaymentDueDate).DisplayName("Payment Due Date").MinWidth(110);
            AddColumn(x => x.Nafta303ClaimStatMisc).DisplayName("NAFTA 303 Claim Stat Misc").MinWidth(110);
            AddColumn(x => x.ProvProgTariff).DisplayName("Prov/Prog Tariff").MinWidth(110);
            AddColumn(x => x.InvoiceNumber).DisplayName("Invoice Number").MinWidth(110);
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            SelectFilterFor(x => x.Importer).Title("Importer Name").DataSourceFrom<ClientFullNameWithEinDataProvider>().SetOperand(FilterOperands.Equal);
            DateRangeFilterFor(x => x.ImportDate).Title("Import date");
            TextFilterFor(x => x.EntryNo).Title("Entry No").SetOperand(FilterOperands.Contains);
            SelectFilterFor(x => x.ReconIssue).Title("Recon Issue").DataSourceFrom<ReconIssueDataProvider>().SetOperand(FilterOperands.Equal).NotSearch().IsMultiSelect();
            SelectFilterFor(x => x.NaftaRecon).Title("FTA Recon").DataSourceFrom<ReconFtaDataProvider>().SetOperand(FilterOperands.Custom).NotSearch();
            SelectFilterFor(x => x.FtaReconFiling).Title("FTA Recon Filing").DataSourceFrom<ReconFtaReconFilingDataProvider>().SetOperand(FilterOperands.Equal).NotSearch().IsMultiSelect();
            DateRangeFilterFor(x => x.PreliminaryStatementDate).Title("PSD");
            SelectFilterFor(x => x.ReconJobNumbers).Title("Flagged for Recon").DataSourceFrom<JobNumberFilterDataProvider>().SetOperand(FilterOperands.Custom).NotSearch().IsMultiSelect();

            TextFilterFor(x => x.JobNumber).Title("Job Number").Advanced();
            SelectFilterFor(x => x.TransportMode).Title("Transport Mode").DataSourceFrom<TransportModeCodesDataProvider>().NotSearch().IsMultiSelect().Advanced();
            SelectFilterFor(x => x.Tariff).Title("Tariff").DataSourceFrom<TariffDataProvider>().SetOperand(FilterOperands.Equal).IsMultiSelect().Advanced();
            SelectFilterFor(x => x.EntryPort).Title("Port of Entry").DataSourceFrom<PortOfEntryFilterDataProvider>().SetOperand(FilterOperands.Custom).NotSearch().Advanced();
            TextFilterFor(x => x.Vessel).Title("Vessel Name").Advanced();
            SelectFilterFor(x => x.PsaReason).Title("PSA Reason").DataSourceFrom<PsaReasonFilterDataProvider>().SetOperand(FilterOperands.Custom).NotSearch().IsMultiSelect().Advanced();
            SelectFilterFor(x => x.PsaReason520d).Title("PSA Reason 520d").DataSourceFrom<PsaReasonFilterDataProvider>().SetOperand(FilterOperands.Custom).NotSearch().IsMultiSelect().Advanced();
            DateRangeFilterFor(x => x.FtaDeadline).Title("FTA Deadline").Advanced();
            DateRangeFilterFor(x => x.OtherDeadline).Title("Other Deadline").Advanced();
            SelectFilterFor(x => x.Errors).Title("Ace Comparison").DataSourceFrom<ReconAceComparisonErrorsDataProvider>().SetOperand(FilterOperands.Custom).NotSearch().Advanced();

        }
    }
}
