using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.Import;
using FilingPortal.Domain.Services.GridExport.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Domain.DTOs.ReviewSectionModels
{
    /// <summary>
    /// Represents the Review section
    /// </summary>
    public class ReviewSection
    {
        /// <summary>
        /// Gets or sets section name
        /// </summary>
        public string SectionName { get; set; }

        /// <summary>
        /// Gets or sets section columns
        /// </summary>
        public IEnumerable<ReviewSectionField> Columns { get; set; }

        /// <summary>
        /// Returns list of <see cref="IColumnMapInfo"></see>
        /// </summary>
        public IList<IColumnMapInfo> GetColumnInfos()
        {
            return Columns.Select(col => new ColumnMapInfo<FPDynObject>(col.Value, x => x.GetMember(col.Id), typeof(string)))
                .Cast<IColumnMapInfo>().ToList();
        }

        /// <summary>
        /// Returns the Import configuration
        /// </summary>
        public IImportConfiguration GetImportConfiguration()
        {
            var config = new ReviewSectionStaticConfiguration();

            return config;
        }
    }
}