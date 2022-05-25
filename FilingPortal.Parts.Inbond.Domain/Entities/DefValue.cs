using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Parts.Inbond.Domain.Entities
{
    /// <summary>
    /// Represents the Default Values
    /// </summary>
    public class DefValue : BaseDefValueWithSection<Section>, IConfigurationEntity
    {
        /// <summary>
        /// Gets or sets the value indicating whether this field is used for Marks and Remarks
        /// </summary>
        public bool MarksRemarks { get; set; }
    }
}
