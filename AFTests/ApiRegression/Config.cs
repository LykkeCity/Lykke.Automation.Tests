using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AFTests.ApiRegression
{
    static class Config
    {
        public static string BuyAssetEmail = Environment.GetEnvironmentVariable("ApiRegressionBuyAssetEmail") ??
                                             TestContext.Parameters["ApiRegressionBuyAssetEmail"] ??
                                             "untest005@test.com";
        public static string BuyAssetPassword = Environment.GetEnvironmentVariable("ApiRegressionBuyAssetPassword") ??
                                             TestContext.Parameters["ApiRegressionBuyAssetPassword"] ??
                                             "1234567";
        public static string BuyAssetPin = Environment.GetEnvironmentVariable("ApiRegressionBuyAssetPin") ??
                                             TestContext.Parameters["ApiRegressionBuyAssetPin"] ??
                                             "1111";
    }
}
