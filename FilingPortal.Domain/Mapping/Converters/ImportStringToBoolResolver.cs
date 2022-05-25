using AutoMapper;

namespace FilingPortal.Domain.Mapping.Converters
{
    /// <summary>
    /// 
    /// </summary>
    public class ImportStringToBoolResolver : IMemberValueResolver<object, object, string, bool>
    {
        /// <summary>
        /// Implementors use source object to provide a destination object.
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object, if exists</param>
        /// <param name="sourceMember">Source member</param>
        /// <param name="destMember">Destination member</param>
        /// <param name="context">The context of the mapping</param>
        /// <returns>Result, typically build from the source resolution result</returns>
        public bool Resolve(object source, object destination, string sourceMember, bool destMember,
            ResolutionContext context)
        {
            return int.TryParse(sourceMember, out int x) && x > 0;
        }
    }
}