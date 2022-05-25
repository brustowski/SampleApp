using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Domain.Repositories;
using Framework.DataLayer;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.DataLayer.Repositories.VesselExport
{
    /// <summary>
    /// Represents the repository of the <see cref="VesselExportRuleUsppiConsignee" />
    /// </summary>
    internal class VesselExportRuleUsppiConsigneeRepository : SearchRepositoryWithTypedId<VesselExportRuleUsppiConsignee, int>, IRuleRepository<VesselExportRuleUsppiConsignee>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportRuleUsppiConsignee"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public VesselExportRuleUsppiConsigneeRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Checks whether specified rule is not duplicating any other rule
        /// </summary>
        /// <param name="rule">The Rule to be checked</param>
        public bool IsDuplicate(VesselExportRuleUsppiConsignee rule)
        {
            return GetId(rule) != default(int);
        }

        /// <summary>
        /// Gets the duplicate rule
        /// </summary>
        /// <param name="rule">The rule to get</param>
        public int GetId(VesselExportRuleUsppiConsignee rule)
        {
            if (rule == null)
            {
                return default(int);
            }

            return Set.Where(x =>
                    x.Id != rule.Id && x.UsppiId == rule.UsppiId &&
                    x.ConsigneeId == rule.ConsigneeId)
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
