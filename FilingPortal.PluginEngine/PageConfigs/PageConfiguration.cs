using System.Collections.Generic;
using FilingPortal.Domain.Common;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.PluginEngine.Models;

namespace FilingPortal.PluginEngine.PageConfigs
{
    /// <summary>
    /// Represents the Page Configuration of the specified type
    /// </summary>
    /// <typeparam name="TItem">The model type</typeparam>
    public abstract class PageConfiguration<TItem> : IPageConfiguration
    {
        /// <summary>
        /// List of the Action Configuration
        /// </summary>
        private readonly List<ActionConfig> _actionConfigs = new List<ActionConfig>();

        /// <summary>
        /// Gets or sets the Page Name
        /// </summary>
        public string PageName { get; protected set; }

        /// <summary>
        /// Adds Action with specified name to the action list
        /// </summary>
        /// <param name="name">The action name</param>
        protected IActionOptions<TItem> AddAction(string name)
        {
            var config = new ActionConfig(name);
            _actionConfigs.Add(config);
            return new ActionOptions<TItem>(config);
        }

        /// <summary>
        /// Prepares Current page Configuration
        /// </summary>
        public abstract void Configure();

        /// <summary>
        /// Provides <see cref="Actions"/> for the specified model and user
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public Actions GetActions(object model, IPermissionChecker resourceRequestor)
        {
            var actions = new Actions();

            foreach (ActionConfig action in _actionConfigs)
            {
                actions.Add(action.Name, action.AvailableRule(model, resourceRequestor));
            }

            return actions;
        }
    }
}
