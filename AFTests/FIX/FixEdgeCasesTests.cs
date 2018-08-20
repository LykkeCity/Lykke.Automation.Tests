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
using QuickFix.FIX44;
using System.Linq;
using System.Threading.Tasks;

namespace AFTests.FIX
{
    partial class FixTests
    {
        public class WrongCredentials : FixBaseTest
        {    
            [Test]
            [Category("FIX")]
            public void WrongCredentialsTest()
            {
                Environment.SetEnvironmentVariable("FIXWrongPassword", TestData.GenerateString(6));

                var fixClient2 = new FixClient("LYKKE_T", "SENDER_T", Init.LocalConfig()["TestClient"]["ServiceUrl"].ToString(), 12357);
                
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

        public class SecurityListclass : FixBaseTest
        {
            FixClient fixClient;

            [SetUp]
            public void SetUp()
            {
                fixClient = new FixClient(uri: Init.LocalConfig()["TestClient"]["ServiceUrl"].ToString());
                fixClient.Init();
            }

            [TearDown]
            public void TearDown()
            {
                fixClient.Stop();
                fixClient.Dispose();
            }

            [Test]
            [Category("FIX")]
            public void SecurityListclassTest()
            {
                var assetPairs = privateApi.Assets.GetAssetPairs().GetResponseObject().FindAll(a => a.IsDisabled == false);
                var validAssets = privateApi.Assets.GetAssets(false).GetResponseObject().
                    FindAll(a => a.IsDisabled == false).FindAll(a => a.IsTradable == true);

                assetPairs = assetPairs.FindAll(a =>
                    a.IsDisabled == false
                ).FindAll(a =>
                     validAssets.Any(va => va.Id == a.BaseAssetId)
                ).FindAll(a =>
                     validAssets.Any(va => va.Id == a.QuotingAssetId)
                );

                var m = new SecurityListRequest
                {
                    SecurityReqID = new SecurityReqID("42"),
                    SecurityListRequestType = new SecurityListRequestType(SecurityListRequestType.SYMBOL)
                };
                fixClient.Send(m);

                var response = fixClient.GetResponse<SecurityList>();
                Assert.That(response, Is.Not.Null);

                var sList = response.ToString();

                Assert.Multiple(() => 
                {
                    assetPairs.ForEach(a => Assert.That(sList, Does.Contain(a.Id), $"Security list does not contain asset pair {a.Id}"));
                });
            }
        }
    }
}
