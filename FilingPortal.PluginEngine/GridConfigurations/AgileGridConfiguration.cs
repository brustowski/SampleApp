using System.Collections.Generic;
using AutoMapper;
using FilingPortal.Parts.Common.Domain.AgileSettings;
using FilingPortal.PluginEngine.GridConfigurations.Columns;

namespace FilingPortal.PluginEngine.GridConfigurations
{
    /// <summary>
    /// Class for grid configuration
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AgileGridConfiguration<T> : GridConfiguration<T>
        where T: class
    {
        /// <summary>
        /// Defines the repository where AgileConfiguration is implemented
        /// </summary>
        private readonly IAgileConfiguration<T> _repository;

        /// <summary>
        /// Creates <see cref="AgileGridConfiguration{T}"/> class.
        /// </summary>
        /// <param name="repository">The repository where grid configuration is stored<see cref="IAgileConfiguration{T}"/></param>
        protected AgileGridConfiguration(IAgileConfiguration<T> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Executes requests to prepare column configuration
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumnRange(Mapper.Map<IEnumerable<ColumnConfig>>(_repository.GetFields()));
        }

        /// <summary>
        /// Executes requests to prepare filter configuration
        /// </summary>
        protected override void ConfigureFilters() { }

        /// <summary>
        /// Returns all columns configuration
        /// </summary>
        public override IEnumerable<ColumnConfig> GetColumns()
        {
            Configure();
            return base.GetColumns();
        }
    }
}
