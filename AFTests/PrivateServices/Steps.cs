using LykkeAutomationPrivate.DataGenerators;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using LykkeAutomationPrivate.Resources.ClientAccountResource;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.PrivateApiTests
{
    public static class Steps
    {
        //TODO: Think about
        //public static ClientAccountInformation GetClientAccount()
        //{
        //    ClientRegistrationModel clientRegistration = new ClientRegistrationModel().GetTestModel();

        //    var registerResponse = new Clients().PostRegister(clientRegistration);
        //    Assert.That(registerResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK), 
        //        "Couldn't register client");

        //    return registerResponse.GetResponseObject();
        //}
    }
}
