using System;
using System.Collections.Generic;

namespace XUnitTestCommon.Utils
{
    public class EnumerableUtils
    {
        public static T PickRandom<T>(IList<T> model)
        {
            int randomInt = Helpers.Random.Next(model.Count);
            return model[randomInt];
        }
    }
}
