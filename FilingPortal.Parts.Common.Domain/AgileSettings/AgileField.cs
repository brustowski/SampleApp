using FilingPortal.Parts.Common.Domain.Common;

namespace FilingPortal.Parts.Common.Domain.AgileSettings
{
    /// <summary>
    /// Column configuration for agile grid configurations
    /// </summary>
    public class AgileField
    {
        /// <summary>
        /// average symbol length in pixels
        /// </summary>
        private const int AvgSymbolLength = 5;

        /// <summary>
        /// Field display name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Max field length (is possible)
        /// </summary>
        public int? MaxLength { get; private set; }

        /// <summary>
        /// Max symbols in text field
        /// </summary>
        public int? MaxSymbolsLength
        {
            set => MaxLength = value * AvgSymbolLength;
        }

        /// <summary>
        /// Table name
        /// </summary>
        public string TableName { get; set; }
        
        /// <summary>
        /// Field name
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Field name
        /// </summary>
        public DefValuesUniqueData FieldId => new DefValuesUniqueData(TableName, ColumnName);

        /// <summary>
        /// Column order
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Column type
        /// </summary>
        public string TypeName { get; set; }
    }
}
