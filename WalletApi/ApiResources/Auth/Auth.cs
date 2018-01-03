﻿using Lykke.Client.AutorestClient.Models;
using LykkeAutomation.Api.ApiModels.AuthModels;
using LykkeAutomation.TestsCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XUnitTestCommon.RestRequests.Interfaces;
using XUnitTestCommon.TestsCore;
using static LykkeAutomation.Api.ApiModels.AuthModels.AuthModels;

namespace WalletApi.Api.AuthResource
{
    public class Auth : WalletApi
    {
        private const string resource = "/Auth";
        private const string resourceLogOut = "/Auth/LogOut";

        public IResponse<AuthModelResponse> PostAuthResponse(AuthenticateModel auth)
        {
            return Request.Post(resource).AddJsonBody(auth).Build().Execute<AuthModelResponse>();
        }

        public IResponse PostAuthLogOutResponse(AuthenticateModel auth)
        {
            return Request.Post(resourceLogOut).AddJsonBody(auth).Build().Execute();

        }
    }
}