using System;
using FilingPortal.Domain.Common;
using FilingPortal.Parts.Common.Domain.Commands;

namespace FilingPortal.PluginEngine.PageConfigs
{
    /// <summary>
    /// Provides the Action Configuration
    /// </summary>
    public class ActionConfig
    {
        /// <summary>
        /// Provides the Action Availability Rule
        /// </summary>
        public Func<object, IPermissionChecker, bool> AvailableRule;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionConfig"/> class.
        /// </summary>
        /// <param name="name">The Action Name</param>
        public ActionConfig(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets the Action Name
        /// </summary>
        public string Name { get; private set; }
    }
}
