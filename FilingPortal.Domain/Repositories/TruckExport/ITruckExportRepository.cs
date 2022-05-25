using FilingPortal.Domain.DTOs.TruckExport;
using FilingPortal.Domain.Entities.TruckExport;
using Framework.Domain.Paging;
using System.Collections.Generic;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.TruckExport
{

    /// <summary>
    /// Describes the repository of the Truck Export
    /// </summary>
    public interface ITruckExportRepository : IInboundRecordsRepository<TruckExportRecord>
    {
        /// <summary>
        /// Gets all records with AutoRefile set to true
        /// </summary>
        IEnumerable<TruckExportRecord> GetAutoRefileRecords();

        /// <summary>
        /// Gets the matched entity form the repository for the provided entity or null if entity not found
        /// </summary>
        /// <param name="entity">The entity to search</param>
        TruckExportRecord GetMatchedEntity(TruckExportImportModel entity);

        /// <summary>
        /// Gets the collection of the users specified by search request
        /// </summary>
        /// <param name="searchRequest">The search request</param>

        IList<string> GetUsers(SearchRequest searchRequest);

        /// <summary>
        /// Runs Database validation of truck export records
        /// </summary>
        /// <param name="records">Truck Export records</param>
        void Validate(IList<TruckExportRecord> records);

        /// <summary>
        /// Runs Database validation of truck export records
        /// </summary>
        /// <param name="recordIds">Truck Export records ids</param>
        void Validate(IEnumerable<int> recordIds);

        /// <summary>
        /// Try to get the job number for specified record
        /// </summary>
        /// <param name="record">Inbound record</param>
        string TryGetJobNumber(TruckExportRecord record);
    }
}
