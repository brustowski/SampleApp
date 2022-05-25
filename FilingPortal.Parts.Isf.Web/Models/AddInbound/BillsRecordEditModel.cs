using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Isf.Web.Models.AddInbound
{
    /// <summary>
    /// Bill edit model
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false, FlattenHierarchy = true, Name = "IsfInboundBillRecordEditModel")]
    public class BillsRecordEditModel
    {
        /// <summary>
        /// Gets or sets bill record id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets bill type
        /// </summary>
        public string BillType { get; set; }

        /// <summary>
        /// Gets or sets bill number
        /// </summary>
        public string BillNumber { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        [TsIgnore]
        public override string ToString()
        {
            return $"{BillType}|{BillNumber}";
        }
    }
}
