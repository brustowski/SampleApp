using FilingPortal.Domain.Common.Parsing;
using Framework.Domain;

/// <summary>
/// Customs entry from XML
/// </summary>
public partial class FTZ_214FTZ_ADMISSION : Entity, IParsingDataModel
{
    /// <summary>
    /// Gets or sets corresponding row number in the file
    /// </summary>
    public int RowNumberInFile { get; set; }
}