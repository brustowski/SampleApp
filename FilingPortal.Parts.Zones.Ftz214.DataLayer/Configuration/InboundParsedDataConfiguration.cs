using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.Parts.Zones.Ftz214.DataLayer.Configuration
{
    /// <summary>
    /// Provides Inbound Parsed Data entity type configuration
    /// </summary>
    public class InboundParsedDataConfiguration : EntityTypeConfiguration<InboundParsedData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundParsedDataConfiguration"/> class.
        /// </summary>
        public InboundParsedDataConfiguration()
            : this("zones_ftz214")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundParsedDataConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public InboundParsedDataConfiguration(string schema)
        {
            ToTable("inbound_parsed_data", schema);
            HasKey(x => x.Id);

            Property(x => x.CompanyCode).HasMaxLength(10);
            Property(x => x.OfficeCode).HasMaxLength(10);
            Property(x => x.FileNo).HasMaxLength(9);
            Property(x => x.FilerCode).HasMaxLength(3);
            Property(x => x.RecordExistsAction).HasMaxLength(1);
            Property(x => x.PartnerKey).HasMaxLength(35);
            Property(x => x.PartnerKey2).HasMaxLength(20);
            Property(x => x.PartnerKey3).HasMaxLength(20);
            Property(x => x.PartnerKey4).HasMaxLength(20);
            Property(x => x.AdmissionNo).HasMaxLength(8);
            Property(x => x.AdmissionYear).HasMaxLength(2);
            Property(x => x.ZoneNo).HasMaxLength(7);
            Property(x => x.ZonePort).HasMaxLength(4);
            Property(x => x.DirectDelivery).HasMaxLength(1);
            Property(x => x.AbiRouting).HasMaxLength(9);
            Property(x => x.SubmitterIrsNo).HasMaxLength(12);
            Property(x => x.ApplicantIrsNo).HasMaxLength(12);
            Property(x => x.SentToCensus).HasMaxLength(1);
            Property(x => x.AuthorizedGoodsDesc).HasMaxLength(360);
            Property(x => x.ExceptionsExist).HasMaxLength(1);
            Property(x => x.ExceptGoodsDesc).HasMaxLength(360);

            Property(x => x.AdmissionType).HasMaxLength(1);
            Property(x => x.UnladingPort).HasMaxLength(4);
            Property(x => x.Mot).HasMaxLength(2);
            Property(x => x.ImpCarrierCode).HasMaxLength(4);
            Property(x => x.ImpCarrierName).HasMaxLength(30);
            Property(x => x.ImpVessel).HasMaxLength(30);
            Property(x => x.ImpVesselCountryCode).HasMaxLength(2);
            Property(x => x.FltVoyTrip).HasMaxLength(15);
            Property(x => x.UserProvidedSfTransactionNo).HasMaxLength(15);

            Property(x => x.PttFirms).HasMaxLength(4);
            Property(x => x.PttIrsNo).HasMaxLength(12);
            Property(x => x.ItNo).HasMaxLength(35);
            Property(x => x.ItCarrierCode).HasMaxLength(4);
            Property(x => x.ItCarrierName).HasMaxLength(35);
            Property(x => x.ItFromPort).HasMaxLength(4);
            Property(x => x.ItFromZoneNo).HasMaxLength(7);

            Property(x => x.Master).HasMaxLength(35);
            Property(x => x.House).HasMaxLength(20);
            Property(x => x.Qtyuom).HasMaxLength(15);
            Property(x => x.Ce).HasMaxLength(2);
            Property(x => x.ForeignPort).HasMaxLength(5);
            Property(x => x.ForeignPortCode).HasMaxLength(5);
            Property(x => x.ForeignPortName).HasMaxLength(35);
            Property(x => x.BtaIndicator).HasMaxLength(1);
            Property(x => x.Container).IsMaxLength();

            HasRequired(x => x.InboundRecord)
                .WithOptional(x=>x.InboundParsedData)
                .WillCascadeOnDelete(true);
        }
    }
}
