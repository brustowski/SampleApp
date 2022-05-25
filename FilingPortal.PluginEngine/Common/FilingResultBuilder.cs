using System.Collections.Generic;
using FilingPortal.PluginEngine.Models.InboundRecordModels;
using Reinforced.Typings.Attributes;

namespace FilingPortal.PluginEngine.Common
{
    /// <summary>
    /// Defines the <see cref="FilingResultBuilder" />
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false, AutoExportMethods = false)]
    public class FilingResultBuilder
    {
        /// <summary>
        /// Gets a value indicating whether overrall result is valid or not
        /// </summary>
        public bool IsValid => !Results.Exists(x => x.IsValid == false);

        /// <summary>
        /// Gets or sets the Error Message, if not valid
        /// </summary>
        public List<string> Messages { get; } = new List<string>();

        /// <summary>
        /// Gets or sets the results list
        /// </summary>
        public List<FilingResultViewModel> Results { get; } = new List<FilingResultViewModel>();

        /// <summary>
        /// Adds Bad result to results collection
        /// </summary>
        /// <param name="model">The model<see cref="InboundRecordFileModel"/></param>
        /// <param name="errorMessage">The errorMessage<see cref="string"/></param>
        public FilingResultBuilder AddBadResult(InboundRecordFileModel model, string errorMessage)
        {
            Results.Add(new FilingResultViewModel(model.FilingHeaderId, errorMessage));
            return this;
        }

        /// <summary>
        /// Adds Error message to errors message collection
        /// </summary>
        /// <param name="errorMessage">The errorMessage<see cref="string"/></param>
        public FilingResultBuilder AddErrorMessage(string errorMessage)
        {
            Messages.Add(errorMessage);
            return this;
        }

        /// <summary>
        /// Adds good result to results collection
        /// </summary>
        /// <param name="model">The model<see cref="InboundRecordFileModel"/></param>
        public FilingResultBuilder AddResult(InboundRecordFileModel model) {
            Results.Add(new FilingResultViewModel(model.FilingHeaderId));
            return this;
        }
    }
}
