using FilingPortal.Domain.Common.Reporting;
using FilingPortal.Parts.Recon.Domain.Config;
using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Domain.Repositories;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilingPortal.Parts.Recon.Domain.Reporting.ReconEntryAssociation
{
    /// <summary>
    /// Represents the data source for Association Recon Entry report
    /// </summary>
    internal class DataSource : IReportDatasource
    {
        /// <summary>
        /// Repository of <see cref="FtaReconReadModel"/>
        /// </summary>
        private readonly IFtaReconReadModelRepository _repository;

        /// <summary>
        /// Gets the name of the data source
        /// </summary>
        public string Name => ReportNames.AssociationReconEntryReport;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSource" /> class
        /// </summary>
        /// <param name="repository">Repository of <see cref="FtaReconReadModel"/></param>
        public DataSource(IFtaReconReadModelRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets all available records as <see cref="SimplePagedResult{TEntityDto}"/> specified by <see cref="SearchRequest"/> model
        /// </summary>
        /// <typeparam name="TEntity">Type of the records</typeparam>
        /// <param name="request">The <see cref="SearchRequest"/> object</param>
        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(SearchRequest request) where TEntity : class, new()
            => await _repository.GetProcessedAsync<TEntity>(request);


        /// <summary>
        /// Gets total matches of records specified by <see cref="SearchRequest"/> model
        /// </summary>
        /// <typeparam name="TEntity">Type of the records</typeparam>
        /// <param name="request">The <see cref="SearchRequest"/> object</param>
        public async Task<int> GetTotalMatches<TEntity>(SearchRequest request) where TEntity : class
            => await _repository.GetTotalProcessedAsync<TEntity>(request);
    }
}
