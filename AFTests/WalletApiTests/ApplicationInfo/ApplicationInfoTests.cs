using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.WalletApiTests.ApplicationInfo
{
    class ApplicationInfoTests : WalletApiBaseTest
    {
        public class GetApplicationInfo : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetApplicationInfoTest()
            {
                var applicationInfo = walletApi.ApplicationInfo.GetApplicationInfo();
                applicationInfo.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(applicationInfo.GetResponseObject(), Is.Not.Null, "ApplicationInfo object is null");
                Assert.That(applicationInfo.GetResponseObject().Result.AppVersion, Is.Not.Null, "Application version is null");
                Assert.That(applicationInfo.GetResponseObject().Result.SupportPhoneNum, Is.Not.Null, "SupportPhone number is null");
                Assert.That(applicationInfo.GetResponseObject().Result.TermsOfUseLink, Is.Not.Null, "Terms of use is null");
                Assert.That(applicationInfo.GetResponseObject().Result.UserAgreementUrl, Is.Not.Null, "User agreement url is null");
                Assert.That(applicationInfo.GetResponseObject().Result.RefundInfoLink, Is.Not.Null, "Refund info link is null");
                Assert.That(applicationInfo.GetResponseObject().Result.MarginSettings, Is.Not.Null, "Margin settings is null");
                Assert.That(applicationInfo.GetResponseObject().Result.InformationBrochureLink, Is.Not.Null, "Information Brochure link is null");
                Assert.That(applicationInfo.GetResponseObject().Result.CountryPhoneCodesLastModified, Is.Not.Null, "Country phone codes last modified is null");
                Assert.That(applicationInfo.GetResponseObject().Error, Is.Null, "Error is not null");
            }
        }
    }
}
