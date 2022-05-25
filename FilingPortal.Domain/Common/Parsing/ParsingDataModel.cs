using System;
using System.Linq;
using System.Reflection;

namespace FilingPortal.Domain.Common.Parsing
{
    /// <summary>
    /// Represents Parsing data model
    /// </summary>
    public class ParsingDataModel : IParsingDataModel
    {
        /// <summary>
        /// Gets or sets corresponding row number in the file
        /// </summary>
        public int RowNumberInFile { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            PropertyInfo[] props = this.GetType().GetProperties();
            var vals = props.Where(x => x.Name != "RowNumberInFile").Select(x => (x.GetValue(this, null) ?? string.Empty).ToString()).ToArray();
            return string.Join("|", vals);
        }
    }
}
