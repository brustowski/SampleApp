using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Recon.Domain.Entities;
using Framework.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilingPortal.Parts.Recon.Domain.Validators
{
    /// <summary>
    /// Provides methods for single <see cref="InboundRecordReadModel"/> record validation
    /// </summary>
    public class CargoWiseRecordValidator : ISingleRecordValidator<InboundRecordReadModel>
    {
        /// <summary>
        /// Adds the list of the customs errors for the specified record
        /// </summary>
        /// <param name="errors">List of errors to witch to add customs errors</param>
        /// <param name="record">The record to check</param>
        private List<string> AddCustomErrors(List<string> errors, InboundRecordReadModel record)
        {
            if (!record.AceFound)
            {
                return errors;
            }

            errors.AddIf(record.MismatchReconValueFlag, string.IsNullOrWhiteSpace(record.AceReconIndicator) ? "Reconciliation Indicator doesn’t exist" : "Recon Issue mismatch");
            errors.AddIf(record.MismatchReconFtaFlag, string.IsNullOrWhiteSpace(record.AceNaftaReconIndicator) ? "NAFTA Reconciliation Indicator doesn’t exist" : "NAFTA Recon mismatch");
            errors.AddIf(record.MismatchEntryValue, record.AceLineGoodsValueAmount == 0 ? "Line Goods Value Amount doesn’t exist" : "Line Entered Value mismatch");
            errors.AddIf(record.MismatchDuty, record.AceLineDutyAmount == 0 ? "Line Duty Amount doesn’t exist" : "Duty mismatch");
            errors.AddIf(record.MismatchMpf, record.AceLineMpfAmount == 0 ? "Line MPF Amount doesn’t exist" : "MPF mismatch");
            errors.AddIf(record.MismatchPayableMpf, record.AceTotalPaidMpfAmount == 0 ? "Total Paid MPF Amount doesn’t exist" : "Entry Payable MPF mismatch");
            errors.AddIf(record.MismatchQuantity, record.AceLineTariffQuantity == 0 ? "Line Tariff Quantity (1) doesn’t exist" : "Customs Qty 1 mismatch");
            errors.AddIf(record.MismatchHts, string.IsNullOrWhiteSpace(record.AceHtsNumberFull) ? "HTS Number doesn’t exist" : "Tariff mismatch");

            return errors;
        }

        /// <summary>
        /// Gets the list of errors for the specified Single Record
        /// </summary>
        /// <param name="record">The record</param>
        public List<string> GetErrors(InboundRecordReadModel record)
        {
            var errors = new List<string>();
            return AddCustomErrors(errors, record);
        }

        /// <summary>
        /// Gets the list of errors for the specified Single Record
        /// </summary>
        /// <param name="record">The record</param>
        public Task<List<string>> GetErrorsAsync(InboundRecordReadModel record)
        {
            return Task.Run(() => GetErrors(record));
        }
    }
}
