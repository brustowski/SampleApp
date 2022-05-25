using AutoMapper;
using FilingPortal.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.PluginEngine.Models.Fields;
using Newtonsoft.Json;

namespace FilingPortal.Web.Mapping.Converters
{
    /// <summary>
    /// Converter from string to Address
    /// </summary>
    public class StringToAddressConverter : ITypeConverter<string, AppAddress>
    {
        /// <summary>
        /// Performs conversion from source to destination type
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        public AppAddress Convert(string source, AppAddress destination, ResolutionContext context)
        {
            try
            {
                var address = JsonConvert.DeserializeObject<AddressFieldEditModel>(source);
                return address.Map<AddressFieldEditModel, AppAddress>();
            }
            catch
            {
                return null;
            }
        }
    }
}