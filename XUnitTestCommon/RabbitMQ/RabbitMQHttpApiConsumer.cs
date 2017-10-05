using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using XUnitTestCommon.Utils;
using XUnitTestCommon.DTOs.RabbitMQ;

namespace XUnitTestCommon.RabbitMQ
{
    public static class RabbitMQHttpApiConsumer
    {
        private static HttpClient _client = new HttpClient();
        private static ConfigBuilder _config;

        public static void Setup(ConfigBuilder configBuidler)
        {
            _config = configBuidler;

            _client.BaseAddress = new Uri("http://" + _config.Config["RabbitMQHost"] + ":" + _config.Config["RabbitMQPort"]);

            byte[] authBytes = new UTF8Encoding().GetBytes(_config.Config["RabbitMQUsername"] + ":" + _config.Config["RabbitMQPassword"]);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        //public static async Task<string> GetAllNodesJson()
        //{
        //    return await ExecuteGetRequestAsync("/api/nodes");
        //}

        public static async Task<bool> CreateQueueAsync(string name, string vhost = "%2f")
        {
            StringBuilder requestPathSb = new StringBuilder("/api/queues");
            requestPathSb.Append("/");
            requestPathSb.Append(vhost);
            requestPathSb.Append("/");
            requestPathSb.Append(name);

            RabbitMQCreateQueueDTO requestData = new RabbitMQCreateQueueDTO();
            requestData.arguments = new RabbitMQCreateQueueDTO.Arguments();
            requestData.auto_delete = false;
            requestData.durable = true;
            requestData.node = _config.Config["RabbitMQNode"];

            string requestDataStr = JsonUtils.SerializeObject(requestData);

            string putResponse = await ExecutePutRequestAsync(requestPathSb.ToString(), new StringContent(requestDataStr));

            if (putResponse == null)
                return true;
            return false;
        }

        public static async Task<bool> DeleteQueueAsync(string name, string vhost = "%2f")
        {
            StringBuilder requestPathSb = new StringBuilder("/api/queues");
            requestPathSb.Append("/");
            requestPathSb.Append(vhost);
            requestPathSb.Append("/");
            requestPathSb.Append(name);

            string deleteResponse = await ExecuteDeleteRequestAsync(requestPathSb.ToString());

            if (deleteResponse == null)
                return true;
            return false;
        }

        public static async Task<bool> BindQueueAsync(string exchangeName, string queueName, string routingKey = "", string vhost = "%2f")
        {
            StringBuilder requestPathSb = new StringBuilder("/api/bindings");
            requestPathSb.Append("/");
            requestPathSb.Append(vhost);
            requestPathSb.Append("/e/");
            requestPathSb.Append(exchangeName);
            requestPathSb.Append("/q/");
            requestPathSb.Append(queueName);

            RabbitMQBindQueueDTO requestData = new RabbitMQBindQueueDTO();
            requestData.arguments = new RabbitMQBindQueueDTO.Arguments();
            requestData.routing_key = routingKey;

            string requestDataStr = JsonUtils.SerializeObject(requestData);

            string postResponse = await ExecutePostRequestAsync(requestPathSb.ToString(), new StringContent(requestDataStr));

            if (postResponse == null)
                return true;
            return false;
        }

        public static async Task<RabbitMQHttpApiQueueResultDTO> GetQueueByNameAsync(string name, string vhost = "%2f")
        {
            StringBuilder requestPathSb = new StringBuilder("/api/queues");
            requestPathSb.Append("/");
            requestPathSb.Append(vhost);
            requestPathSb.Append("/");
            requestPathSb.Append(name);

            return await ExecuteGetRequestJsonAsync<RabbitMQHttpApiQueueResultDTO>(requestPathSb.ToString());

        }

        public static async Task<List<RabbitMQHttpApiQueueResultDTO>> GetAllQueuesAsync(string vhost = null)
        {
            StringBuilder requestPathSb = new StringBuilder("/api/queues");
            if (vhost != null)
            {
                requestPathSb.Append("/");
                requestPathSb.Append(vhost);
            }

            return await ExecuteGetRequestJsonAsync<List<RabbitMQHttpApiQueueResultDTO>>(requestPathSb.ToString());

        }

        private static async Task<string> ExecuteGetRequestAsync(string path)
        {
            string strResponse = null;
            HttpResponseMessage response = await _client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                strResponse = await response.Content.ReadAsStringAsync();
            }
            return strResponse;
        }

        private static async Task<T> ExecuteGetRequestJsonAsync<T>(string path)
        {
            string strResponse = await ExecuteGetRequestAsync(path);
            if (strResponse == null)
            {
                strResponse = "null";
            }

            T result = JsonUtils.DeserializeJson<T>(strResponse);
            return result;
        }

        private static async Task<string> ExecutePutRequestAsync(string path, StringContent content)
        {
            string strResponse = null;
            HttpResponseMessage response = await _client.PutAsync(path, content);
            if (!response.IsSuccessStatusCode)
            {
                strResponse = await response.Content.ReadAsStringAsync();
            }
            return strResponse;
        }

        private static async Task<string> ExecutePostRequestAsync(string path, StringContent content)
        {
            string strResponse = null;
            HttpResponseMessage response = await _client.PostAsync(path, content);
            if (!response.IsSuccessStatusCode)
            {
                strResponse = await response.Content.ReadAsStringAsync();
            }
            return strResponse;
        }

        private static async Task<string> ExecuteDeleteRequestAsync(string path)
        {
            string strResponse = null;
            HttpResponseMessage response = await _client.DeleteAsync(path);
            if (!response.IsSuccessStatusCode)
            {
                strResponse = await response.Content.ReadAsStringAsync();
            }
            return strResponse;
        }
    }
}
