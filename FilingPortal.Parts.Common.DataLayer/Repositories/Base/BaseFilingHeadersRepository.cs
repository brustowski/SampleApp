using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Parts.Common.DataLayer.Repositories.Base
{
    public abstract class BaseFilingHeadersRepository<TFilingHeader> : Repository<TFilingHeader>, IFilingHeaderRepository<TFilingHeader>, IFilingSectionRepository
    where TFilingHeader : BaseFilingHeader
    {
        /// <summary>
        /// The name of the create entry record stored procedure
        /// </summary>
        protected abstract string CreateEntryProcedureName { get; }

        /// <summary>
        /// The name of the refile entry record stored procedure
        /// </summary>
        protected virtual string RefileEntryProcedureName => string.Empty;

        /// <summary>
        /// The name of the delete entry record stored procedure
        /// </summary>
        protected abstract string DeleteEntryProcedureName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseFilingHeadersRepository{TDocument}"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        protected BaseFilingHeadersRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }
        
        /// <summary>
        /// Finds the Filing Headers by inbound record identifiers
        /// </summary>
        /// <param name="ids">The inbound record identifiers</param>
        public abstract IEnumerable<TFilingHeader> FindByInboundRecordIds(IEnumerable<int> ids);

        /// <summary>
        /// Fills the data for header by specified filing header identifier using stored procedure
        /// </summary>
        /// <param name="headerId">The filing header identifier</param>
        /// <param name="userAccount">User account, under which credentials process is executed</param>
        public int FillDataForFilingHeader(int headerId, string userAccount = null)
        {
            var filingHeaderIdParam = new SqlParameter
            {
                ParameterName = "@filingHeaderId",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = headerId
            };

            var userAccountParam = new SqlParameter
            {
                ParameterName = "@filingUser",
                SqlDbType = SqlDbType.NVarChar,
                Direction = ParameterDirection.Input,
                Value = userAccount
            };
            if (string.IsNullOrEmpty(userAccount))
            {
                userAccountParam.Value = DBNull.Value;
            }

            var procResultParam = new SqlParameter
            {
                ParameterName = "@procResult",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            var command = $"EXEC @procResult = {UnitOfWork.Context.DefaultSchema}.{CreateEntryProcedureName} @filingHeaderId, @filingUser";

            var context = UnitOfWork.Context;
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, command, filingHeaderIdParam, userAccountParam, procResultParam);

            return (int)procResultParam.Value;
        }

        /// <summary>
        /// Fills the data for header by specified filing header identifier using stored procedure
        /// </summary>
        /// <param name="headerId">The filing header identifier</param>
        /// <param name="userAccount">User account, under which credentials process is executed</param>
        public int RefillDataForFilingHeader(int headerId, string userAccount = null)
        {
            if (string.IsNullOrEmpty(RefileEntryProcedureName)) return default(int);
            var filingHeaderIdParam = new SqlParameter
            {
                ParameterName = "@filingHeaderId",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = headerId
            };

            var userAccountParam = new SqlParameter
            {
                ParameterName = "@filingUser",
                SqlDbType = SqlDbType.NVarChar,
                Direction = ParameterDirection.Input,
                Value = userAccount
            };
            if (string.IsNullOrEmpty(userAccount))
            {
                userAccountParam.Value = DBNull.Value;
            }

            var procResultParam = new SqlParameter
            {
                ParameterName = "@procResult",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            var command =
                $"EXEC @procResult = {UnitOfWork.Context.DefaultSchema}.{RefileEntryProcedureName} @filingHeaderId, @filingUser";

            var context = UnitOfWork.Context;
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, command,
                filingHeaderIdParam, userAccountParam, procResultParam);

            return (int) procResultParam.Value;

        }

        /// <summary>
        /// Call File procedure for the records with specified filing header identifier
        /// </summary>
        /// <param name="headerId">The filing header identifier</param>
        [Obsolete("Due to changes in the database, this method is deprecated and will be removed in a future version.")]
        public virtual void FileRecordsWithHeader(int headerId)
        {
            //todo: remove after finish with other workflow
        }

        /// <summary>
        /// Calls Cancel filing process procedure for the specified filing header
        /// </summary>
        /// <param name="headerId">The filing header identifier</param>
        public void CancelFilingProcess(int headerId)
        {
            var recordIdParam = new SqlParameter
            {
                ParameterName = "@recordId",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = headerId
            };

            var tableNameParam = new SqlParameter
            {
                ParameterName = "@tableName",
                SqlDbType = SqlDbType.VarChar,
                Size = 128,
                Direction = ParameterDirection.Input,
                Value = DBNull.Value
            };

            var command = $"EXEC {UnitOfWork.Context.DefaultSchema}.{DeleteEntryProcedureName} @recordId, @tableName";

            var context = UnitOfWork.Context;
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, command, recordIdParam, tableNameParam);
        }

        /// <summary>
        /// Add new row to the table corresponding to specified section
        /// </summary>
        /// <param name="sectionName">Section name</param>
        /// <param name="filingHeaderId">Filing Header id</param>
        /// <param name="parentId">Parent section id</param>
        /// <param name="userAccount">User account, under which credentials process is executed</param>
        public Guid AddSectionRecord(string sectionName, int filingHeaderId, int parentId, string userAccount = null)
        {
            BaseSection section = GetSection(sectionName);

            if (section == null || string.IsNullOrWhiteSpace(section.ProcedureName))
            {
                throw new ArgumentOutOfRangeException(nameof(sectionName));
            }

            var filingHeaderIdParam = new SqlParameter
            {
                ParameterName = "@filingHeaderId",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = filingHeaderId
            };

            var parentIdParam = new SqlParameter
            {
                ParameterName = "@parentId",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = parentId
            };

            var userAccountParam = new SqlParameter
            {
                ParameterName = "@filingUser",
                SqlDbType = SqlDbType.NVarChar,
                Direction = ParameterDirection.Input,
                Value = userAccount
            };
            if (string.IsNullOrEmpty(userAccount))
            {
                userAccountParam.Value = DBNull.Value;
            }

            var operationId = new SqlParameter
            {
                ParameterName = "@operationId",
                SqlDbType = SqlDbType.UniqueIdentifier,
                Direction = ParameterDirection.Output
            };

            var command = $"EXEC {UnitOfWork.Context.DefaultSchema}.{section.ProcedureName} @filingHeaderId, @parentId, @filingUser, @operationId OUTPUT";

            var context = UnitOfWork.Context;
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, command, filingHeaderIdParam, parentIdParam, userAccountParam, operationId);

            return (Guid)operationId.Value;
        }
        /// <summary>
        /// Gets the section by its name
        /// </summary>
        /// <param name="sectionName">The section name</param>
        protected abstract BaseSection GetSection(string sectionName);

        /// <summary>
        /// Delete record with specified id from the table corresponding to the specified section name
        /// </summary>
        /// <param name="sectionName">Section name</param>
        /// <param name="recordId">Record id</param>
        public void DeleteSectionRecord(string sectionName, int recordId)
        {
            BaseSection section = GetSection(sectionName);

            if (section == null || string.IsNullOrWhiteSpace(section.TableName))
            {
                throw new ArgumentOutOfRangeException(nameof(sectionName));
            }

            var tableNameParam = new SqlParameter
            {
                ParameterName = "@tableName",
                SqlDbType = SqlDbType.VarChar,
                Size = 128,
                Direction = ParameterDirection.Input,
                Value = section.TableName
            };

            var recordIdParam = new SqlParameter
            {
                ParameterName = "@recordId",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = recordId
            };

            var command = $"EXEC {UnitOfWork.Context.DefaultSchema}.{DeleteEntryProcedureName} @recordId, @tableName";
            var context = UnitOfWork.Context;
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, command, recordIdParam, tableNameParam);
        }
    }
}