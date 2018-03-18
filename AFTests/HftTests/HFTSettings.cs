using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AFTests.HftTests
{
    public class HFTSettingsModel
    {
        public string ApiKey { get; set; }
        public string FirstAssetId { get; set; }
        public string SecondAssetId { get; set; }
        public string AssetPair { get; set; }
    }

    public class HFTSettings
    {
        private static HFTSettingsModel _settings;

        public static HFTSettingsModel GetHFTSettings()
        {
            if (_settings != null)
                return _settings;

            if (File.Exists(Path.Combine(TestContext.CurrentContext.WorkDirectory, "properties.json")))
            {
                try
                {
                    _settings = JsonConvert.DeserializeObject<HFTSettingsModel>(File.ReadAllText(Path.Combine(TestContext.CurrentContext.WorkDirectory, "properties.json")));
                }
                catch (Exception e)
                {
                    TestContext.Progress.WriteLine("An error while parsing settings from properties.json");
                    TestContext.Progress.WriteLine(e);
                    TestContext.Progress.WriteLine(File.ReadAllText(Path.Combine(TestContext.CurrentContext.WorkDirectory, "properties.json")));
                }
                if (!string.IsNullOrEmpty(_settings?.ApiKey))
                {
                    TestContext.Progress.WriteLine($"propeties.json: {JsonConvert.SerializeObject(_settings)}");
                    return _settings;
                }
                else
                    TestContext.Progress.WriteLine("properties.json is present but api url is null or empty");
            }
            //in case nothing or existed properties.json missed some settings
            _settings = new HFTSettingsModel()
            {
                ApiKey = "3982d2d7-ae7b-4995-8074-d563707b986e",
                FirstAssetId = "BTC",
                SecondAssetId = "CHF",
                AssetPair = "BTCCHF"
            };
            TestContext.Progress.WriteLine($"propeties.json: {JsonConvert.SerializeObject(_settings)}");

            return _settings;
        }
    }
}
