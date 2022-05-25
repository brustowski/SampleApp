using FilingPortal.Domain.Entities.Pipeline;

namespace FilingPortal.Domain.Repositories.Pipeline
{
    /// <summary>
    /// Describes Pipeline facility rules repository
    /// </summary>
    public interface IPipelineRuleFacilityRepository
    {
        /// <summary>
        /// Gets Facility rule by name
        /// </summary>
        /// <param name="name">Facility name</param>
        PipelineRuleFacility GetByFacilityName(string name);
    }
}
