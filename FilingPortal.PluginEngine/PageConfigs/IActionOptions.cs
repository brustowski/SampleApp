namespace FilingPortal.PluginEngine.PageConfigs
{
    /// <summary>
    /// Defines the Action Option
    /// </summary>
    /// <typeparam name="TItem">Action Model Type</typeparam>
    public interface IActionOptions<out TItem>
    {
        /// <summary>
        /// Always Available Action Configuration
        /// </summary>
        IActionOptions<TItem> AlwaysAvailable();

        /// <summary>
        /// The Action Availability From Rule
        /// </summary>
        /// <typeparam name="TRule">The Rule type</typeparam>
        IActionOptions<TItem> AvailabilityRulesFrom<TRule>() where TRule : IAvailableRule<TItem>, new();
    }
}
