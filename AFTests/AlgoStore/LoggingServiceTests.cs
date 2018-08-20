using AlgoStoreData.DTOs;
using AlgoStoreData.Fixtures;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using XUnitTestCommon;
using XUnitTestCommon.Utils;

namespace AFTests.AlgoStore
{
    [Category("FullRegression")]
    [Category("AlgoStore")]
    public partial class AlgoStoreTestsInstanceRequired : CreateAlgoWithInstanceFixture
    {
        [Test, Description("AL-524")]
        [Category("AlgoStore")]
        public async Task CheckWriteMessageToLog()
        {
            var writeMessageToLogUrl = $"{BaseUrl.AlgoStoreLoggingApiBaseUrl}{ApiPaths.ALGO_STORE_LOGGING_API_WRITE_MESSAGE}";

            // Instance token
            var instanceToken = await GetInstanceAuthToken(postInstanceData.InstanceId);

            // Build params dictionary
            var messageToInsert = $"Test Message added to instance log - {Helpers.GetTimestampIso8601()}";
            Dictionary<string, string> paramsDictionary = new Dictionary<string, string>();
            paramsDictionary.Add("instanceId", postInstanceData.InstanceId);
            paramsDictionary.Add("message", messageToInsert);

            var writeMessageToLogRequest = await Consumer.ExecuteRequestCustomEndpoint(writeMessageToLogUrl, paramsDictionary, null, Method.POST, authToken: instanceToken);
            Assert.That(writeMessageToLogRequest.Status, Is.EqualTo(HttpStatusCode.NoContent));

            // Get instance log
            var instanceLog = await GetInstanceTailLogFromLoggingService(postInstanceData);
            var instanceMessages = instanceLog.Select(x => x.Message).ToList();

            // Get instance log from Api
            var instanceLogFromApi = await GetInstanceTailLogFromApi(postInstanceData);

            // Assert message added to log
            Assert.Multiple(() =>
            {
                Assert.That(instanceMessages, Does.Contain(messageToInsert));
                Assert.That(instanceLogFromApi, Does.Contain(messageToInsert));
            });
        }

        [Test, Description("AL-524")]
        [Category("AlgoStore")]
        public async Task CheckWriteLog()
        {
            var writeMessageToLogUrl = $"{BaseUrl.AlgoStoreLoggingApiBaseUrl}{ApiPaths.ALGO_STORE_LOGGING_API_WRITE_LOG}";

            // Instance token
            var instanceToken = await GetInstanceAuthToken(postInstanceData.InstanceId);

            // Build request body
            var messageToInsert = $"Test Log added to instance log - {Helpers.GetTimestampIso8601()}";
            TailLogDTO tailMessage = new TailLogDTO()
            {
                InstanceId = postInstanceData.InstanceId,
                Message = messageToInsert
            };

            var requestBody = JsonUtils.SerializeObject(tailMessage);

            var writeMessageToLogRequest = await Consumer.ExecuteRequestCustomEndpoint(writeMessageToLogUrl, Helpers.EmptyDictionary, requestBody, Method.POST, authToken: instanceToken);
            Assert.That(writeMessageToLogRequest.Status, Is.EqualTo(HttpStatusCode.NoContent));

            // Get instance log
            var instanceLog = await GetInstanceTailLogFromLoggingService(postInstanceData);
            var instanceMessages = instanceLog.Select(x => x.Message).ToList();

            // Get instance log from Api
            var instanceLogFromApi = await GetInstanceTailLogFromApi(postInstanceData);

            // Assert message added to log
            Assert.Multiple(() =>
            {
                Assert.That(instanceMessages, Does.Contain(messageToInsert));
                Assert.That(instanceLogFromApi, Does.Contain(messageToInsert));
            });
        }

