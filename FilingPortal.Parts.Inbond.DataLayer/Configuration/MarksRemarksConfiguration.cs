using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Inbond.Domain.Entities;

namespace FilingPortal.Parts.Inbond.DataLayer.Configuration
{
    /// <summary>
    /// Provides Marks and Remarks entity type configuration
    /// </summary>
    public class MarksRemarksConfiguration : EntityTypeConfiguration<MarksRemarksTemplate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarksRemarksConfiguration"/> class.
        /// </summary>
        public MarksRemarksConfiguration() : this("inbond")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MarksRemarksConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public MarksRemarksConfiguration(string schema)
        {
            ToTable("handbook_marks_remarks_template", schema);

            HasKey(x => x.Id);

            Property(x => x.EntryType).IsRequired();
            Property(x => x.TemplateType).IsRequired();
            Property(x => x.DescriptionTemplate).IsRequired().HasMaxLength(1000);
            Property(x => x.MarksNumbersTemplate).IsRequired().HasMaxLength(500);

            HasIndex(x => new {x.EntryType, x.TemplateType}).IsUnique();
        }
    }
}
