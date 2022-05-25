using System;

namespace FilingPortal.Domain.Entities
{
    /// <summary>
    /// Defines Rule Entity
    /// </summary>
    public interface IRuleEntity
    {
        /// <summary>
        /// Gets or sets Rule Creation Date
        /// </summary>
        DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets Rule Creator
        /// </summary>
        string CreatedUser { get; set; }
    }
}
