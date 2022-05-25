using FilingPortal.Domain.Enums;
using Framework.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using FilingPortal.Parts.Common.Domain.Enums;

namespace FilingPortal.Domain.Specifications.Clients
{
    /// <summary>
    /// Provides Client Type custom specification for Clients filtering
    /// </summary>
    public class ClientTypeSpecification : SpecificationBase<Client>, ICustomSpecification<Client>
    {
        private ClientType _value;

        private Dictionary<ClientType, Expression<Func<Client, bool>>> functorsDictionary = new Dictionary<ClientType, Expression<Func<Client, bool>>>
            {
                { ClientType.Importer, x => x.Importer == true },
                { ClientType.Supplier, x => x.Supplier == true }
            };

        /// <summary>
        /// Specification  builder for Client type
        /// </summary>
        public override Expression<Func<Client, bool>> GetExpression()
        {
            return functorsDictionary.ContainsKey(_value) 
                ? functorsDictionary[_value] 
                : new DefaultSpecification<Client>().GetExpression();
        }
        /// <summary>
        /// Sets expression values for specification
        /// </summary>
        /// <param name="fieldName">Field name</param>
        /// <param name="values">Values for specification</param>
        public void SetValue(string fieldName, object[] values)
        {
            var filterValue = Convert.ToByte(values[0]);
            _value = (ClientType)filterValue;
        }
    }
}
