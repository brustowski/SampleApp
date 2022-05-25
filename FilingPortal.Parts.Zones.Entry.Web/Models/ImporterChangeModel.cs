using System;
using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Zones.Entry.Web.Models
{
    /// <summary>
    /// Model with importer id changes
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false)]
    public class ImporterChangeModel
    {
        /// <summary>
        /// Gets or sets client id
        /// </summary>
        public Guid ClientId { get; set; }
    }
}
