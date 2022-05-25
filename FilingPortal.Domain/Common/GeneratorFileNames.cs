using Reinforced.Typings.Attributes;

namespace FilingPortal.Domain.Common
{
    [TsClass(IncludeNamespace = false, FlattenHierarchy = true)]
    public class GeneratorFileNames
    {
        /// <summary>
        /// ApiCalculator File Generator file name
        /// </summary>
        [TsProperty]
        public static string PipelineApiCalculatorFileName = "api_calculator.xlsx";
    }
}
