namespace FilingPortal.DataLayer.Models.Configurations.Rail
{
    using FilingPortal.Domain.Entities.Rail;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides Rail Tables entity type configuration
    /// </summary>
    internal class RailTablesConfiguration : EntityTypeConfiguration<RailTables>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailTablesConfiguration"/> class.
        /// </summary>
        public RailTablesConfiguration() : this("dbo") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="RailTablesConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public RailTablesConfiguration(string schema)
        {
            ToTable("v_imp_rail_field_configuration", schema);
        }
    }
}
