namespace Framework.Domain.Paging
{
    /// <summary>
    /// Represents the lookup item
    /// </summary>
    public class LookupItem
    {
        /// <summary>
        /// Gets or sets the display value
        /// </summary>
        public string DisplayValue { get; set; }
        
        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether lookup item should be selected by default
        /// </summary>
        public bool IsDefault { get; set; }
    }
    
    /// <summary>
    /// Represents the lookup item with the specified type value
    /// </summary>
    /// <typeparam name="TType">The type of the value</typeparam>
    public class LookupItem<TType>: LookupItem
    {
        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public new TType Value { get; set; }
    }
}