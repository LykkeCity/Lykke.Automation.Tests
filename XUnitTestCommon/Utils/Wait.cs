using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace XUnitTestCommon.Utils
{
    public class Wait
    {
        public static void ForPredefinedTime(int waitTime = 1000)
        {
            Thread.Sleep(waitTime);
        }
    }
}
