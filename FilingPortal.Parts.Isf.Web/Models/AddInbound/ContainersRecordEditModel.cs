using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Isf.Web.Models.AddInbound
{
    /// <summary>
    /// Container edit model
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false, FlattenHierarchy = true, Name = "IsfContainersRecordEditModel")]
    public class ContainersRecordEditModel
    {
        /// <summary>
        /// Gets or sets container record id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets container type
        /// </summary>
        public string ContainerType { get; set; }
        /// <summary>
        /// Gets or sets container number
        /// </summary>
        public string ContainerNumber { get; set; }
    }
}
