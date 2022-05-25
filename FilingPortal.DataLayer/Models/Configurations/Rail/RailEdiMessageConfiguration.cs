using FilingPortal.Domain.Entities.Rail;

namespace FilingPortal.DataLayer.Models.Configurations.Rail
{

    public class RailEdiMessageConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<RailEdiMessage>
    {
        public RailEdiMessageConfiguration()
            : this("dbo")
        {
        }

        public RailEdiMessageConfiguration(string schema)
        {
            ToTable("imp_rail_broker_download", schema);

            Property(x => x.EmMessageText).HasColumnType("varchar(max)").IsRequired();
            Property(x => x.CreatedDate).IsOptional();
        }
    }

}
