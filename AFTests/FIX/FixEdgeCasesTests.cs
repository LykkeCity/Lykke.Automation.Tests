using FIX.Client;
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
    partial class FixTests
    {

        public class TwoClientsInParrallel : FixBaseTest
        {
            protected FixClient fixClient;

            [SetUp]
            public void SetUp()
            {
                fixClient = new FixClient("LYKKE_T", "SENDER_T", Init.LocalConfig().GetSection("TestClient:ServiceUrl").Value, 12357);
                fixClient.Init();
            }

            [TearDown]
            public void TearDown()
            {
                try
                {
                    fixClient.Stop();
                    fixClient.Dispose();
                }
                catch (Exception) { }
            }

            //[Test]
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
                finally
                {
                    fixClient2.Stop();
                }
            }
        }

        public class WrongCredentials : FixBaseTest
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
                catch (Exception e)
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
