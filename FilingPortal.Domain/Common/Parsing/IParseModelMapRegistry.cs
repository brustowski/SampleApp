using System;

namespace FilingPortal.Domain.Common.Parsing
{
    /// <summary>
    /// Describes Register of Parsing Models
    /// </summary>
    public interface IParseModelMapRegistry
    {
        /// <summary>
        /// Gets <see cref="IParseModelMap"/> by model type
        /// </summary>
        /// <typeparam name="T">Type of the model</typeparam>
        IParseModelMap Get<T>();
        /// <summary>
        /// Gets <see cref="IParseModelMap"/> by model type
        /// </summary>
        /// <param name="modeType"><see cref="Type"/> of the model</param>
        /// <returns></returns>
        IParseModelMap Get(Type modeType);
    }
}
