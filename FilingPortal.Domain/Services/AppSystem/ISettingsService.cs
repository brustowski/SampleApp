namespace FilingPortal.Domain.Services.AppSystem
{
    /// <summary>
    /// Describes settings service
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Gets value of settings parameter
        /// </summary>
        /// <typeparam name="T">Cast to type</typeparam>
        /// <param name="paramName">Parameter name</param>
        T Get<T>(string paramName);

        /// <summary>
        /// Sets string value to parameter
        /// </summary>
        /// <param name="paramName">Parameter name</param>
        /// <param name="value">Value</param>
        void Set(string paramName, string value);

        /// <summary>
        /// Creates new parameter
        /// </summary>
        /// <param name="paramName">Parameter name</param>
        /// <param name="section">Section name</param>
        /// <param name="value">Value</param>
        /// <param name="description">Description</param>
        void Create(string paramName, string section, string value, string description = "");
    }
}