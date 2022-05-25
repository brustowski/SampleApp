using AutoMapper;
using FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters;
using FilingPortal.Web.FieldConfigurations.InboundRecordParameters;

namespace FilingPortal.PluginEngine.Mapping.Converters
{
    /// <summary>
    /// Defines the <see cref="FieldTypeValueResolver" />
    /// </summary>
    public class FieldTypeValueResolver : IMemberValueResolver<object, object, string, string>
    {
        /// <summary>
        /// The value type converter
        /// </summary>
        private readonly IValueTypeConverter _valueTypeConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldTypeValueResolver"/> class.
        /// </summary>
        public FieldTypeValueResolver()
        {
            _valueTypeConverter = new ValueTypeConverter();
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
            return _valueTypeConverter.Convert(sourceMember);
        }
    }
}
