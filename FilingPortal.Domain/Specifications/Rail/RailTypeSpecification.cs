using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Enums;
using Framework.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FilingPortal.Domain.Specifications.Rail
{
    /// <summary>
    /// Provides Rail Status custom specification for Rail Inbound records filtering
    /// </summary>
    class RailTypeSpecification : SpecificationBase<RailInboundReadModel>, ICustomSpecification<RailInboundReadModel>
    {
        private RailInboundRecordStatus _value;
        private static DateTime ArchDate => DateTime.Now.AddDays(-60).Date;

        private Dictionary<RailInboundRecordStatus, Expression<Func<RailInboundReadModel, bool>>> functorsDictionary = new Dictionary<RailInboundRecordStatus, Expression<Func<RailInboundReadModel, bool>>>
            {
                { RailInboundRecordStatus.Open, x => x.IsDeleted == false && x.IsDuplicated == false && x.CreatedDate >= ArchDate },
                { RailInboundRecordStatus.Duplicates, x => x.IsDuplicated == true },
                { RailInboundRecordStatus.Deleted, x => x.IsDeleted == true },
                { RailInboundRecordStatus.Archived, x => x.CreatedDate < ArchDate}
            };

        /// <summary>
        /// Specification builder for Rail Inbound record Status 
        /// </summary>
        public override Expression<Func<RailInboundReadModel, bool>> GetExpression()
        {
            return functorsDictionary.ContainsKey(_value) 
                ? functorsDictionary[_value] 
                : new DefaultSpecification<RailInboundReadModel>().GetExpression();
        }
        /// <summary>
        /// Sets expression values for specification
        /// </summary>
        /// <param name="fieldName">Field name</param>
        /// <param name="values">Values for specification</param>
        public void SetValue(string fieldName, object[] values)
        {
            var filterValue = Convert.ToByte(values[0]);
            _value = (RailInboundRecordStatus)filterValue;
        }
    }
}
