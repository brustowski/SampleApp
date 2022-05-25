using System;

namespace Framework.Domain
{
    /// <summary>
    /// Entity with author field and creation date
    /// </summary>
    public abstract class AuditableEntity: Entity
    {
        /// <summary>
        /// Gets or sets Rule Creation Date
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets Rule Creator
        /// </summary>
        public virtual string CreatedUser { get; set; }
    }
}
