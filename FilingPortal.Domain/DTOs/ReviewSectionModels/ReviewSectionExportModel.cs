using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Common;
using Reinforced.Typings.Attributes;

namespace FilingPortal.Domain.DTOs.ReviewSectionModels
{
    /// <summary>
    /// Exchange model to get data from review section
    /// </summary>
    [TsClass(IncludeNamespace = false, FlattenHierarchy = true)]
    public class ReviewSectionExportModel : ReviewSection
    {
        /// <summary>
        /// Gets or sets Values
        /// </summary>
        public IEnumerable<IEnumerable<ReviewSectionField>> Values { get; set; }

        public IEnumerable<FPDynObject> GetDynamicData()
        {
            return Values.Select(value =>
                new FPDynObject(
                    value.ToDictionary(x => x.Id,
                        x => (object)x.Value)))
                .ToList();
        }
    }
}