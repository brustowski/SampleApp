namespace Framework.Domain.Repositories
{
    /// <summary>
    /// Represent the Sorting Settings
    /// </summary>
    public class SortingSettings
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="SortingSettings"/> class
        /// </summary>
        public SortingSettings()
        {
            SortOrder = SortOrder.Asc;
        }
        /// <summary>
        /// Initialize a new instance of the <see cref="SortingSettings"/> class based on existing settings
        /// </summary>
        /// <param name="settings">The sorting settings</param>
        public SortingSettings(SortingSettings settings) : this()
        {
            if (settings == null) return;
            Field = settings.Field;
            SortOrder = settings.SortOrder;
        }

        /// <summary>
        /// Gets or sets sorting field
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the sorting order
        /// </summary>
        public SortOrder SortOrder { get; set; }
    }
}