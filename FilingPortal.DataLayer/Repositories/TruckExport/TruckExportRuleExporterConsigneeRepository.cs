using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Repositories;
using Framework.DataLayer;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.DataLayer.Repositories.TruckExport
{
    /// <summary>
    /// Represents the repository of the <see cref="TruckExportRuleExporterConsignee" />
    /// </summary>
    internal class TruckExportRuleExporterConsigneeRepository : SearchRepositoryWithTypedId<TruckExportRuleExporterConsignee, int>, IRuleRepository<TruckExportRuleExporterConsignee>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportRuleExporterConsignee"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public TruckExportRuleExporterConsigneeRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Checks whether specified rule is not duplicating any other rule
        /// </summary>
        /// <param name="rule">The Rule to be checked</param>
        public bool IsDuplicate(TruckExportRuleExporterConsignee rule)
        {
            return GetId(rule) != default(int);
        }

        /// <summary>
        /// Gets the duplicate rule
        /// </summary>
        /// <param name="rule">The rule to get</param>
        public int GetId(TruckExportRuleExporterConsignee rule)
        {
            if (rule == null || string.IsNullOrWhiteSpace(rule.Exporter) || string.IsNullOrWhiteSpace(rule.ConsigneeCode))
            {
                return default(int);
            }

            return Set.Where(x =>
                x.Id != rule.Id && x.Exporter.Trim() == rule.Exporter.Trim() &&
                x.ConsigneeCode.Trim() == rule.ConsigneeCode.Trim())
                .Select(x => x.Id)
                .FirstOrDefault();
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
