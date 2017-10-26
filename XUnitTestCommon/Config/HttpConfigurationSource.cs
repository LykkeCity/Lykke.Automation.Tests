using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Linq;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace XUnitTestCommon.Config
{
    public class HttpConfigurationSource : IConfigurationSource
    {
        private readonly string _settingsUrl;
        private readonly string _rootItemName;
        private readonly string _testItemName;

        public HttpConfigurationSource(string url, string rootItemName, string testItemName)
        {
            _settingsUrl = url;
            _rootItemName = rootItemName;
            _testItemName = testItemName;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new HttpConfigurationProvider(_settingsUrl, _rootItemName, _testItemName);
        }
    }

    public class HttpConfigurationProvider : ConfigurationProvider
    {
        private Dictionary<string, string> _config = new Dictionary<string, string>();

        private readonly string _settingsUrl;
        private readonly string _rootItemName;
        private readonly string _testItemName;

        public HttpConfigurationProvider(string url, string rootItemName, string testItemName)
        {
            _settingsUrl = url;
            _rootItemName = rootItemName;
            _testItemName = testItemName;
        }

        public override void Load()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string content = Task.Run(async () =>
                {
                    return await httpClient.GetStringAsync(_settingsUrl);
                }).Result;

                JObject settingsObject = JObject.Parse(content).SelectToken(_rootItemName).SelectToken(_testItemName).Value<JObject>();

                _config = settingsObject.ToObject<Dictionary<string, string>>();
            }
        }

        public override void Set(string key, string value)
        {
            throw new NotImplementedException();
        }

        public override bool TryGet(string key, out string value)
        {
            if (_config.TryGetValue(key, out string val))
            {
                value = val;
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }
    }
}
