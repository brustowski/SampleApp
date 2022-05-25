namespace FilingPortal.Domain.Services
{
    /// <summary>
    /// Interface for Excel file builder service
    /// </summary>
    public interface IExcelDocumentBuilder
    {
        /// <summary>
        /// Open builder from bytes array
        /// </summary>
        /// <param name="bytes">The content of the excel as byte array</param>
        IExcelDocumentBuilder Open(byte[] bytes);
        
        /// <summary>
        /// Sets the value to the named cell
        /// </summary>
        /// <param name="cellName">Name of the cell</param>
        /// <param name="value">Value</param>
        IExcelDocumentBuilder SetNamedCellValue(string cellName, object value);

        /// <summary>
        /// Sets the value of the "FullCalculationOnLoad" property
        /// </summary>
        /// <param name="value">True - to enable full calculation on load, otherwise - false</param>
        IExcelDocumentBuilder SetFullCalculationOnLoad(bool value);


        /// <summary>
        /// Provides content of the file as byte array
        /// </summary>
        byte[] ToByteArray();
    }
}