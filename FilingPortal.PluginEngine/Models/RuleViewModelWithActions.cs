using System;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Mapping;
using FilingPortal.PluginEngine.Common.Json;
using Newtonsoft.Json;

namespace FilingPortal.PluginEngine.Models
{
    /// <summary>
    /// Represents View model with actions
    /// </summary>
    public abstract class RuleViewModelWithActions : ViewModelWithActions, IRuleEntity
    {
        /// <summary>
        /// Gets or sets Rule Creation Date
        /// </summary>
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets Rule Creator
        /// </summary>
        public string CreatedUser { get; set; }
    }
}