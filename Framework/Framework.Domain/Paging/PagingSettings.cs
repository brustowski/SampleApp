namespace Framework.Domain.Paging
{
    /// <summary>
    /// Represents the Paging Settings
    /// </summary>
    public class PagingSettings
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="PagingSettings"/> class
        /// </summary>
        public PagingSettings() : this(50)
        { }

        /// <summary>
        /// Initialize a new instance of the <see cref="PagingSettings"/> class
        /// </summary>
        /// <param name="pageSize">The page size</param>
        /// <param name="pageNumber">The page number</param>
        public PagingSettings(int pageSize, int pageNumber = 1)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="PagingSettings"/> class based on existing page settings
        /// </summary>
        /// <param name="settings">Paging settings</param>
        public PagingSettings(PagingSettings settings) : this()
        {
            if (settings == null)
            {
                return;
            }

            PageSize = settings.PageSize;
            PageNumber = settings.PageNumber;
        }

        /// <summary>
        /// Gets or sets the page number
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the page size
        /// </summary>
        public int PageSize { get; set; }
    }
}