namespace FilingPortal.DataLayer.Models.Configurations.Rail
{
    using FilingPortal.Domain.Entities.Rail;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides Rail DefValues Manual Read Model entity type configuration
    /// </summary>
    public class RailDefValuesReadModelConfiguration : EntityTypeConfiguration<RailDefValuesReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailDefValuesReadModelConfiguration"/> class.
        /// </summary>
        public RailDefValuesReadModelConfiguration() : this("dbo")
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="RailDefValuesReadModelConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public RailDefValuesReadModelConfiguration(string schema)
        {
            ToTable("v_imp_rail_form_configuration", schema);
            HasKey(x => x.Id);
        }
    }
}