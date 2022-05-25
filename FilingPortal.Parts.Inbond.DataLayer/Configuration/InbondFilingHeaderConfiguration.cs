using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Inbond.Domain.Entities;

namespace FilingPortal.Parts.Inbond.DataLayer.Configuration
{
    /// <summary>
    /// Provides Vessel Import Filing Header type Configuration
    /// </summary>
    public class InbondFilingHeaderConfiguration : EntityTypeConfiguration<FilingHeader>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InbondFilingHeaderConfiguration"/> class.
        /// </summary>
        public InbondFilingHeaderConfiguration() : this("inbond") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InbondFilingHeaderConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public InbondFilingHeaderConfiguration(string schema)
        {
            ToTable("filing_header", schema);
            HasKey(x => x.Id);

            Property(x => x.FilingNumber).HasMaxLength(255);
            Property(x => x.CreatedDate).IsRequired();
            Property(x => x.CreatedUser).IsRequired().HasMaxLength(128);
            Property(x => x.MappingStatus).HasColumnType("tinyint");
            Property(x => x.FilingStatus).HasColumnType("tinyint");
            Property(x => x.JobLink).IsMaxLength();

            HasMany(t => t.InbondRecords).WithMany(t => t.FilingHeaders)
                .Map(m =>
                {
                    m.ToTable("filing_detail", schema);
                    m.MapLeftKey("Filing_Headers_FK");
                    m.MapRightKey("Z_FK");
                });
        }
    }
}
