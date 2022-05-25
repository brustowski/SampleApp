using System;
using Framework.Infrastructure;

namespace FilingPortal.Parts.Zones.Entry.Domain.Converters
{
    /// <summary>
    /// Converts weight from kilograms to tons and kilotons
    /// </summary>
    internal class WeightConverter
    {
        /// <summary>
        /// Conversion types
        /// </summary>
        public enum WeightConversionTypes
        {
            WholeNumber,
            DecimalNumber
        }

        /// <summary>
        /// Converted weight
        /// </summary>
        public decimal? Weight { get; private set; }
        /// <summary>
        /// Converted unit
        /// </summary>
        public string Measure { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="WeightConverter"/>
        /// </summary>
        /// <param name="value">Value in KG</param>
        /// <param name="conversionType">Conversion type</param>
        public WeightConverter(string value, WeightConversionTypes conversionType)
        {
            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var weight = Convert.ToDecimal(value);
                    // if (weight > 10000000) TODO check the conversion logic
                                     
                    if (weight > 99999999)
                        {
                        //Weight = Math.Round(weight /  100000,
                          Weight = Math.Round(weight / 1000000,
                            conversionType == WeightConversionTypes.DecimalNumber ? 4 : 0);
                        Measure = "KT";
                    }
                    else
                    {
                        Weight = Math.Round(weight / 1000,
                                conversionType == WeightConversionTypes.DecimalNumber ? 4 : 0);
                            Measure = "T";
                       
                    }
                }
                else
                {
                    Weight = 0;
                    Measure = "T";
                }
            }
            catch (Exception ex)
            {
                Weight = null;
                Measure = null;
                AppLogger.Error(ex, "Error when converting gross weight");
            }
        }
    }
}
