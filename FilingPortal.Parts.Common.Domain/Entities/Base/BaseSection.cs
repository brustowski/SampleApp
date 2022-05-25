using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Entities.Base
{
    /// <summary>
    /// Abstract fields section
    /// </summary>
    public abstract class BaseSection : Entity
    {
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the Table Name
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets the Stored Procedure name
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Section is an array
        /// </summary>
        public bool IsArray { get; set; }
        /// <summary>
        /// Gets or sets whether children should be displayed as a grid
        /// </summary>
        public bool DisplayAsGrid { get; set; }

        /// <summary>
        /// Gets or sets a value indication whether section is hidden from UI
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets the Parent Id
        /// </summary>
        public int? ParentId { get; set; }
    }
}