namespace Framework.Domain.Paging
{
    /// <summary>
    /// Represents Paged Result With Summary Row
    /// </summary>
    /// <typeparam name="T">Type of the result</typeparam>
    public class PagedResultWithSummaryRow<T> : SimplePagedResult<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedResultWithSummaryRow{T}"/> class.
        /// </summary>
        public PagedResultWithSummaryRow()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedResultWithSummaryRow{T}"/> class.
        /// </summary>
        /// <param name="simplePagedResult">The base class instance</param>
        public PagedResultWithSummaryRow(SimplePagedResult<T> simplePagedResult)
        {
            CurrentPage = simplePagedResult.CurrentPage;
            PageSize = simplePagedResult.PageSize;
            Results = simplePagedResult.Results;
        }

        /// <summary>
        /// Gets or sets the Summary
        /// </summary>
        public T Summary { get; set; }
    }
}
