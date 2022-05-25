using FilingPortal.Domain.Entities.TruckExport;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.TruckExport
{
    public class TruckExportReadModelConfiguration : EntityTypeConfiguration<TruckExportReadModel>
    {
        public TruckExportReadModelConfiguration()
            : this("dbo")
        {
        }

        public TruckExportReadModelConfiguration(string schema)
        {
            ToTable("v_exp_truck_inbound_grid", schema);
            HasKey(x => x.Id);

            Property(x => x.JobStatus).HasColumnType("int");
            Property(x => x.Hazardous).HasMaxLength(3);

            Property(x => x.IsDeleted).HasColumnName(@"deleted");
        }
    }
}
