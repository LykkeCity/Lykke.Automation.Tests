using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using NUnit.Framework;
using LykkeAutomationPrivate.Models.ClientAccount.Models;

namespace LykkeAutomationPrivate.Tests.ClientAccount
{
    class IsAliveService : PrivateApiBaseTest
    {
        [Test]
        [Category("IsAliveService"), Category("ClientAccount"), Category("ServiceAll")]
        public void IsAliveServiceTest()
        {
            string serviceName = "Lykke.Service.ClientAccount";
            var isAlive = lykkeApi.ClientAccount.IsAliveService.GetIsAliveService();
            Assert.That(isAlive.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(isAlive.GetResponseObject().Name, Is.EqualTo(serviceName));
        }
    }
}
