namespace FilingPortal.PluginEngine.PageConfigs
{
    /// <summary>
    /// Represents the Action Option
    /// </summary>
    /// <typeparam name="TItem">Action Model Type</typeparam>
    internal class ActionOptions<TItem> : IActionOptions<TItem>
    {
        /// <summary>
        /// The Action Configuration
        /// </summary>
        private readonly ActionConfig _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionOptions{TItem}"/> class.
        /// </summary>2
        /// <param name="config">The Action Configuration</param>
        public ActionOptions(ActionConfig config)
        {
            _config = config;
        }

        /// <summary>
        /// Always Available Action Configuration
        /// </summary>
        public IActionOptions<TItem> AlwaysAvailable()
        {
            _config.AvailableRule = (o, u) => true;
            return this;
        }

        /// <summary>
        /// The Action Availability From Rule
        /// </summary>
        /// <typeparam name="TRule">The Rule type</typeparam>
        public IActionOptions<TItem> AvailabilityRulesFrom<TRule>() where TRule : IAvailableRule<TItem>, new()
        {
            _config.AvailableRule = (o, u) => new TRule().IsAvailable((TItem)o, u);
            return this;
        }
    }
}
