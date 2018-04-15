﻿using FIX.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Message = QuickFix.Message;
using QuickFix;
using QuickFix.Fields;
using QuickFix.FIX44;
using QuickFix.Lykke;
using XUnitTestCommon.TestsData;
using XUnitTestCommon.Tests;

namespace AFTests.FIX
{
    class FixEdgeCasesTests
    {

        public class TwoClientsInParrallel : FixBaseTest
        {
            [Test]
            [Category("FIX")]
            public void TwoClientsInParrallelTest()
            {
                var fixClient2 = new FixClient("LYKKE_T", "SENDER_T", Init.LocalConfig().GetSection("TestClient:ServiceUrl").Value, 12357);
                fixClient2.Init();

                var orderId = Guid.NewGuid().ToString();
                var price = 0.01m;
                var quantity = 0.01m;
                var marketOrder = FixHelpers.CreateNewOrder(orderId, isMarket: false, isBuy: true, qty: quantity, price: price);

                try
                {
                    fixClient.Send(marketOrder);
                    
                    Assert.Fail("First instance on FIXClient has not been disconnected after second has been created");
                }catch(Exception e)
                {
                    //Pass
                }
            }
        }

        public class WrongCredentials : BaseTest
        {    
            [Test]
            [Category("FIX")]
            public void WrongCredentialsTest()
            {
                Environment.SetEnvironmentVariable("FIXWrongPassword", TestData.GenerateString(6));

                var fixClient2 = new FixClient("LYKKE_T", "SENDER_T", Init.LocalConfig().GetSection("TestClient:ServiceUrl").Value, 12357);
                
                fixClient2.Init();

                var orderId = Guid.NewGuid().ToString();
                var price = 0.01m;
                var quantity = 0.01m;
                var marketOrder = FixHelpers.CreateNewOrder(orderId, isMarket: false, isBuy: true, qty: quantity, price: price);

                try
                {
                    fixClient2.Send(marketOrder);
                    Assert.Fail("First instance on FIXClient has not been disconnected after second has been created");
                }
                catch (Exception)
                {
                    fixClient2.GetResponse<Message>();
                }
                finally
                {
                    Environment.SetEnvironmentVariable("FIXWrongPassword", null);
                }
            }
        }
    }
}