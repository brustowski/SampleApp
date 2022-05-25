namespace FilingPortal.Domain.Common.Import.Models
{
    /// <summary>
    /// Represents file processing result
    /// </summary>
    public class FileProcessingDetailedResult : FileProcessingResult
    {
        /// <summary>
        /// Gets or sets the number of the inserted records
        /// </summary>
        public int Inserted { get; set; }
        /// <summary>
        /// Gets or sets the number of the updated records
        /// </summary>
        public int Updated { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="FileProcessingResult" /> class
        /// </summary>
        public FileProcessingDetailedResult(string fileName) : base(fileName)
        {
            Updated = 0;
            Inserted = 0;
        }
    }
}
