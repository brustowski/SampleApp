using System.Collections.Generic;

namespace Framework.Domain.Paging
{
    /// <summary>
    /// Represents the filters settings
    /// </summary>
    public class FiltersSet
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="FiltersSet"/> class
        /// </summary>
        public FiltersSet()
        {
            Filters = new List<Filter>();
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="FiltersSet"/> class based on the existing filter settings
        /// </summary>
        /// <param name="filters">Filter settings</param>
        public FiltersSet(FiltersSet filters) : this()
        {
            if (filters?.Filters == null) return;
            
            Filters.AddRange(filters.Filters);
        }

        /// <summary>
        /// Gets or sets the collection of the <see cref="Filters"/>
        /// </summary>
        public List<Filter> Filters { get; set; }
    }
}