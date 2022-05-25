namespace FilingPortal.PluginEngine.Models
{
    /// <summary>
    /// Provides access to individual field that have been posted by a client 
    /// </summary>
    public class HttpPostedField
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpPostedField"/> class
        /// </summary>
        /// <param name="name">Name of the field</param>
        /// <param name="value">Value of the field</param>
        public HttpPostedField(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Gets or sets Name of the field
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Gets or sets Value of the field
        /// </summary>
        public string Value { get; private set; }
    }
}