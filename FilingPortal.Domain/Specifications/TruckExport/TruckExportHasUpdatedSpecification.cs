using System;
using System.Linq.Expressions;
using FilingPortal.Domain.Entities.TruckExport;
using Framework.Domain.Specifications;

namespace FilingPortal.Domain.Specifications.TruckExport
{
    /// <summary>
    /// Provides Truck Export custom specification
    /// </summary>
    internal class TruckExportHasUpdatedSpecification : SpecificationBase<TruckExportReadModel>, ICustomSpecification<TruckExportReadModel>
    {
        private bool _value;

        /// <summary>
        /// Specification builder for uploaded date
        /// </summary>
        public override Expression<Func<TruckExportReadModel, bool>> GetExpression()
        {
            return new DefaultSpecification<TruckExportReadModel>().GetExpression();
            //return _value
            //    ? model => model.UploadedDate.HasValue
            //    : new DefaultSpecification<TruckExportReadModel>().GetExpression();
        }
        /// <summary>
        /// Sets expression values for specification
        /// </summary>
        /// <param name="fieldName">Field name</param>
        /// <param name="values">Values for specification</param>
        public void SetValue(string fieldName, object[] values)
        {
            _value = Convert.ToBoolean(values[0]);
        }
    }
}
