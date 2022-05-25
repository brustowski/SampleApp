using System.Threading.Tasks;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;

namespace FilingPortal.Domain.Services.TruckExport
{
    /// <summary>
    /// Describes Truck export auto refile service
    /// </summary>
    public interface ITruckExportAutoRefileService
    {
        /// <summary>
        /// Executes auto-refile and returns report
        /// </summary>
        /// <param name="user">Current user</param>
        Task<string> Execute(AppUsersModel user);
    }
}