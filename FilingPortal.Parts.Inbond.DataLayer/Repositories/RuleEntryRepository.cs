﻿using FilingPortal.Parts.Inbond.Domain.Entities;
using Framework.DataLayer;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Parts.Inbond.DataLayer.Repositories
{
    /// <summary>
    /// Represents the repository of the <see cref="RuleEntry" />
    /// </summary>
    internal class RuleEntryRepository : SearchRepositoryWithTypedId<RuleEntry, int>, IRuleRepository<RuleEntry>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RuleEntryRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public RuleEntryRepository(IUnitOfWorkFactory<UnitOfWorkInbondContext> unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Checks whether specified rule is not duplicating any other rule
        /// </summary>
        /// <param name="rule">The Rule to be checked</param>
        public bool IsDuplicate(RuleEntry rule)
        {
            return GetId(rule) != default(int);
        }

        /// <summary>
        /// Gets the duplicate rule
        /// </summary>
        /// <param name="rule">The rule to get</param>
        public int GetId(RuleEntry rule)
        {
            if (rule == null)
            {
                return default(int);
            }

            return Set.Where(x => x.Id != rule.Id
                                  && x.FirmsCodeId == rule.FirmsCodeId
                                  && x.ImporterId == rule.ImporterId
                                  && x.Carrier == rule.Carrier.Trim()
                                  && x.ConsigneeId == rule.ConsigneeId
                                  && x.UsPortOfDestination == rule.UsPortOfDestination.Trim())
                .Select(x => x.Id)
                .FirstOrDefault();
        }

        /// <summary>
        /// Checks whether default value record with specified id exist
        /// </summary>
        /// <param name="id">The default value record identifier</param>
        public bool IsExist(int id) => Set.Any(x => x.Id == id);
    }
}
