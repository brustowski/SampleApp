using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using FilingPortal.Domain.Entities.Audit.Rail;
using FilingPortal.Domain.Repositories.Audit.Rail;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Repositories.Audit.Rail
{
    /// <summary>
    /// Repository of the <see cref="AuditRailTrainConsistSheet"/> entity
    /// </summary>
    class RailDailyAuditRepository : SearchRepository<AuditRailDaily>, IRailDailyAuditRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="RailDailyAuditRepository"/>
        /// </summary>
        /// <param name="unitOfWork"></param>
        public RailDailyAuditRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Loads Audit data from CargoWise
        /// </summary>
        /// <param name="dtFrom"></param>
        /// <param name="dtTo"></param>
        public int LoadFromCargoWise(DateTime dtFrom, DateTime dtTo)
        {
            var dateFromParameter = new SqlParameter
            {
                ParameterName = "@dtFrom",
                SqlDbType = SqlDbType.SmallDateTime,
                Direction = ParameterDirection.Input,
                Value = dtFrom
            };
            var dateToParameter = new SqlParameter
            {
                ParameterName = "@dtTo",
                SqlDbType = SqlDbType.SmallDateTime,
                Direction = ParameterDirection.Input,
                Value = dtTo
            };

            var procResultParam = new SqlParameter
            {
                ParameterName = "@procResult",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            var command = $"EXEC @procResult = {UnitOfWork.Context.DefaultSchema}.sp_cw_imp_rail_get_daily_audit @dtFrom, @dtTo";

            DbExtendedContext context = UnitOfWork.Context;
            context.RunLongDatabaseOperation(() =>
            {
                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, command, dateFromParameter, dateToParameter, procResultParam);
            });

            return (int)procResultParam.Value;
        }
    }
}