        [Test, Description("AL-524")]
        [Category("AlgoStore")]
        public async Task CheckWriteLogs()
        {
            var writeMessagesToLogUrl = $"{BaseUrl.AlgoStoreLoggingApiBaseUrl}{ApiPaths.ALGO_STORE_LOGGING_API_WRITE_LOGS}";

            // Instance token
            var instanceToken = await GetInstanceAuthToken(postInstanceData.InstanceId);

            // Keep messaged that will be inserted in a list
            List<String> logMessages = new List<string>();

            // Build request body
            List<TailLogDTO> logMessagesToInsert = new List<TailLogDTO>();

            for (int i = 1; i <= 100; i++)
            {
                var messageToInsert = $"Test Log added to instance log - {i.ToString().PadLeft(3, '0')}";
                TailLogDTO logMessageToInsert = new TailLogDTO()
                {
                    InstanceId = postInstanceData.InstanceId,
                    Message = messageToInsert
                };

                logMessages.Add(messageToInsert);
                logMessagesToInsert.Add(logMessageToInsert);
            }

            var requestBody = JsonUtils.SerializeObject(logMessagesToInsert);

            var writeMessageToLogRequest = await Consumer.ExecuteRequestCustomEndpoint(writeMessagesToLogUrl, Helpers.EmptyDictionary, requestBody, Method.POST, authToken: instanceToken);
            Assert.That(writeMessageToLogRequest.Status, Is.EqualTo(HttpStatusCode.NoContent));

            // Get instance log from Logging Service
            var instanceLog = await GetInstanceTailLogFromLoggingService(postInstanceData);
            var instanceMessages = instanceLog.Select(x => x.Message).ToList();

            // Get instance log from Api
            var instanceLogFromApi = await GetInstanceTailLogFromApi(postInstanceData);

            // Assert message added to log
            Assert.Multiple(() =>
            {
                Assert.That(logMessages, Is.SubsetOf(instanceMessages));
                foreach (var l in logMessagesToInsert)
                {
                    Assert.That(instanceLogFromApi, Does.Contain(l.Message));
                }
            });
        }

