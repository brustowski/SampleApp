using Framework.Domain.Paging;

namespace FilingPortal.Web.Models.DocumentTypeModels
{
    /// <summary>
    /// Class describing document type model for filtering
    /// </summary>
    public class DocumentTypeFilterItem : LookupItem
    {
        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }
    }
}