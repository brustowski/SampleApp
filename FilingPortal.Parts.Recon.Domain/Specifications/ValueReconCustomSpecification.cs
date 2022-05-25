using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Domain.Enums;
using Framework.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FilingPortal.Parts.Recon.Domain.Specifications
{
    /// <summary>
    /// Provides custom specification for Recon filtering
    /// </summary>
    public class ValueReconCustomSpecification : SpecificationBase<ValueReconReadModel>, ICustomSpecification<ValueReconReadModel>
    {
        private object _value;
        private string _fieldName;

        /// <summary>
        /// The PSA Reason filtering expression dictionary
        /// </summary>
        private readonly Dictionary<PsaReason, Expression<Func<ValueReconReadModel, bool>>> _psaReasonDictionary = new Dictionary<PsaReason, Expression<Func<ValueReconReadModel, bool>>>
        {
            { PsaReason.NotFlagged, x => x.PsaReason == null || x.PsaReason == string.Empty },
            { PsaReason.Flagged | PsaReason.Filed, x => x.PsaReason != null && x.PsaReason != string.Empty && x.PsaFiledDate != null},
            { PsaReason.Flagged | PsaReason.NotFiled, x => x.PsaReason != null && x.PsaReason != string.Empty && x.PsaFiledDate == null},
            { PsaReason.Flagged | PsaReason.Filed | PsaReason.NotFiled, x => x.PsaReason != null && x.PsaReason != string.Empty},
            { PsaReason.NotFlagged | PsaReason.Flagged | PsaReason.Filed, x =>
                (x.PsaReason == null || x.PsaReason == string.Empty) || (x.PsaReason != null && x.PsaReason != string.Empty && x.PsaFiledDate != null)},
            { PsaReason.NotFlagged | PsaReason.Flagged | PsaReason.NotFiled, x =>
            (x.PsaReason == null || x.PsaReason == string.Empty) || (x.PsaReason != null && x.PsaReason != string.Empty && x.PsaFiledDate == null)},
        };

        /// <summary>
        /// The PSA Reason 520d filtering expression dictionary
        /// </summary>
        private readonly Dictionary<PsaReason, Expression<Func<ValueReconReadModel, bool>>> _psaReason520dDictionary = new Dictionary<PsaReason, Expression<Func<ValueReconReadModel, bool>>>
        {
            { PsaReason.NotFlagged, x => x.PsaReason520d == null || x.PsaReason520d == string.Empty },
            { PsaReason.Flagged | PsaReason.Filed, x => x.PsaReason520d != null && x.PsaReason520d != string.Empty && x.PsaFiledDate520d != null},
            { PsaReason.Flagged | PsaReason.NotFiled, x => x.PsaReason520d != null && x.PsaReason520d != string.Empty && x.PsaFiledDate520d == null},
            { PsaReason.Flagged | PsaReason.Filed | PsaReason.NotFiled, x => x.PsaReason520d != null && x.PsaReason520d != string.Empty},
            { PsaReason.NotFlagged | PsaReason.Flagged | PsaReason.Filed, x =>
                (x.PsaReason520d == null || x.PsaReason520d == string.Empty) || (x.PsaReason520d != null && x.PsaReason520d != string.Empty && x.PsaFiledDate520d != null)},
            { PsaReason.NotFlagged | PsaReason.Flagged | PsaReason.NotFiled, x =>
                (x.PsaReason520d == null || x.PsaReason520d == string.Empty) || (x.PsaReason520d != null && x.PsaReason520d != string.Empty && x.PsaFiledDate520d == null)},
        };

        /// <summary>
        /// The NAFTA filtering expression dictionary
        /// </summary>
        private readonly Dictionary<string, Expression<Func<ValueReconReadModel, bool>>> _naftaDictionary = new Dictionary<string, Expression<Func<ValueReconReadModel, bool>>>
        {
            { string.Empty, x => x.NaftaRecon == null || x.NaftaRecon == string.Empty },
            { "Y", x => x.NaftaRecon == "Y" },
        };

        /// <summary>
        /// Specification  builder for Client type
        /// </summary>
        public override Expression<Func<ValueReconReadModel, bool>> GetExpression()
        {

            if (_fieldName == PropertyExpressionHelper.GetPropertyName<ValueReconReadModel>(x => x.PsaReason)
                && _value is PsaReason psaReason && _psaReasonDictionary.ContainsKey(psaReason))
            {
                return _psaReasonDictionary[psaReason];
            }

            if (_fieldName == PropertyExpressionHelper.GetPropertyName<ValueReconReadModel>(x => x.PsaReason520d)
                && _value is PsaReason psaReason520d && _psaReason520dDictionary.ContainsKey(psaReason520d))
            {
                return _psaReason520dDictionary[psaReason520d];
            }

            if (_fieldName == PropertyExpressionHelper.GetPropertyName<ValueReconReadModel>(x => x.NaftaRecon) &&
                _value is string st && _naftaDictionary.ContainsKey(st))
            {
                return _naftaDictionary[st];
            }

            return new DefaultSpecification<ValueReconReadModel>().GetExpression();
        }
        /// <summary>
        /// Sets expression values for specification
        /// </summary>
        /// <param name="fieldName">Field name</param>
        /// <param name="values">Values for specification</param>
        public void SetValue(string fieldName, object[] values)
        {
            _fieldName = fieldName;

            if (_fieldName == PropertyExpressionHelper.GetPropertyName<ValueReconReadModel>(x => x.PsaReason) ||
                _fieldName == PropertyExpressionHelper.GetPropertyName<ValueReconReadModel>(x => x.PsaReason520d))
            {
                _value = values.Select(Convert.ToInt32).Aggregate(PsaReason.None, (current, value) => current | (PsaReason)value);
            }
        }
    }
}
