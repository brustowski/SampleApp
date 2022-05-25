namespace FilingPortal.PluginEngine.Lookups
{
    /// <summary>
    /// Class for Search Information
    /// </summary>
    public class SearchInfo
    {
        /// <summary>
        /// Default limit of data to take from handbook
        /// </summary>
        private const int DefaultLimit = 20;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchInfo"/> class
        /// </summary>
        public SearchInfo(string search, int limit, bool searchByKey = false)
        {
            Search = search;
            Limit = limit == default(int) ? DefaultLimit : limit;
            SearchByKey = searchByKey;
        }

        /// <summary>
        /// Gets or sets the search text
        /// </summary>
        public string Search { get; }
        /// <summary>
        /// Gets or sets the limit for the search results
        /// </summary>
        public int Limit { get; }

        /// <summary>
        /// Gets if search should work by key
        /// </summary>
        public bool SearchByKey { get; }

        /// <summary>
        /// Gets or sets the value on which the search depends
        /// </summary>
        public string DependValue { get; set; }
        /// <summary>
        /// Gets or sets property name on which the search depends
        /// </summary>
        public string DependOn { get; set; }
    }
}