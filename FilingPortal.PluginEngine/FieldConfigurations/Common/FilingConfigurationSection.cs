using Reinforced.Typings.Attributes;

namespace FilingPortal.PluginEngine.FieldConfigurations.Common
{
    /// <summary>
    /// Represents filing section configuration
    /// </summary>
    [TsInterface(Name = "FilingConfigurationSectionServer", IncludeNamespace = false, AutoI = false, Order = 201)]
    public class FilingConfigurationSection
    {
        /// <summary>
        /// Gets or sets the identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the Title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the parent record identifier
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this section is single
        /// </summary>
        public bool IsSingleSection { get; set; }
        /// <summary>
        /// Gets or sets whether children should be displayed as a grid
        /// </summary>
        public bool DisplayAsGrid { get; set; }
        
    }
}