using FilingPortal.DataLayer.Models.Configurations.Vessel;
using FilingPortal.Domain.Entities.Handbooks;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.Handbook
{
    /// <summary>
    /// Provides Model Configuration for <see cref="VesselHandbookRecord"/>
    /// </summary>
    public class VesselHandbookRecordConfiguration : EntityTypeConfiguration<VesselHandbookRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportConfiguration"/> class.
        /// </summary>
        public VesselHandbookRecordConfiguration()
            : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselHandbookRecordConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public VesselHandbookRecordConfiguration(string schema)
        {
            ToTable("handbook_vessel", schema);
            HasKey(x => x.Id);

            //Columns
            Property(x => x.Name).IsRequired();
        }
    }
}
