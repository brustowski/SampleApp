namespace FilingPortal.PluginEngine.PageConfigs
{
    /// <summary>
    /// Defines the Page Configuration Container
    /// </summary>
    public interface IPageConfigContainer
    {
        /// <summary>
        /// Provides Page Configuration for specified page name
        /// </summary>
        /// <param name="pageName">The Page Name</param>
        IPageConfiguration GetPageConfig(string pageName);

        /// <summary>
        /// Provides Def Values page configuration
        /// </summary>
        IPageConfiguration GetDefValueActionsConfig();
    }
}
