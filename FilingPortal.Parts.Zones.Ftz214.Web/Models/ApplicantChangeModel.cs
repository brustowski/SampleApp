using System;
using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Zones.Ftz214.Web.Models
{
    /// <summary>
    /// Model with importer id changes
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false)]
    public class ApplicantChangeModel
    {
        /// <summary>
        /// Gets or sets client id
        /// </summary>
        public Guid ClientId { get; set; }
    }
}
