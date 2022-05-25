using FilingPortal.Cargowise.Domain.Repositories;
using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Domain.Enums;
using FilingPortal.PluginEngine.Models;
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
    public class InboundRecordSpecification : SpecificationBase<InboundRecordReadModel>, ICustomSpecification<InboundRecordReadModel>
    {
        private object _value;
        private string _fieldName;

        /// <summary>
        /// The Puerto Rico port list
        /// </summary>
        private static string[] _prPorts;

        /// <summary>
        /// The Error type filtering expression dictionary
        /// </summary>
        private readonly Dictionary<ErrorType, Expression<Func<InboundRecordReadModel, bool>>> _errorTypesDictionary = new Dictionary<ErrorType, Expression<Func<InboundRecordReadModel, bool>>>
            {
                {
                    ErrorType.All, x => x.MismatchReconValueFlag || x.MismatchReconValueFlag || x.MismatchReconFtaFlag || x.MismatchEntryValue || x.MismatchDuty
                     || x.MismatchMpf || x.MismatchPayableMpf || x.MismatchQuantity || x.MismatchHts
                },
                { ErrorType.ReconValueFlag, x => x.MismatchReconValueFlag},
                { ErrorType.ReconFtaFlag, x => x.MismatchReconFtaFlag},
                { ErrorType.EntryValue, x => x.MismatchEntryValue},
                { ErrorType.Duty, x => x.MismatchDuty},
                { ErrorType.Mpf, x => x.MismatchMpf },
                { ErrorType.PayableMpf, x => x.MismatchPayableMpf },
                { ErrorType.Quantity, x => x.MismatchQuantity},
                { ErrorType.Hts, x => x.MismatchHts},
            };
        /// <summary>
        /// The NAFTA filtering expression dictionary
        /// </summary>
        private readonly Dictionary<string, Expression<Func<InboundRecordReadModel, bool>>> _naftaDictionary = new Dictionary<string, Expression<Func<InboundRecordReadModel, bool>>>
        {
            { string.Empty, x => x.NaftaRecon == null || x.NaftaRecon == string.Empty },
            { "Y", x => x.NaftaRecon == "Y" },
        };
        /// <summary>
        /// The PSA Reason filtering expression dictionary
        /// </summary>
        private readonly Dictionary<PsaReason, Expression<Func<InboundRecordReadModel, bool>>> _psaReasonDictionary = new Dictionary<PsaReason, Expression<Func<InboundRecordReadModel, bool>>>
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
        private readonly Dictionary<PsaReason, Expression<Func<InboundRecordReadModel, bool>>> _psaReason520dDictionary = new Dictionary<PsaReason, Expression<Func<InboundRecordReadModel, bool>>>
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
        /// The Job number filtering expression dictionary
        /// </summary>
        private readonly Dictionary<JobNumberFilterValues, Expression<Func<InboundRecordReadModel, bool>>> _jobNumberDictionary = new Dictionary<JobNumberFilterValues, Expression<Func<InboundRecordReadModel, bool>>>
        {
            { JobNumberFilterValues.NotFlagged, x => x.FtaReconFlaggedNotFiled == null && x.ValueReconFlaggedNotFiled == null},
            { JobNumberFilterValues.ValueFlaggedNotFiled , x => x.ValueReconFlaggedNotFiled == "Y" },
            { JobNumberFilterValues.ValueFlaggedFiled , x => x.ValueReconFlaggedNotFiled == "N" },
            { JobNumberFilterValues.FtaFlaggedNotFiled , x => x.FtaReconFlaggedNotFiled == "Y" },
            { JobNumberFilterValues.FtaFlaggedFiled , x => x.FtaReconFlaggedNotFiled == "N" }
        };
        /// <summary>
        /// The Port of Entry filtering expression dictionary
        /// </summary>
        private readonly Dictionary<PortOfEntryFilterValues, Expression<Func<InboundRecordReadModel, bool>>> _portOfEntryDictionary = new Dictionary<PortOfEntryFilterValues, Expression<Func<InboundRecordReadModel, bool>>>
        {
            { PortOfEntryFilterValues.PuertoRico, x => _prPorts.Any(y=> y == x.EntryPort) },
            { PortOfEntryFilterValues.MainlandUS, x => _prPorts.All(y=> y != x.EntryPort) },
        };

        public InboundRecordSpecification(IForeignPortsRepository repository)
        {

            _prPorts = repository.GetCountryPorts("PR").Select(x => x.PortCode).ToArray();
        }

        /// <summary>
        /// Specification  builder for Client type
        /// </summary>
        public override Expression<Func<InboundRecordReadModel, bool>> GetExpression()
        {

            if (_fieldName == PropertyExpressionHelper.GetPropertyName<IModelWithStringValidation>(x => x.Errors) &&
                _value is ErrorType err && _errorTypesDictionary.ContainsKey(err))
            {
                return _errorTypesDictionary[err];
            }

            if (_fieldName == PropertyExpressionHelper.GetPropertyName<InboundRecordReadModel>(x => x.NaftaRecon) &&
                _value is string st && _naftaDictionary.ContainsKey(st))
            {
                return _naftaDictionary[st];
            }

            if (_fieldName == PropertyExpressionHelper.GetPropertyName<InboundRecordReadModel>(x => x.PsaReason)
                && _value is PsaReason psaReason && _psaReasonDictionary.ContainsKey(psaReason))
            {
                return _psaReasonDictionary[psaReason];
            }

            if (_fieldName == PropertyExpressionHelper.GetPropertyName<InboundRecordReadModel>(x => x.PsaReason520d)
                && _value is PsaReason psaReason520d && _psaReason520dDictionary.ContainsKey(psaReason520d))
            {
                return _psaReason520dDictionary[psaReason520d];
            }

            if (_fieldName == PropertyExpressionHelper.GetPropertyName<InboundRecordReadModel>(x => x.ReconJobNumbers)
                && _value is JobNumberFilterValues jobNumber)
            {
                IEnumerable<JobNumberFilterValues> values = Enum.GetValues(jobNumber.GetType())
                    .Cast<Enum>()
                    .Where(jobNumber.HasFlag)
                    .Cast<JobNumberFilterValues>()
                    .Where(x => x != JobNumberFilterValues.None);

                Expression<Func<InboundRecordReadModel, bool>> expr = values
                    .Select(x=> _jobNumberDictionary[x])
                    .Aggregate((agg, next) => agg.OrExpression(next));

                return expr;
            }

            if (_fieldName == PropertyExpressionHelper.GetPropertyName<InboundRecordReadModel>(x => x.EntryPort)
                && _value is PortOfEntryFilterValues entryPort && _portOfEntryDictionary.ContainsKey(entryPort))
            {
                return _portOfEntryDictionary[entryPort];
            }

            return new DefaultSpecification<InboundRecordReadModel>().GetExpression();
        }
        /// <summary>
        /// Sets expression values for specification
        /// </summary>
        /// <param name="fieldName">Field name</param>
        /// <param name="values">Values for specification</param>
        public void SetValue(string fieldName, object[] values)
        {
            _fieldName = fieldName;
            if (_fieldName == PropertyExpressionHelper.GetPropertyName<IModelWithStringValidation>(x => x.Errors))
            {
                var filterValue = Convert.ToByte(values[0]);
                _value = (ErrorType)filterValue;
            }

            if (_fieldName == PropertyExpressionHelper.GetPropertyName<InboundRecordReadModel>(x => x.NaftaRecon))
            {
                _value = Convert.ToString(values[0]);
            }

            if (_fieldName == PropertyExpressionHelper.GetPropertyName<InboundRecordReadModel>(x => x.PsaReason) ||
                _fieldName == PropertyExpressionHelper.GetPropertyName<InboundRecordReadModel>(x => x.PsaReason520d))
            {
                _value = values.Select(Convert.ToInt32).Aggregate(PsaReason.None, (current, value) => current | (PsaReason)value);
            }

            if (_fieldName == PropertyExpressionHelper.GetPropertyName<InboundRecordReadModel>(x => x.ReconJobNumbers))
            {
                _value = values.Select(Convert.ToInt32).Aggregate(JobNumberFilterValues.None, (current, value) => current | (JobNumberFilterValues)value);
            }

            if (_fieldName == PropertyExpressionHelper.GetPropertyName<InboundRecordReadModel>(x => x.EntryPort))
            {
                var filterValue = Convert.ToInt32(values[0]);
                _value = (PortOfEntryFilterValues)filterValue;
            }
        }
    }
}
