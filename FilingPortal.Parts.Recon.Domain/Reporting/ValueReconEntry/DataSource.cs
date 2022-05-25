using FilingPortal.Domain.Common.Reporting;
using FilingPortal.Parts.Recon.Domain.Config;
using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Domain.Repositories;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilingPortal.Parts.Recon.Domain.Reporting.ValueReconEntry
{
    /// <summary>
    /// Represents the data source for Value Recon Entry report
    /// </summary>
    internal class DataSource : IReportDatasource
    {
        /// <summary>
        /// Repository of <see cref="ValueReconReadModel"/>
        /// </summary>
        private readonly IValueReconReadModelRepository _repository;

        /// <summary>
        /// Gets the name of the data source
        /// </summary>
        public string Name => ReportNames.ValueReconEntryReport;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSource" /> class
        /// </summary>
        /// <param name="repository">Repository of <see cref="ValueReconReadModel"/></param>
        public DataSource(IValueReconReadModelRepository repository)
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
