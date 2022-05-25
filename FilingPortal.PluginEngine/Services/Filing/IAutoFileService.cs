using System.Threading.Tasks;
using FilingPortal.Parts.Common.Domain.Common.InboundTypes;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;

namespace FilingPortal.PluginEngine.Services.Filing
{
    /// <summary>
    /// Describes autofile service for autofile entities
    /// </summary>
    /// <typeparam name="TInboundType">Auto-file entity</typeparam>
    public interface IAutoFileService<TInboundType> where TInboundType : IAutoFilingEntity
    {
        /// <summary>
        /// Executes auto-refile and returns report
        /// </summary>
        /// <param name="user">Current user</param>
        Task<string> Execute(AppUsersModel user);
    }
}
