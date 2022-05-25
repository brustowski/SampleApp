using FilingPortal.Domain.Common;
using Framework.Domain;

namespace FilingPortal.Domain.Services
{
    /// <summary>
    /// Describes document generator
    /// </summary>
    /// <typeparam name="TModel">The type of the model</typeparam>
    public interface IFileGenerator<in TModel> where TModel : Entity, new()
    {
        /// <summary>
        /// Generates the document for specified model
        /// </summary>
        BinaryFileModel Generate(TModel model);

    }
}
