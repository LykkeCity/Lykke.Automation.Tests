using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XUnitTestCommon.TestsCore;

namespace AFTests.BlockchainsIntegration
{
    public class BlockchainSpecificSettingsFactory
    {
        private static BlockchainSpecificModel _settings;

        public static BlockchainSpecificModel BlockchainSpecificSettings(string blockchain)
        {
            if (_settings != null)
                return _settings;

            if (File.Exists(Path.Combine(TestContext.CurrentContext.WorkDirectory, "properties.json")))
            {
                try
                {
                    _settings = LocalConfig.LocalConfigModel();
                }catch(Exception e)
                {
                    TestContext.Progress.WriteLine("An error while parsing settings from properties.json");
                    TestContext.Progress.WriteLine(e);
                    TestContext.Progress.WriteLine(File.ReadAllText(Path.Combine(TestContext.CurrentContext.WorkDirectory, "properties.json")));
                }
                if (!string.IsNullOrEmpty(_settings?.BlockchainApi))
                {
                    TestContext.Progress.WriteLine($"propeties.json: {JsonConvert.SerializeObject(_settings)}");
                    return _settings;
                }  
                else
                    TestContext.Progress.WriteLine("properties.json is present but api url is null or empty");
            }

            blockchain = blockchain.ToLower();

            if (blockchain == "litecoin")
                _settings = new LitecoinSettings();

            if (blockchain == "dash")
                _settings = new DashSettings();

            if (blockchain == "zcash")
                _settings = new ZcashSettings();

            if (blockchain == "ripple")
                _settings = new RippleSettings();

            if (blockchain == "stellar")
                _settings = new StellarSettings();

            if (blockchain == "stellar-v2")
                _settings = new StellarV2Settings();

            if (blockchain == "bitshares")
                _settings = new BitsharesSettings();

            if (blockchain == "raiblocks")
                _settings = new RaiBlocksSettings();

            if(blockchain == "waves")
                _settings = new WavesSettings();

            TestContext.Progress.WriteLine($"propeties.json: {JsonConvert.SerializeObject(_settings)}");

            return _settings;
        }


        class LitecoinSettings : BlockchainSpecificModel
        {
            public LitecoinSettings()
            {
                BlockchainApi = "http://litecoin-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://litecoin-sign.autotests-service.svc.cluster.local/api";
                WalletsUrl = null;
                ClientId = "b623b171-a307-4485-897c-f3a70b763217";
                AssetId = "LTC";
            }
        }

        class DashSettings : BlockchainSpecificModel
        {
            public DashSettings()
            {
                BlockchainApi = "http://dash-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://dash-sign.autotests-service.svc.cluster.local/api";
                WalletsUrl = null;
                ClientId = "b623b171-a307-4485-897c-f3a70b763217";
                AssetId = "DASH";
            }
        }

        class ZcashSettings : BlockchainSpecificModel
        {
            public ZcashSettings()
            {
                BlockchainApi = "http://zcash-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://zcash-sign-service.autotests-service.svc.cluster.local/api";
                WalletsUrl = null;
                ClientId = "b623b171-a307-4485-897c-f3a70b763217";
                AssetId = "ZEC";
            }
        }

        class RippleSettings : BlockchainSpecificModel
        {
            public RippleSettings()
            {
                BlockchainIntegration = "Ripple";
                BlockchainApi = "http://ripple-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://ripple-sign-service.autotests-service.svc.cluster.local/api";
                WalletsUrl = null;
                HotWallet = "rJ2zMCyShMsjHf7tHptVy7qCWt6dYv8B7r";
                HotWalletKey = "sndT9kLXjNv7L1HWfr6Lo5N6pmEvU";
                ExternalWalletAddress = "rG1Zu2dm2Ty9pQrnGJux1RuKZA6qhjWwMc";
                ExternalWalletKey = "ss91FvLcTaKNBAMtyfN2X3izKhNux";
                ClientId = "b623b171-a307-4485-897c-f3a70b763217";
                AssetId = "XRP";
            }
        }

        class StellarSettings : BlockchainSpecificModel
        {
            public StellarSettings()
            {
                BlockchainApi = "http://stellar-api.lykke-service.svc.cluster.local/api";
                BlockchainSign = "http://stellar-sign-service.lykke-service.svc.cluster.local/api";
                WalletsUrl = null;
                ClientId = "b623b171-a307-4485-897c-f3a70b763217";
                AssetId = "XLM";
            }
        }

