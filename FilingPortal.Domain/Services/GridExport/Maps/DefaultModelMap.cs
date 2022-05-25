using FilingPortal.Domain.Services.GridExport.Configuration;
using Framework.Domain;

namespace FilingPortal.Domain.Services.GridExport.Maps
{
    /// <summary>
    /// Class describing default report model mapping
    /// </summary>
    internal class DefaultModelMap<T> : ReportModelMap<T>, IReportModelMap where T: Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultModelMap{T}"/> class.
        /// </summary>
        public DefaultModelMap()
        {
            Ignore(x => x.Id);
        }
    }
}