using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Domain.Repositories.VesselExport;
using Framework.DataLayer;
using Framework.Domain.Repositories;
using Framework.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace FilingPortal.DataLayer.Repositories.VesselExport
{
    /// <summary>
    /// Defines repository for the Vessel Export Read Model
    /// </summary>
    public class VesselExportReadModelRepository : SearchRepository<VesselExportReadModel>, IVesselExportReadModelRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportReadModelRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public VesselExportReadModelRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Deletes the record with specified identifier
        /// </summary>
        /// <param name="id">The record identifier</param>
        public override void DeleteById(int id)
        {
            SetDeleteStatus(id, true);
        }

        /// <summary>
        /// Calls procedure for soft-delete record with the specified identifier by setting Deleted flag
        /// </summary>
        /// <param name="id">The record identifier</param>
        /// <param name="isDeleted">Deletion flag</param>
        private int SetDeleteStatus(int id, bool isDeleted)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "@id",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Input,
                Value = id
            };

            var isDeletedParam = new SqlParameter
            {
                ParameterName = "@deleted",
                SqlDbType = System.Data.SqlDbType.Bit,
                Direction = System.Data.ParameterDirection.Input,
                Value = isDeleted
            };
            const string command = "EXEC dbo.sp_exp_vessel_delete_inbound @id, @deleted";
            var context = (FilingPortalContext)UnitOfWork.Context;
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, command, idParam,
                isDeletedParam);

            return (int)idParam.Value;
        }

        /// <summary>
        /// Mass mark as delete action
        /// </summary>
        /// <param name="ids">Ids of records to mark as delete</param>
        public override void Delete(IEnumerable<int> ids) => ids.ForEach(DeleteById);

        /// <summary>
        /// Gets Vessel Export records with specified by list of the Filing Header identifiers
        /// </summary>
        /// <param name="filingHeaderIds">List of the Filing Header identifiers</param>
        public IEnumerable<VesselExportReadModel> GetByFilingHeaderIds(IEnumerable<int> filingHeaderIds)
        {
            List<VesselExportReadModel> records = GetSet<VesselExportFilingHeader>().Where(x => filingHeaderIds.Contains(x.Id))
                .ToList()
                .SelectMany(x => x.VesselExports.Select(y => new VesselExportReadModel
                {
                    Id = y.Id,
                    FilingHeaderId = x.Id,
                    MappingStatus = x.MappingStatus,
                    FilingStatus = x.FilingStatus,
                    Usppi = y.Usppi.ClientCode,
                    Importer = y.Importer.ClientCode,
                    TariffType = y.TariffType,
                    Tariff = y.Tariff,
                    SoldEnRoute = y.SoldEnRoute,
                    OriginIndicator = y.OriginIndicator,
                    ExportDate = y.ExportDate,
                    GoodsDescription = y.GoodsDescription,
                    CreatedDate = y.CreatedDate,
                })).ToList();
            return records;
        }

        /// <summary>
        /// Gets the total rows by Filing Header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public TableInfo GetTotalRowsByFilingHeaderId(int filingHeaderId) => 
            new TableInfo { RowCount = Set.Count(x => x.FilingHeaderId == filingHeaderId) };
    }
}