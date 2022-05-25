using System.Linq;

namespace FilingPortal.Infrastructure.Report
{
    /// <summary>
    /// Provides methods for Excel Cell reference generation
    /// </summary>
    internal class ExcelCellRefGenerator
    {
        /// <summary>
        /// Array of all allowed references
        /// </summary>
        private readonly string[] _names;

        /// <summary>
        /// Initialize a new instance of the <see cref="ExcelCellRefGenerator"/> class
        /// </summary>
        public ExcelCellRefGenerator()
        {
            string[] alphabet = { string.Empty, "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

            _names = (from c1 in alphabet
                      from c2 in alphabet
                      from c3 in alphabet.Skip(1)                    // c3 is never empty
                      where c1 == string.Empty || c2 != string.Empty // only allow c2 to be empty if c1 is also empty
                      select c1 + c2 + c3).ToArray();
        }

        /// <summary>
        /// Gets the Cell Reference for specified row and column numbers
        /// </summary>
        /// <param name="rowNumber">The row number</param>
        /// <param name="columnNumber">The column number</param>
        public string GetReference(uint rowNumber, uint columnNumber)
        {
            return $"{_names[columnNumber]}{rowNumber}";
        }
    }
}
