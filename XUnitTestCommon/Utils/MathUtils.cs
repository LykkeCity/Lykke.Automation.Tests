using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon.Utils
{
    public class MathUtils
    {
        public static double RoundUp(double value, int precision)
        {
            int multiplier = (int)Math.Pow(10, precision);
            double ceiledValue = Math.Ceiling(value * multiplier);

            return ceiledValue / multiplier;
        }

        public static double RoundDown(double value, int precision)
        {
            int multiplier = (int)Math.Pow(10, precision);
            double flooredValue = Math.Floor(value * multiplier);

            return flooredValue / multiplier;
        }
    }
}
