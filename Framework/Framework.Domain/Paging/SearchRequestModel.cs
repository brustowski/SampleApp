using System;
using Framework.Domain.Repositories;

namespace Framework.Domain.Paging
{
    /// <summary>
    /// Represents the Search request model
    /// </summary>
    public class SearchRequestModel
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="SearchRequestModel"/> class
        /// </summary>
        public SearchRequestModel()
        {
            SortingSettings = new SortingSettings();
            PagingSettings = new PagingSettings();
            FilterSettings = new FiltersSet();
        }
        
        /// <summary>
        /// Initialize a new instance of the <see cref="SearchRequestModel"/> class based on existing search request model
        /// </summary>
        /// <param name="model">Search request model</param>
        public SearchRequestModel(SearchRequestModel model)
        {
            SortingSettings = new SortingSettings(model.SortingSettings);
            PagingSettings = new PagingSettings(model.PagingSettings);
            FilterSettings = new FiltersSet(model.FilterSettings);
        }

        /// <summary>
        /// Gets or sets the <see cref="PagingSettings"/> 
        /// </summary>
        public PagingSettings PagingSettings { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SortingSettings"/> 
        /// </summary>
        public SortingSettings SortingSettings { get; set; }
        /// <summary>
        /// Gets or sets the <see cref="FilterSettings"/> 
        /// </summary>
        public FiltersSet FilterSettings { get; set; }
    }
}