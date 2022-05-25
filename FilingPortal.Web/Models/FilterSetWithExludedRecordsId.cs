namespace FilingPortal.Web.Models
{
    using Framework.Domain.Paging;
    using System.Collections.Generic;

    /// <summary>
    /// Represents Filter set with list of excluded records id
    /// </summary>
    public class FilterSetWithExludedRecordsId : FiltersSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterSetWithExludedRecordsId"/> class.
        /// </summary>
        public FilterSetWithExludedRecordsId() : base()
        {
        }

        /// <summary>
        /// Gets or sets the Excluded Records Id
        /// </summary>
        public IEnumerable<int> ExcludedRecordsId { get; set; }
    }
}
