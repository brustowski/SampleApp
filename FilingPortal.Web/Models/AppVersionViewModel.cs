namespace FilingPortal.Web.Models
{
    /// <summary>
    /// Class describing the application version information model
    /// </summary>
    public class AppVersionViewModel
    {
        /// <summary>
        /// Gets or sets the application version in shortened format
        /// </summary>
        public string ShortAppVersion { get; set; }
        /// <summary>
        /// Gets or sets full application version
        /// </summary>
        public string AppVersion { get; set; }
        /// <summary>
        /// Gets or sets the application build date
        /// </summary>
        public string AppBuildDate { get; set; }
    }
}