using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using FilingPortal.Domain.Entities.Audit.Rail;
using FilingPortal.Domain.Repositories.Audit.Rail;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Repositories.Audit.Rail
{
    /// <summary>
    /// Repository of the <see cref="AuditRailTrainConsistSheet"/> entity
    /// </summary>
    class AuditTrainConsistSheetRepository : SearchRepository<AuditRailTrainConsistSheet>, IAuditTrainConsistSheetRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="AuditTrainConsistSheetRepository"/>
        /// </summary>
        /// <param name="unitOfWork"></param>
        public AuditTrainConsistSheetRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Returns Train consist sheets created by specific user
        /// </summary>
        /// <param name="userAccount">User login</param>
        /// <returns></returns>
        public IEnumerable<AuditRailTrainConsistSheet> GetAll(string userAccount)
        {
            return Set.Where(x => x.CreatedUser == userAccount);
        }

        /// <summary>
        /// Runs stored procedure to verify rail train consist sheet data
        /// </summary>
        /// <param name="userAccount">User who starts verification process</param>
        public void Verify(string userAccount)
        {
            var userAccountParam = new SqlParameter
            {
                DbType = DbType.String,
                Size = 128,
                ParameterName = "@userAccount",
                Value = userAccount,
                Direction = ParameterDirection.Input
            };

            var command = "EXEC dbo.sp_imp_rail_audit_train_consist_sheet_verify @userAccount";

            var context = (FilingPortalContext)UnitOfWork.Context;
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, command, userAccountParam);
        }
    }
}
