using System;
using System.Collections.Generic;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Repositories.AppSystem;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using Framework.Infrastructure;

namespace FilingPortal.Domain.Services.AppSystem
{
    /// <summary>
    /// Implements settings service
    /// </summary>
    public class SettingsService : ISettingsService
    {
        private static readonly IDictionary<string, string> DefaultSettings = new Dictionary<string, string>
        {
            {SettingsNames.RailDailyAuditCustomsQtyWarningThreshold, "0.05" },
            {SettingsNames.RailDailyAuditCustomsQtyErrorThreshold, "0.1" },
        };

        private readonly IAppSettingsRepository _repository;

        /// <summary>
        /// Creates a new instance of <see cref="SettingsService"/>
        /// </summary>
        /// <param name="repository">Application settings repository</param>
        public SettingsService(IAppSettingsRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets value of settings parameter
        /// </summary>
        /// <typeparam name="T">Cast to type</typeparam>
        /// <param name="paramName">Parameter name</param>
        public T Get<T>(string paramName)
        {
            AppSettings param = _repository.Get(paramName);
            if (param != null)
            {
                try
                {
                    return (T)Convert.ChangeType(param.Value, typeof(T));
                }
                catch
                {
                    AppLogger.Error(
                        $"Can't convert settings value {paramName}='{param.Value}' to type {typeof(T).Name}");
                    return (T)Convert.ChangeType(DefaultSettings[paramName], typeof(T));
                }
            }

            return (T)Convert.ChangeType(DefaultSettings[paramName], typeof(T));
        }

        /// <summary>
        /// Sets string value to parameter
        /// </summary>
        /// <param name="paramName">Parameter name</param>
        /// <param name="value">Value</param>
        public void Set(string paramName, string value)
        {
            AppSettings param = _repository.Get(paramName);
            param.Value = value;
            _repository.Update(param);
            _repository.Save();
        }

        /// <summary>
        /// Creates new parameter
        /// </summary>
        /// <param name="paramName">Parameter name</param>
        /// <param name="section">Section name</param>
        /// <param name="value">Value</param>
        /// <param name="description">Description</param>
        public void Create(string paramName, string section, string value, string description = "")
        {
            AppSettings param = _repository.Get(paramName);
            if (param == null)
            {
                param = new AppSettings { Id = paramName, Section = section, Description = description, Value = value };
                _repository.Add(param);
                _repository.Save();
            }
        }
    }
}
