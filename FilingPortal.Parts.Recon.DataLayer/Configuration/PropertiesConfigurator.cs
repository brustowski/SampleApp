using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Recon.Domain.Entities;
using Framework.Domain;

namespace FilingPortal.Parts.Recon.DataLayer.Configuration
{
    /// <summary>
    /// Entity configuration for both inbound record and client record
    /// </summary>
    /// <typeparam name="T">Inbound record type</typeparam>
    public class PropertiesConfigurator<T> : EntityTypeConfiguration<T>
    where T : Entity, IBaseInboundRecord
    {
        protected void ConfigureProperties()
        {
            HasKey(x => x.Id);

            Property(x => x.JobNumber).HasMaxLength(25);
            Property(x => x.Importer).HasMaxLength(100);
            Property(x => x.ImporterNo).HasMaxLength(259);
            Property(x => x.BondType).HasMaxLength(8000);
            Property(x => x.SuretyCode).HasMaxLength(100);
            Property(x => x.EntryType).HasMaxLength(100);
            Property(x => x.Filer).HasMaxLength(100);
            Property(x => x.EntryNo).HasMaxLength(35);
            Property(x => x.LineNumber7501).HasMaxLength(8000);
            Property(x => x.ReconIssue).HasMaxLength(100);
            Property(x => x.NaftaRecon).HasMaxLength(100);
            Property(x => x.ReconJobNumbers).IsMaxLength();
            Property(x => x.MainReconIssues).IsMaxLength();
            Property(x => x.FtaReconFiling).HasMaxLength(100);
            Property(x => x.CO).HasMaxLength(2);
            Property(x => x.Spi).HasMaxLength(8000);
            Property(x => x.ManufacturerMid).HasMaxLength(254);
            Property(x => x.Tariff).HasMaxLength(35);
            Property(x => x.CustomsUq1).HasMaxLength(3);
            Property(x => x.Cancellation).HasMaxLength(100);
            Property(x => x.PsaReason).HasMaxLength(100);
            Property(x => x.PsaReason520d).HasMaxLength(100);
            Property(x => x.PsaFiledBy).HasMaxLength(100);
            Property(x => x.PscExplanation).HasMaxLength(1024);
            Property(x => x.PscReasonCodesHeader).HasMaxLength(350);
            Property(x => x.PscReasonCodesLine).HasMaxLength(350);
            Property(x => x.LiqType).HasMaxLength(1);
            Property(x => x.CbpForm28Action).HasMaxLength(100);
            Property(x => x.CbpForm29Action).HasMaxLength(100);
            Property(x => x.PriorDisclosureMisc).HasMaxLength(100);
            Property(x => x.ProtestPetitionFiledStatMisc).HasMaxLength(100);
            Property(x => x.TransportMode).HasMaxLength(3);

            Property(x => x.EntryPort).HasMaxLength(4);
            Property(x => x.DestinationState).HasMaxLength(2);
            Property(x => x.Vessel).HasMaxLength(35);
            Property(x => x.Voyage).HasMaxLength(10);
            Property(x => x.OwnerRef).HasMaxLength(35);
            Property(x => x.EnsStatus).HasMaxLength(3);
            Property(x => x.EnsStatusDescription).HasMaxLength(100);
            Property(x => x.GoodsDescription).HasMaxLength(1000);
            Property(x => x.Container).IsMaxLength();
            Property(x => x.MasterBill).IsMaxLength();

            Property(x => x.InvoiceNumber).HasMaxLength(35);
            Property(x => x.FtaReconFlaggedNotFiled).HasMaxLength(1);
            Property(x => x.ValueReconFlaggedNotFiled).HasMaxLength(1);
        }
    }
}
