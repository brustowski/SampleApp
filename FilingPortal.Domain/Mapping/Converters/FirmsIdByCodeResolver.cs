using AutoMapper;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Cargowise.Domain.Repositories;

namespace FilingPortal.Domain.Mapping.Converters
{
    /// <summary>
    /// Resolves FIRMs Code id by its code for specified member on mapping between models
    /// </summary>
    public class FirmsIdByCodeResolver : IMemberValueResolver<object, object, string, int>
    {
        /// <summary>
        /// Clients repository
        /// </summary>
        private readonly IFirmsCodesRepository _repository;

        /// <summary>
        /// Creates a new instance of <see cref="FirmsIdByCodeResolver"/> class.
        /// </summary>
        /// <param name="repository">Clients repository</param>
        public FirmsIdByCodeResolver(IFirmsCodesRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Implementors use source object to provide a destination object.
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object, if exists</param>
        /// <param name="sourceMember">Source member</param>
        /// <param name="destMember">Destination member</param>
        /// <param name="context">The context of the mapping</param>
        public int Resolve(object source, object destination, string sourceMember, int destMember, ResolutionContext context)
        {
            CargowiseFirmsCodes entity = _repository.GetByCode(sourceMember);
            return entity != null ? entity.Id : default(int);
        }
    }
}
