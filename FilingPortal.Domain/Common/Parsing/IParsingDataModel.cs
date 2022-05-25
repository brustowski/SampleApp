namespace FilingPortal.Domain.Common.Parsing
{
    /// <summary>
    /// Describes Parsing model
    /// </summary>
    public interface IParsingDataModel
    {
        /// <summary>
        /// Gets or sets corresponding row number in the file
        /// </summary>
        int RowNumberInFile { get; set; }
    }
}
