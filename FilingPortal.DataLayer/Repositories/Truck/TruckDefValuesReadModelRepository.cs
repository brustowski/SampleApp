using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Repositories.Truck;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.AgileSettings;

namespace FilingPortal.DataLayer.Repositories.Truck
{
    /// <summary>
    /// Represents the repository of the <see cref="TruckDefValueReadModel"/>
    /// </summary>
    internal class TruckDefValuesReadModelRepository : SearchRepositoryWithTypedId<TruckDefValueReadModel, int>, ITruckDefValuesReadModelRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckDefValuesRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public TruckDefValuesReadModelRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }
        /// <summary>
        /// Get the columns configuration <see cref="AgileField"/> for the grid
        /// </summary>
        public IEnumerable<AgileField> GetFields()
        {
            return Set
                .Where(x => x.SingleFilingOrder.HasValue)
                .OrderBy(x => x.SingleFilingOrder)
                .Select(x => new AgileField
                {
                    ColumnName = x.ColumnName,
                    TableName = x.TableName,
                    DisplayName = x.Label,
                    TypeName = x.ValueType
                }).ToList();
        }
    }
}
