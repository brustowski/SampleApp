using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters;
using FilingPortal.Web.FieldConfigurations.InboundRecordParameters;
using Reinforced.Typings.Attributes;

namespace FilingPortal.PluginEngine.FieldConfigurations.Common
{
    /// <summary>
    /// Class representing Inbound Record field description
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false)]
    public class ComplexInboundRecordField : BaseInboundRecordField
    {
        /// <summary>
        /// Defines the _fields
        /// </summary>
        private readonly IEnumerable<InboundRecordField> _fields;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexInboundRecordField"/> class.
        /// </summary>
        /// <param name="main">The main field to render</param>
        /// <param name="additional">The additional<see cref="InboundRecordField"/></param>
        public ComplexInboundRecordField(InboundRecordField main, InboundRecordField additional)
        {
            if (main == null) throw new ArgumentException(@"Inbound record can't be null", nameof(main));
            if (additional == null) throw new ArgumentException(@"Inbound record can't be null", nameof(additional));
            _fields = new List<InboundRecordField> {main, additional};
            Title = main.Title;
            Type = UIValueTypes.Complex;
            MarkedForFiltering = _fields.Any(x => x.ConfirmationNeeded || x.IsMandatory || x.MarkedForFiltering);
        }

        /// <summary>
        /// Gets the Fields. Will be mapped to UI by reinforced typings
        /// </summary>
        public List<InboundRecordField> Fields => _fields.ToList();
    }
}
