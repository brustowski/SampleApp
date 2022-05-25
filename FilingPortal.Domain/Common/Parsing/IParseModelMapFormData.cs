using System.Collections.Generic;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Common.Parsing
{
    /// <summary>
    /// Describes mapping parsed data to form model
    /// </summary>
    public interface IParseModelMapFormData : IParseModelMap
    {
        /// <summary>
        /// Gets or sets the form section
        /// </summary>
        string Section { get; set; }
    }
}