        class StellarV2Settings : BlockchainSpecificModel
        {
            public StellarV2Settings()
            {
                BlockchainIntegration = "Stellar";
                BlockchainApi = "http://stellar-api-schnidlo.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://stellar-sign-service-schnidlo.autotests-service.svc.cluster.local/api";
                HotWallet = "GCCWY6MNVWORHO7B3L6W5LULGFR337K5UAATA2Z3FSPQA5MYRW5X33ZP";
                HotWalletKey = "SBBEPWMAINBQKQFW42L6GTWF73WPP5VRVG6KZHCWZVSVM4DUMWZESK2Z";
                WalletsUrl = null;
                ClientId = "b623b171-a307-4485-897c-f3a70b763217";
                AssetId = "XLM";
                ExternalWalletAddress = "GDTCLM7L3RKSP324DBRROKRLTRU4WLSPLHDEVSC75LAVZFN6ZDQPK7LD";
                ExternalWalletKey = "SCCSEPSZNHAIJXUVVHAN7ZHBSBSTBJY5Y2W7CJYIP3FALGAFEV5FEGMQ";
            }
        }

        class BitsharesSettings : BlockchainSpecificModel
        {
            public BitsharesSettings()
            {
                BlockchainIntegration = "Bitshares";
                BlockchainApi = "http://lykke-service-bitshares-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://lykke-service-bitshares-sign.autotests-service.svc.cluster.local/api";
                WalletsUrl = null;
                HotWallet = "1.2.20477:e91757da-6a69-43f4-8283-b2612703af59";
                HotWalletKey = "keep_keys_private";
                ExternalWalletAddress = "1.2.22396:::06eea045-43ee-4cca-a19d-1356abc2b70e";
                ExternalWalletKey = "P5JjofmjPvvEt9iu7o3DneEqcMCzBesjcybJfGHHHGRtK";
                ClientId = "";
                AssetId = "1.3.0";
                ExternalWallerAddressContext = "5KJBVnJaiYhVq7x3mF47f5xd6RUisnqjWCdc5fx9uhWSDdrd1MR";
            }
        }

        public class RaiBlocksSettings : BlockchainSpecificModel
        {
            public RaiBlocksSettings()
            {
                BlockchainIntegration = "RaiBlocks";
                BlockchainApi = "http://raiblocks-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://raiblocks-sign.autotests-service.svc.cluster.local/api";
                HotWallet = "xrb_1ygmfkg4x394rnhpyhafs1qgwpdyiru1f393osig37qzoeiqqhpdxfa7w6kk";
                HotWalletKey = "14DA575835310081121A8F7AA656377E5CBDBB96D8CE8672A564AF902F128FCB";
                ExternalWalletAddress = "xrb_1e9pi3r7qa9qpsas68go55rmu7m8ku9np5pw3roeekx671joz7b1eiu1i8zk";
                ExternalWalletKey = "4CF23CABA08F2C105DE9B80AA5CEBE37CAA3285E015258513F84E41100BDE2A9";
                ExternalWallerAddressContext = "";
                ClientId = "";
                AssetId = "XRB";
            }
        }

        public class WavesSettings : BlockchainSpecificModel
        {
            public WavesSettings()
            {
                BlockchainIntegration = "Waves";

                BlockchainApi = "http://lykke-service-waves-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://lykke-service-waves-sign.autotests-service.svc.cluster.local/api";
                HotWallet = "3N8FLCfGbzyPFZE2NPjXTpgLPXJy4KvnbdL";
                HotWalletKey = "5sh7zcP4W3jnhTmF7ZnhKVsnpmLDkLn1KUVa94ZwF7Ka";
                ExternalWalletAddress = "3NBQgiFAYGUAhwsP4iAFBqm9oKdv1ZEvjq2";
                ExternalWalletKey = "5MGbbqvUvPWLbBLdfQKyvRwFWLktG1P7nC5UtpzahA4i";
                ExternalWallerAddressContext = "";
                ClientId = "";
                AssetId = "WAVES";
            }
        }

        public class LocalConfig : BlockchainSpecificModel
        {
            public static BlockchainSpecificModel LocalConfigModel()
            {
                var json = File.ReadAllText(Path.Combine(TestContext.CurrentContext.WorkDirectory, "properties.json"));
                return JsonConvert.DeserializeObject<LocalConfig>(json);
            }
        }
    }

    public abstract class BlockchainSpecificModel
    {
        public string BlockchainIntegration { get; set; }
        public string BlockchainApi { get; set; }
        public string BlockchainSign { get; set; }
        public string HotWallet { get; set; }
        public string HotWalletKey { get; set; }
        public string WalletsUrl { get; set; }
        public string ClientId { get; set; }
        public string AssetId { get; set; }
        public string ExternalWalletAddress { get; set; }
        public string ExternalWalletKey { get; set; }
        public string ExternalWallerAddressContext { get; set; }
        public long? BlockchainMiningTime { get; set; }
    }
}