        [Test, Description("AL-524")]
        [TestCase("")]
        [TestCase("NonExistingToken-1234567890")]
        [TestCase("NonExistingToken")]
        [TestCase("1234567890")]
        [TestCase(null)]
        public async Task CheckWriteLogsInvalidToken(string instanceToken)
        {
            var writeMessagesToLogUrl = $"{BaseUrl.AlgoStoreLoggingApiBaseUrl}{ApiPaths.ALGO_STORE_LOGGING_API_WRITE_LOGS}";

            // Keep messaged that will be inserted in a list
            List<String> logMessages = new List<string>();

            // Build request body
            List<TailLogDTO> logMessagesToInsert = new List<TailLogDTO>();

            for (int i = 1; i <= 100; i++)
            {
                var messageToInsert = $"Test Log added to instance log - {i.ToString().PadLeft(3, '0')}";
                TailLogDTO logMessageToInsert = new TailLogDTO()
                {
                    InstanceId = postInstanceData.InstanceId,
                    Message = messageToInsert
                };

                logMessages.Add(messageToInsert);
                logMessagesToInsert.Add(logMessageToInsert);
            }

            var requestBody = JsonUtils.SerializeObject(logMessagesToInsert);

            var writeMessageToLogRequest = await Consumer.ExecuteRequestCustomEndpoint(writeMessagesToLogUrl, Helpers.EmptyDictionary, requestBody, Method.POST, authToken: instanceToken);
            Assert.That(writeMessageToLogRequest.Status, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test, Description("AL-524")]
        public async Task CheckWriteLogsInvalidRequestBody()
        {
            var writeMessageToLogUrl = $"{BaseUrl.AlgoStoreLoggingApiBaseUrl}{ApiPaths.ALGO_STORE_LOGGING_API_WRITE_MESSAGE}";
            var writeSignleMessageToLogUrl = $"{BaseUrl.AlgoStoreLoggingApiBaseUrl}{ApiPaths.ALGO_STORE_LOGGING_API_WRITE_LOG}";
            var writeMultipleMessagesToLogUrl = $"{BaseUrl.AlgoStoreLoggingApiBaseUrl}{ApiPaths.ALGO_STORE_LOGGING_API_WRITE_LOGS}";

            // Instance token
            var instanceToken = await GetInstanceAuthToken(postInstanceData.InstanceId);

            // Build single message request body
            var messageToInsert = $"Test Log added to instance log - {Helpers.GetTimestampIso8601()}";

            // Replace negative messages
            var messagesString = NegativeMessagesAsString.Replace("replaceWithInstanceId", postInstanceData.InstanceId).Replace("replaceWithMessage", messageToInsert);

            // Build request body
            List<TailLogDTO> emptyList = new List<TailLogDTO>();
            List<TailLogDTO> invalidMessages = JsonUtils.DeserializeJson<List<TailLogDTO>>(messagesString);

            // List of messages
            var emptyListRequestBody = JsonUtils.SerializeObject(emptyList);
            var multipleMessagesRequestBody = JsonUtils.SerializeObject(invalidMessages);

            // Keep responses in a list
            List<Response> requestResponses = new List<Response>();

            // Send various invalid messages to Single Message Log
            foreach (var message in invalidMessages)
            {
                var singleMessageRequestBody = JsonUtils.SerializeObject(message);
                var writeSingleMessageToLogRequest = await Consumer.ExecuteRequestCustomEndpoint(writeSignleMessageToLogUrl, Helpers.EmptyDictionary, singleMessageRequestBody, Method.POST, authToken: instanceToken);
                requestResponses.Add(writeSingleMessageToLogRequest);
            }

            // Send list of messages to Single Message Log
            var writeMultipleMessagesToSingleLogRequest = await Consumer.ExecuteRequestCustomEndpoint(writeSignleMessageToLogUrl, Helpers.EmptyDictionary, multipleMessagesRequestBody, Method.POST, authToken: instanceToken);

            // Send multiple invalid messages to Multi Message Log
            var writeMultipleMessagesToMultiLogRequest = await Consumer.ExecuteRequestCustomEndpoint(writeMultipleMessagesToLogUrl, Helpers.EmptyDictionary, multipleMessagesRequestBody, Method.POST, authToken: instanceToken);

            // Send null as request body to single and multi log
            Dictionary<string, string> paramsDictionary = new Dictionary<string, string>();
            paramsDictionary.Add("instanceId", postInstanceData.InstanceId);
            paramsDictionary.Add("message", null);
            var nullWriteMessageToLogUrl = await Consumer.ExecuteRequestCustomEndpoint(writeMessageToLogUrl, paramsDictionary, null, Method.POST, authToken: instanceToken);

            // Send one message to multi log
            TailLogDTO logMessageToInsert = new TailLogDTO()
            {
                InstanceId = postInstanceData.InstanceId,
                Message = messageToInsert
            };
            messageToInsert = JsonUtils.SerializeObject(logMessageToInsert);
            var oneMessageToMultiRequest = await Consumer.ExecuteRequestCustomEndpoint(writeMultipleMessagesToLogUrl, Helpers.EmptyDictionary, messageToInsert, Method.POST, authToken: instanceToken);

            Assert.Multiple(() =>
            {
                foreach (var response in requestResponses)
                {
                    Assert.That(response.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
                    Assert.That(response.ResponseJson, Does.Match(".*Validation error: .* cannot be empty.*"));
                }

                Assert.That(writeMultipleMessagesToSingleLogRequest.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
                Assert.That(writeMultipleMessagesToSingleLogRequest.ResponseJson, Does.Match(".*Technical problem.*"));

                Assert.That(writeMultipleMessagesToMultiLogRequest.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
                Assert.That(writeMultipleMessagesToMultiLogRequest.ResponseJson, Does.Match(".*InstanceId must be the same for all logs*"));

                Assert.That(nullWriteMessageToLogUrl.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
                Assert.That(nullWriteMessageToLogUrl.ResponseJson, Does.Match(".*Validation error: .* cannot be empty.*"));

                Assert.That(oneMessageToMultiRequest.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
                Assert.That(oneMessageToMultiRequest.ResponseJson, Does.Match(".*Technical problem.*"));
            });
        }

        [Test, Description("AL-524")]
        public async Task CheckWriteHugeBatchOfLogs()
        {
            var writeMessagesToLogUrl = $"{BaseUrl.AlgoStoreLoggingApiBaseUrl}{ApiPaths.ALGO_STORE_LOGGING_API_WRITE_LOGS}";

            // Instance token
            var instanceToken = await GetInstanceAuthToken(postInstanceData.InstanceId);

            // Keep messaged that will be inserted in a list
            List<String> logMessages = new List<string>();

            // Build request body
            List<TailLogDTO> logMessagesToInsert = new List<TailLogDTO>();

            for (int i = 1; i <= 150; i++)
            {
                var messageToInsert = $"Test Log added to instance log - {i.ToString().PadLeft(3, '0')}";
                TailLogDTO logMessageToInsert = new TailLogDTO()
                {
                    InstanceId = postInstanceData.InstanceId,
                    Message = messageToInsert
                };

                logMessages.Add(messageToInsert);
                logMessagesToInsert.Add(logMessageToInsert);
            }

            var requestBody = JsonUtils.SerializeObject(logMessagesToInsert);

            var writeMessageToLogRequest = await Consumer.ExecuteRequestCustomEndpoint(writeMessagesToLogUrl, Helpers.EmptyDictionary, requestBody, Method.POST, authToken: instanceToken);
            Assert.That(writeMessageToLogRequest.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
            Assert.That(writeMessageToLogRequest.ResponseJson, Does.Match(".*Validation error: Cannot save more then 100 log entries per batch.*"));

            // Get instance log from Logging Service
            var instanceLog = await GetInstanceTailLogFromLoggingService(postInstanceData);
            var instanceMessages = instanceLog.Select(x => x.Message).ToList();

            // Get instance log from Api
            var instanceLogFromApi = await GetInstanceTailLogFromApi(postInstanceData);

            // Assert message added to log
            Assert.Multiple(() =>
            {
                Assert.That(logMessages, Is.Not.SubsetOf(instanceMessages));
                foreach (var l in logMessagesToInsert)
                {
                    Assert.That(instanceLogFromApi, Does.Not.Contain(l.Message));
                }
            });
        }
    }
}
