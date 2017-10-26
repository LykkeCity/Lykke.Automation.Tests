using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon.Utils
{
    public class EnumerableUtils
    {
        public static T PickRandom<T>(IList<T> model)
        {
            Random rnd = new Random();
            int randomInt = rnd.Next(model.Count);
            return model[randomInt];
        }
    }
}
