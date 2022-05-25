using Reinforced.Typings.Attributes;
using System.ComponentModel;

namespace FilingPortal.Parts.Zones.Entry.Domain.Enums
{
    /// <summary>
    /// Defines the Inbound Document Types
    /// </summary>
    [TsEnum(IncludeNamespace = false, Name = "ZonesEntryDocumentTypes")]
    public enum DocumentTypes
    {
        /// <summary>
        /// Defines the 3461 Form
        /// </summary>
        [Description("3461 Form")]
        Form3461 = 3461,
        /// <summary>
        /// Defines the 7501 Form
        /// </summary>
        [Description("7501 Form")]
        Form7501 = 7501,
    }
}
