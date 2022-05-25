using AutoMapper;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories.AppSystem;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.PluginEngine.Models.Fields;
using Newtonsoft.Json;

namespace FilingPortal.PluginEngine.Mapping.Converters
{
    /// <summary>
    /// Defines the <see cref="AddressFieldTypeValueResolver" />
    /// </summary>
    public class AddressFieldTypeValueResolver : IMemberValueResolver<object, object, string, string>
    {
        /// <summary>
        /// The overridden address repository
        /// </summary>
        private readonly IAppAddressesRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldTypeValueResolver"/> class.
        /// </summary>
        /// <param name="repository">The overridden address repository</param>
        public AddressFieldTypeValueResolver(IAppAddressesRepository repository)
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
        public string Resolve(object source, object destination, string sourceMember, string destMember, ResolutionContext context)
        {
            if (!int.TryParse(sourceMember, out var id))
            {
                return string.Empty;
            }

            AppAddress address = _repository.GetUntracked(id);
            AddressFieldEditModel editModel = address.Map<AppAddress, AddressFieldEditModel>();
            var result = JsonConvert.SerializeObject(editModel);
            return result;
        }
    }
}
