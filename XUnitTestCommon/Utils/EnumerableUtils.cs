using System;
using System.Collections.Generic;

namespace XUnitTestCommon.Utils
{
    public class EnumerableUtils
    {
        public static T PickRandom<T>(IList<T> model)
        {
            //if (model == null || model.Count == 0)
            //    return (T) (object)null;

            Random rnd = new Random();
            int randomInt = rnd.Next(model.Count);
            return model[randomInt];
        }
    }
}
