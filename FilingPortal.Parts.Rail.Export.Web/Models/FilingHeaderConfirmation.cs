using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Rail.Export.Web.Models
{
    /// <summary>
    /// Pair of filing header id and expected confirmation value
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false, FlattenHierarchy = true)]
    public class FilingHeaderConfirmation
    {
        /// <summary>
        /// Filing header ID
        /// </summary>
        [TsProperty]
        public int FilingHeaderId { get; set; }
        /// <summary>
        /// Filing header confirmation status
        /// </summary>
        [TsProperty]
        public bool Confirmed { get; set; }
    }
}