using System;
using FilingPortal.Domain.Entities;
using Framework.Domain;

namespace FilingPortal.Web.Tests.Common
{
    public class FakeRuleEntity : Entity, IRuleEntity
    {
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Rule Creation Date
        /// </summary>
        public DateTime CreatedDate { get; set; }
        public string CreatedUser { get; set; }
    }
}
