using LykkeAutomation.TestsCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using LykkePay.Api;
using XUnitTestCommon.Reports;
using XUnitTestCommon.TestsCore;
using XUnitTestCommon.Tests;

namespace AFTests.LykkePayTests
{
    public class LykkepPayBaseTest : BaseTest
    {
        public LykkePayApi lykkePayApi = new LykkePayApi(); 
    }
}
