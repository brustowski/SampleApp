namespace FilingPortal.Domain.Common.Parsing
{
    /// <summary>
    /// Represents Property map option object
    /// </summary>
    /// <typeparam name="T">Type of the property</typeparam>
    class PropertyMapOptions<T> : IPropertyMapOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMapOptions{T}"/> class
        /// </summary>
        /// <param name="mapInfo"><see cref="PropertyMapInfo{T}"/> object</param>
        public PropertyMapOptions(PropertyMapInfo<T> mapInfo)
        {
        }
    }
}
