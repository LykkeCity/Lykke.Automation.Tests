using AlgoStoreData.DTOs.InstanceData;
using System.Collections.Generic;
using System.ComponentModel;

namespace AlgoStoreData.Enums
{
    public class PredefinedValues
    {
        private static readonly List<PredefinedValue> candleTimeIntervals = new List<PredefinedValue>()
        {
            new PredefinedValue("Unspecified", 0),
            new PredefinedValue("Second", 1),
            new PredefinedValue("Minute", 60),
            new PredefinedValue("5 Minutes", 300),
            new PredefinedValue("15 Minutes", 900),
            new PredefinedValue("30 Minutes", 1800),
            new PredefinedValue("Hour", 3600),
            new PredefinedValue("4 Hours", 14400),
            new PredefinedValue("6 Hours", 21600),
            new PredefinedValue("12 Hours", 42300),
            new PredefinedValue("Day", 86400),
            new PredefinedValue("Week", 604800),
            new PredefinedValue("Month", 2419200)
        };

        private static readonly List<PredefinedValue> candleOperationModes = new List<PredefinedValue>()
        {
            new PredefinedValue("OPEN", 0),
            new PredefinedValue("CLOSE", 1),
            new PredefinedValue("LOW", 2),
            new PredefinedValue("HIGH", 3)
        };

        public static List<PredefinedValue> CandleTimeIntervals => candleTimeIntervals;
        public static List<PredefinedValue> CandleOperationModes => candleOperationModes;
    }
}
