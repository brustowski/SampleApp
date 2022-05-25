using Reinforced.Typings.Attributes;

namespace FilingPortal.PluginEngine.GridConfigurations
{
    /// <summary>
    /// Enum describing highlighting type for row
    /// </summary>
    [TsEnum(FlattenHierarchy = true, IncludeNamespace = false)]
    public enum HighlightingType
    {
        /// <summary>
        /// Without highlighting
        /// </summary>
        NoHighlighting,

        /// <summary>
        /// The error highlighting type
        /// </summary>
        Error,
        
        /// <summary>
        /// Warning highlighting type
        /// </summary>
        Warning,

        /// <summary>
        /// Success row highlighting type
        /// </summary>
        Success,
        
        /// <summary>
        /// Info highlighting type
        /// </summary>
        Info
    }
}