using System;
using System.Collections.Generic;

namespace FilingPortal.Domain.Common.Parsing
{
    /// <summary>
    /// Represents the parsing model map registry
    /// </summary>
    internal class ParseModelMapRegistry : IParseModelMapRegistry
    {
        private readonly Dictionary<Type, IParseModelMap> _parseModelMapsDictionary;

        /// <summary>
        /// Initialize a new instance of the <see cref="ParseModelMapRegistry"/> class.
        /// </summary>
        /// <param name="maps">Collection of the model maps</param>
        public ParseModelMapRegistry(IEnumerable<IParseModelMap> maps)
        {
            _parseModelMapsDictionary = new Dictionary<Type, IParseModelMap>();
            foreach (IParseModelMap map in maps)
            {
                _parseModelMapsDictionary.Add(map.GetModelType, map);
            }
        }

        /// <summary>
        /// Gets model map of the specified type
        /// </summary>
        /// <typeparam name="T">The type of the model map</typeparam>
        public IParseModelMap Get<T>()
        {
            return Get(typeof(T));
        }

        /// <summary>
        /// Gets model map of the specified type
        /// </summary>
        /// <param name="modelType">The type of the model map</param>
        public IParseModelMap Get(Type modelType)
        {
            if (!_parseModelMapsDictionary.ContainsKey(modelType))
            {
                throw new KeyNotFoundException(
                    $"Key {modelType} was not found in ParseModelMapRegistry. Available keys:{string.Join(",", _parseModelMapsDictionary.Keys)}");
            }

            return _parseModelMapsDictionary[modelType];
        }
    }
}
