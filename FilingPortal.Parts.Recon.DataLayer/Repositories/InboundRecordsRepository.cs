using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Domain.Models;
using FilingPortal.Parts.Recon.Domain.Repositories;
using Framework.DataLayer;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace FilingPortal.Parts.Recon.DataLayer.Repositories
{
    /// <summary>
    /// Provides the repository of <see cref="InboundRecord"/>
    /// </summary>
    public class InboundRecordsRepository : SearchRepository<InboundRecord>, IInboundRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordsRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public InboundRecordsRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Prepare report from CargoWise using user-defined filters
        /// </summary>
        /// <param name="filter">Filters set for report</param>
        public int LoadFromCargoWise(ReconFilter filter)
        {

            var reconIssueParameter = new SqlParameter
            {
                ParameterName = "@ReconIssue",
                SqlDbType = SqlDbType.Text,
                Direction = ParameterDirection.Input,
                Value = filter.ReconIssue != null && filter.ReconIssue.Any() ? string.Join(",", filter.ReconIssue.Select(x => $"'{x}'")) : (object)DBNull.Value
            };
            var naftaReconParameter = new SqlParameter
            {
                ParameterName = "@NAFTARecon",
                SqlDbType = SqlDbType.Text,
                Direction = ParameterDirection.Input,
                Value = filter.NaftaRecon ?? (object)DBNull.Value
            };
            var ftaReconFilingParameter = new SqlParameter
            {
                ParameterName = "@FTAReconFiling",
                SqlDbType = SqlDbType.Text,
                Direction = ParameterDirection.Input,
                Value = filter.FtaReconFiling != null && filter.FtaReconFiling.Any() ? string.Join(",", filter.FtaReconFiling.Select(x => $"'{x}'")) : (object)DBNull.Value
            };
            var importDateFromParameter = new SqlParameter
            {
                ParameterName = "@ImportDateFrom",
                SqlDbType = SqlDbType.DateTime,
                Direction = ParameterDirection.Input,
                Value = filter.ImportFrom ?? (object)DBNull.Value
            };
            var importDateToParameter = new SqlParameter
            {
                ParameterName = "@ImportDateTo",
                SqlDbType = SqlDbType.DateTime,
                Direction = ParameterDirection.Input,
                Value = filter.ImportTo ?? (object)DBNull.Value
            };
            var importerParameter = new SqlParameter
            {
                ParameterName = "@Importer",
                SqlDbType = SqlDbType.Text,
                Direction = ParameterDirection.Input,
                Value = filter.Importer ?? (object)DBNull.Value
            };
            var entryNumberParameter = new SqlParameter
            {
                ParameterName = "@EntryNumber",
                SqlDbType = SqlDbType.Text,
                Direction = ParameterDirection.Input,
                Value = filter.EntryNumber ?? (object)DBNull.Value
            };
            var preliminaryStatementDateFromParameter = new SqlParameter
            {
                ParameterName = "@PreliminaryStatementDateFrom",
                SqlDbType = SqlDbType.DateTime,
                Direction = ParameterDirection.Input,
                Value = filter.PreliminaryStatementDateFrom ?? (object)DBNull.Value
            };
            var preliminaryStatementDateToParameter = new SqlParameter
            {
                ParameterName = "@PreliminaryStatementDateTo",
                SqlDbType = SqlDbType.DateTime,
                Direction = ParameterDirection.Input,
                Value = filter.PreliminaryStatementDateTo ?? (object)DBNull.Value
            };
            var reconFlaggedFiledParameter = new SqlParameter
            {
                ParameterName = "@ReconFlaggedFiled",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = filter.ReconFlaggedFiled?.Sum() ?? (object)DBNull.Value
            };

            var procResultParam = new SqlParameter
            {
                ParameterName = "@ProcResult",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            var command = $"EXEC @ProcResult = {UnitOfWork.Context.DefaultSchema}.sp_get_cw_data @ReconIssue, @NAFTARecon, @FTAReconFiling, @ImportDateFrom, @ImportDateTo, @Importer, @EntryNumber, @PreliminaryStatementDateFrom, @PreliminaryStatementDateTo, @ReconFlaggedFiled";

            DbExtendedContext context = UnitOfWork.Context;
            context.RunLongDatabaseOperation(() =>
            {
                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, command
                    , reconIssueParameter
                    , naftaReconParameter
                    , ftaReconFilingParameter
                    , importDateFromParameter
                    , importDateToParameter
                    , importerParameter
                    , entryNumberParameter
                    , preliminaryStatementDateFromParameter
                    , preliminaryStatementDateToParameter
                    , reconFlaggedFiledParameter
                    , procResultParam);
            });

            return (int)procResultParam.Value;
        }

        /// <summary>
        /// Gets entity by filer, entry number and line number
        /// </summary>
        /// <param name="filer">The filer</param>
        /// <param name="entryNo">The entry number</param>
        /// <param name="lineNo">The line number</param>
        public InboundRecord Get(string filer, string entryNo, string lineNo)
        {
            return Set.FirstOrDefault(x => x.Filer == filer && x.EntryNo == entryNo && x.LineNumber7501 == lineNo);
        }
    }
}
