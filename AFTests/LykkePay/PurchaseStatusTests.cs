﻿using LykkePay.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.LykkePayTests
{
    class PurchaseStatusTests
    {
        public class GetPurchaseStatus : LykkepPayBaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void GetPurchaseStatusTest()
            {
                string transactionId = "8062ea42-070a-4db6-ab1d-4073806d719c"; //TODO: Where to get transaction?

                var purchaseStatus = lykkePayApi.purchaseStatus.GetPurchaseStatusResponse(transactionId);

                Assert.That(purchaseStatus.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                //TODO: Check purchase status
            }
        }

        public class GetPurchaseStatusNegative : LykkepPayBaseTest
        {
            [TestCase("")]
            [TestCase("!@#$%")]
            [TestCase("test")]
            [TestCase("test test")]
            [Category("LykkePay")]
            public void GetPurchaseStatusNegativeTest(string transactionId)
            {
                var purchaseStatus = lykkePayApi.purchaseStatus.GetPurchaseStatusResponse(transactionId);

                Assert.That(purchaseStatus.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            }
        }
    }
}
