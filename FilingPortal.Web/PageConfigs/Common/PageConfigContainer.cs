using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Web.PageConfigs.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents the Page Configuration Container
    /// </summary>
    public class PageConfigContainer : IPageConfigContainer
    {
        /// <summary>
        /// Page Configuration collection
        /// </summary>
        private readonly IEnumerable<IPageConfiguration> _pageConfigurations;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageConfigContainer"/> class.
        /// </summary>
        /// <param name="pageConfigurations">Page Configuration collection</param>
        public PageConfigContainer(IEnumerable<IPageConfiguration> pageConfigurations)
        {
            _pageConfigurations = pageConfigurations;

            foreach (IPageConfiguration pageConfiguration in _pageConfigurations)
            {
                pageConfiguration.Configure();
            }
        }

        /// <summary>
        /// Provides Page Configuration for specified page name
        /// </summary>
        /// <param name="pageName">The Page Name</param>
        public IPageConfiguration GetPageConfig(string pageName)
        {
            var result = _pageConfigurations.FirstOrDefault(x => x.PageName == pageName);
            if (result != null)
            {
                return result;
            }
            throw new InvalidOperationException("The Page config was not found for " + pageName);
        }

        /// <summary>
        /// Provides Def Values page configuration
        /// </summary>
        public IPageConfiguration GetDefValueActionsConfig()
        {
            return GetPageConfig(PageConfigNames.DefValueActionsConfigName);
        }
    }
}
