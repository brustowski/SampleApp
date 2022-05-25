using System.Linq;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Rail.Export.Domain.Entities;
using Framework.DataLayer;

namespace FilingPortal.Parts.Rail.Export.DataLayer.Repositories
{
    /// <summary>
    /// Represents the repository of the <see cref="RuleExporterConsignee" />
    /// </summary>
    internal class RuleExporterConsigneeRepository : SearchRepository<RuleExporterConsignee>, IRuleRepository<RuleExporterConsignee>
    {
        public int GetId(RuleExporterConsignee rule) => default(int);
        /// <summary>
        /// Initializes a new instance of the <see cref="RuleExporterConsignee"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public RuleExporterConsigneeRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Checks whether specified rule is not duplicating any other rule
        /// </summary>
        /// <param name="rule">The Rule to be checked</param>
        public bool IsDuplicate(RuleExporterConsignee rule)
        {
            if (rule == null || string.IsNullOrWhiteSpace(rule.Exporter) || string.IsNullOrWhiteSpace(rule.ConsigneeCode)) return false;
            return Set.Any(x =>
                x.Id != rule.Id && x.Exporter.Trim() == rule.Exporter.Trim() &&
                x.ConsigneeCode == rule.ConsigneeCode);
        }

        /// <summary>
        /// Checks whether default value record with specified id exist
        /// </summary>
        /// <param name="id">The default value record identifier</param>
        public bool IsExist(int id)
        {
            return Set.Any(x => x.Id == id);
        }
    }
}
