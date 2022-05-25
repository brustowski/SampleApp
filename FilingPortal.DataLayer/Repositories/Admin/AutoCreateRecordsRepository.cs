using System.Linq;
using FilingPortal.Domain.Entities.Admin;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Repositories.Admin
{
    /// <summary>
    /// Repository of the <see cref="AutoCreateRecord"/> entity
    /// </summary>
    internal class AutoCreateRecordsRepository : SearchRepositoryWithTypedId<AutoCreateRecord, int>, IRuleRepository<AutoCreateRecord>
    {
        /// <summary>
        /// Creates a new instance of <see cref="AutoCreateRecordsRepository"/>
        /// </summary>
        /// <param name="unitOfWork">The Unit of Work</param>
        public AutoCreateRecordsRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Checks whether specified rule is not duplicating any other rule
        /// </summary>
        /// <param name="rule">The Rule to be checked</param>
        public bool IsDuplicate(AutoCreateRecord rule)
        {
            return GetId(rule) != default(int);
        }

        /// <summary>
        /// Gets the duplicate rule
        /// </summary>
        /// <param name="rule">The rule to get</param>
        public int GetId(AutoCreateRecord rule)
        {
            if (rule == null)
            {
                return default(int);
            }

            return Set.Where(x => x.Id != rule.Id 
                                  && x.AutoCreate == rule.AutoCreate
                                  && x.EntryType == rule.EntryType
                                  && x.TransportMode == rule.TransportMode
                                  && x.ImporterExporter == rule.ImporterExporter
                                  && x.ShipmentType == rule.ShipmentType)
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
