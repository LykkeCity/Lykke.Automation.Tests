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
                DepositWalletAddress = "msvNWBpFNDQ6JxiEcTFU3xXbSnDir4EqCk";
                DepositWalletKey = "cRTB3eAajJchgNuybhH5SwC9L5PFoTwxXBjgB8vRNJeJ4EpcXmAP";
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
                DepositWalletAddress = "yUDQmubM2HtBFmkvbSK1rER1t57M5Mcvng";
                DepositWalletKey = "cPX3K2xfuzoakmXMaJG5HrdFKuACxegcax5eq55SMHJ8YxvmttZz";
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
                DepositWalletAddress = "tmQjumT79zunETQgFxNTEMUKz8D841fMCf1";
                DepositWalletKey = "cRPW3spyP9riDJWniNpcbDkiBjpLrhneSh2qTs3uSZUbm4HZLEyB";
                ClientId = "b623b171-a307-4485-897c-f3a70b763217";
                AssetId = "ZEC";
            }
        }

        class RippleSettings : BlockchainSpecificModel
        {
            public RippleSettings()
            {
                BlockchainApi = "http://ripple-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://ripple-sign-service.autotests-service.svc.cluster.local/api";
                WalletsUrl = null;
                DepositWalletKey = "snfFfwhb7tobJcamWtH6GhvvhM6pi";
                DepositWalletAddress = "rwAC8DbqubkRcYKuQQaqSBYppHD4JU84Pa";
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
                DepositWalletKey = "SA7W5C7CQOOJF2AGFL2B2LC7VM6WMAONYTAWVITGAKOJM757YDG4VOQG";
                DepositWalletAddress = "GDGZG75SP7UVW6RRDNMFGCFFC5D5RZTXLAWYWUKYCHJ6SBJ2FTXFLXHA";
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
                HotWallet = "GC4QXOYMYXM3PR5UPAJCVUCLVTQVB2OAKF24NFGBU7PERHIB6C4Y4CFR";
                HotWalletKey = "SDV6L7P7OZFKH3JCEBFKFIDKQCPTOMEO2S7V7JBOZB66RTA644I6LQME";
                WalletsUrl = null;
                DepositWalletAddress = "GDF5BLMAZWORE6JROOP6CBI45AA74UEDM6J4PP3XHWNOLIXDUY5UJB5V";
                DepositWalletKey= "SBBULFX24MDPUIOKHTH4JGHYOTVN3DT3GQEAO4TVMHMNHWF65TJK3YYF";
                ClientId = "b623b171-a307-4485-897c-f3a70b763217";
                AssetId = "XLM";
                ExternalWalletAddress = "GCKJYIJQSYDO4AOY2TMTDVKFMHTCDNXVGMBKMC3E2LNDT7KQ4WUJVFX4";
                ExternalWalletKey = "SARHAWQ3DKJJDHGDXHJIZLKR4QSF2WQBZ6BQZLSFHQADU6MROB6X57B4";
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
                DepositWalletKey = "keep_keys_private";
                DepositWalletAddress = "1.2.20477:::23088da2-9a5f-42de-bdd6-fe5ee259c8cd";
                HotWallet = "1.2.20477:::e91757da-6a69-43f4-8283-b2612703af59";
                HotWalletKey = "keep_keys_private";
                ExternalWalletAddress = "1.2.20407:::06eea045-43ee-4cca-a19d-1356abc2b70e";
                ExternalWalletKey = "5JonSsFtX5XD15rBzFCnx46DQMmhWYmSjgJPMPuZGwY9ne86xJ6";
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
                DepositWalletAddress = "xrb_1inxr89m1dgdyrwcktb5ho5p8fto1zn499n6ofpm33qc3skw6qu7ae8ue9gf";
                DepositWalletKey = "42B05779B54815AEFDB3CCB942E98F8262228B13608E8D8C00F4336D25250513";
                HotWallet = "xrb_1ygmfkg4x394rnhpyhafs1qgwpdyiru1f393osig37qzoeiqqhpdxfa7w6kk";
                HotWalletKey = "14DA575835310081121A8F7AA656377E5CBDBB96D8CE8672A564AF902F128FCB";
                ExternalWalletAddress = "xrb_1e9pi3r7qa9qpsas68go55rmu7m8ku9np5pw3roeekx671joz7b1eiu1i8zk";
                ExternalWalletKey = "4CF23CABA08F2C105DE9B80AA5CEBE37CAA3285E015258513F84E41100BDE2A9";
                ExternalWallerAddressContext = "";
                ClientId = "";
                AssetId = "XRB";
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
        public string DepositWalletAddress { get; set; }
        public string DepositWalletKey { get; set; }
        public string ClientId { get; set; }
        public string AssetId { get; set; }
        public string ExternalWalletAddress { get; set; }
        public string ExternalWalletKey { get; set; }
        public string ExternalWallerAddressContext { get; set; }
    }
}
