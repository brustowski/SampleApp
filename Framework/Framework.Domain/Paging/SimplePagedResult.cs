namespace Framework.Domain.Paging
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents Simple Paged Result
    /// </summary>
    /// <typeparam name="T">Type of the results</typeparam>
    public class SimplePagedResult<T>
    {
        /// <summary>
        /// Gets or sets the Current Page
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the Page Size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the Results
        /// </summary>
        public IEnumerable<T> Results { get; set; }
    }
}
