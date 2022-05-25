﻿using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Rail.Export.Domain.Entities;

namespace FilingPortal.Parts.Rail.Export.DataLayer.Configuration
{
    /// <summary>
    /// Provides Tables entity type configuration
    /// </summary>
    internal class TablesConfiguration : EntityTypeConfiguration<Tables>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TablesConfiguration"/> class.
        /// </summary>
        public TablesConfiguration() : this("us_exp_rail") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="TablesConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public TablesConfiguration(string schema)
        {
            ToTable("v_field_configuration", schema);
        }
    }
}
