using LykkeAutomationPrivate;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AFTests.HftTests
{
    public interface IHFTSettingsModel
    {
        string ApiKey { get; }
        string FirstAssetId { get; }
        string SecondAssetId { get; }
        string AssetPair { get; }
        string SecondApiKey { get; }
    }

    public class HFTSettings
    {
        private static IHFTSettingsModel _settings;

        public static IHFTSettingsModel GetHFTSettings()
        {
            if (_settings != null)
                return _settings;

            if (File.Exists(Path.Combine(TestContext.CurrentContext.WorkDirectory, "properties.json")))
            {
                try
                {
                    _settings = JsonConvert.DeserializeObject<IHFTSettingsModel>(File.ReadAllText(Path.Combine(TestContext.CurrentContext.WorkDirectory, "properties.json")));
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
            _settings = EnvConfig.Env == Env.Dev ? (IHFTSettingsModel)new HFTDevSettings() : (IHFTSettingsModel)new HFTTestSettings();
            TestContext.Progress.WriteLine($"propeties.json: {JsonConvert.SerializeObject(_settings)}");

            return _settings;
        }
    }

    public class HFTTestSettings : IHFTSettingsModel
    {
        public string ApiKey { get { return "1606b4dd-fe22-4425-92ea-dccd5fffcce8"; } } //1606b4dd-fe22-4425-92ea-dccd5fffcce8
        public string FirstAssetId { get { return "91bac417-18b2-4e29-8faa-e9a991c9167e"; } }//BCH
        public string SecondAssetId { get { return "b7d8d50a-f911-4dab-ad0b-b8efc8cd86fc"; } }//BTG
        public string AssetPair { get { return "AUTOTESTHFT"; } }

        public string SecondApiKey { get { return "db448995-39af-4a83-9bd4-800405bf56bb"; } }
    }

    public class HFTDevSettings : IHFTSettingsModel
    {
        public string ApiKey { get { return "d316fc15-6e61-4773-824d-17bc11b04023"; } }
        public string FirstAssetId { get { return "2a1ba47b-406c-42d6-94f2-4f5a201b7eac"; } }//BCH
        public string SecondAssetId { get { return "30e84f2f-6249-4e0c-9423-496e344543d3"; } }//BTG
        public string AssetPair { get { return "AUTOTESTHFT"; } }

        public string SecondApiKey { get { return "55f58239-2d88-44d0-a871-144261aaf017"; } }
    }
}
