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

            if (blockchain == "bitshares")
                _settings = new BitsharesSettings();

            if (blockchain == "raiblocks")
                _settings = new RaiBlocksSettings();

            if(blockchain == "waves")
                _settings = new WavesSettings();

            if(blockchain == "bitcoincash")
                _settings = new BitcoinCashSettings();

            if (blockchain == "bitcoingold")
                _settings = new BitcoinGoldSettings();

            if (blockchain == "monero")
                _settings = new MoneroSettings();

            if (blockchain == "decred")
                _settings = new DecredSettings();

            if (blockchain == "eos")
                _settings = new EosSettings();

            if (blockchain == "bitcoin")
                _settings = new BitcoinSettings();

            if (blockchain == "qtum")
                _settings = new QtumSettings();

            if (blockchain == "steem")
                _settings = new SteemSettings();
            
            if (blockchain == "ethereum")
                _settings = new EthereumSettings();

            TestContext.Progress.WriteLine($"propeties.json: {JsonConvert.SerializeObject(_settings)}");

            return _settings;
        }

        class LitecoinSettings : BlockchainSpecificModel
        {
            public LitecoinSettings()
            {
                BlockchainApi = "http://litecoin-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://litecoin-sign.autotests-service.svc.cluster.local/api";
                AssetId = "LTC";
                ExternalWalletAddress = "QiKk2wXBuVEARMUkGqKQFyjm1nJayAU8ZC";
                ExternalWalletKey = "cSARwquJjL3wcaSZhke5u48UkLF7zDJChrHWaLYnWegpQXjER1fZ";
                ExternalWallerAddressContext ="{\"PubKey\":\"02d02b90654309b5c12abf0789fbc8a113fdd238f863b5692ac9c7b3fa909be34c\"}";

                HotWallet = "QX8BHEUPhKhPgo26QrrvY84FUzpKuhX1Uq";
                HotWalletKey = "cSS6VUdFTuKfr4uFPT9Vy6CCU5SHsTEKRDNrzEaHQZRrVb3eQUi3";
                HotWalletAddressContext = "{\"PubKey\":\"03584f2d9dac279c2a85e58e97f67fc6559a60d950ba30cc4cee544c2ef8457ba1\"}";
                BlockchainMiningTime = 20;
                BlockchainIntegration = "LiteCoin";
                MaxWalletsForCashIn = 10;
            }
        }

        class DashSettings : BlockchainSpecificModel
        {
            public DashSettings()
            {
                BlockchainIntegration = "DASH";
                BlockchainApi = "http://dash-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://dash-sign.autotests-service.svc.cluster.local/api";
                AssetId = "DASH";
            }
        }

        class ZcashSettings : BlockchainSpecificModel
        {
            public ZcashSettings()
            {
                BlockchainIntegration = "Zcash";
                BlockchainApi = "http://zcash-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://zcash-sign-service.autotests-service.svc.cluster.local/api";
                AssetId = "ZEC";
                HotWallet = "tmCd9qu9ffg2LX1Hsc87gNBFHrQH1F994aF";
                HotWalletKey = "cU5b5pAnSBE4DoUtTVMQx5U8RTd3SaXjmVPSN8afDXCvGNayYKqa";
                ExternalWalletAddress = "tmL4JCMEFQW2YtxQptLbBJtH6dzowHouyxw";
                ExternalWalletKey = "cVWdihupzUy3GyP5bha15Dk1W1ejbETBngMV51xATMJzr4Z6fnRk";
                BlockchainMiningTime = 30;
            }
        }

        class RippleSettings : BlockchainSpecificModel
        {
            public RippleSettings()
            {
                BlockchainIntegration = "Ripple";
                BlockchainApi = "http://ripple-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://ripple-sign-service.autotests-service.svc.cluster.local/api";
                ExternalWalletAddress = "rJ2zMCyShMsjHf7tHptVy7qCWt6dYv8B7r";
                ExternalWalletKey = "sndT9kLXjNv7L1HWfr6Lo5N6pmEvU";
                HotWallet = "rG1Zu2dm2Ty9pQrnGJux1RuKZA6qhjWwMc";
                HotWalletKey = "ss91FvLcTaKNBAMtyfN2X3izKhNux";
                AssetId = "XRP";
                AssetAccuracy = 6;
                SignExpiration = 90;
            }
        }

        class StellarSettings : BlockchainSpecificModel
        {
            public StellarSettings()
            {
                BlockchainIntegration = "Stellar";
                BlockchainApi = "http://stellar-api-schnidlo.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://stellar-sign-service-schnidlo.autotests-service.svc.cluster.local/api";
                HotWallet = "GCCWY6MNVWORHO7B3L6W5LULGFR337K5UAATA2Z3FSPQA5MYRW5X33ZP";
                HotWalletKey = "SBBEPWMAINBQKQFW42L6GTWF73WPP5VRVG6KZHCWZVSVM4DUMWZESK2Z";
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
                HotWallet = "1.2.20477:e91757da-6a69-43f4-8283-b2612703af59";
                HotWalletKey = "5JdCrsRQrW1nTgDjTEeVXt7AQ95Kre9w6TQqAz4dKmEF4k8RfLk";
                ExternalWalletAddress = "1.2.22396:06eea045-43ee-4cca-a19d-1356abc2b70e";
                ExternalWalletKey = "5J5sD5KNh4ArR1VdJNyge2oiE9vSMs2A7VYaJQULUagHrMBXfcj";
                AssetId = "1.3.0";
                ExternalWallerAddressContext = "5KJBVnJaiYhVq7x3mF47f5xd6RUisnqjWCdc5fx9uhWSDdrd1MR";
                SignExpiration = 90;
            }
        }

        public class RaiBlocksSettings : BlockchainSpecificModel
        {
            public RaiBlocksSettings()
            {
                BlockchainIntegration = "RaiBlocks";
                BlockchainApi = "http://raiblocks-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://raiblocks-sign.autotests-service.svc.cluster.local/api";
                HotWallet = "xrb_3h4grchtxu6j1c3t9bhkh8wxgri1j8dsgp7ra3rkajexy7wszs49kwiuoc5z";
                HotWalletKey = "1683C72CD811D066F86CA03873845EDAABA2BE1658179A5B76C699FBF9548156";
                ExternalWalletAddress = "xrb_1e9pi3r7qa9qpsas68go55rmu7m8ku9np5pw3roeekx671joz7b1eiu1i8zk";
                ExternalWalletKey = "4CF23CABA08F2C105DE9B80AA5CEBE37CAA3285E015258513F84E41100BDE2A9";
                ExternalWallerAddressContext = "";
                AssetId = "XRB";
                BlockchainMiningTime = 10;
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
                AssetId = "WAVES";
                BlockchainMiningTime = 25;
            }
        }

        public class BitcoinCashSettings : BlockchainSpecificModel
        {
            public BitcoinCashSettings()
            {
                BlockchainIntegration = "BitcoinCash";
                BlockchainApi = "http://bitcoincash-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://bitcoincash-sign.autotests-service.svc.cluster.local/api";
                HotWallet = "mnKfRMF5R5x3qY2sNkUymhaj5WSNNHB9EF";
                HotWalletKey = "cUXBjJxK5KNf8xvx3UYKZi1WASGPYqZLjBsat7spFY48QHezJRmR";
                ExternalWalletAddress = "mnXmzoZEN8jc3xn5WJXwWV9jc4P9xeLi2C";
                ExternalWalletKey = "cMmDCGs1TKLf3VUBSiULr7Pj4xsiWyNUnMpUMdxrTZtb174fLfaS";
                ExternalWallerAddressContext = "";
                AssetId = "BCH";
            }
        }

        public class BitcoinGoldSettings : BlockchainSpecificModel
        {
            public BitcoinGoldSettings()
            {
                BlockchainIntegration = "BitcoinGold";
                BlockchainApi = "http://bitcoingold-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://bitcoingold-sign.autotests-service.svc.cluster.local/api";
                HotWallet = "2MyuBCFAEhiAgN33N9RH1Hg4Zvsauuz8s2u";
                HotWalletKey = "cMy8jYimCPqVsanrx28uCZJqi6Qmx2tkyiKyUDQsErwpX6s4tKAg";
                HotWalletAddressContext = "{\"PubKey\":\"0399a51d0fb3e8b92c40f3859b252b142f5c818042f32f9ed6f23e269c3b9e2cfb\"}";
                ExternalWalletAddress = "2N6qkRBg7sVoygCgs2s7gJoSpGgFPQdbAHC";
                ExternalWalletKey = "cUqJUTKQayhJb5VfHhyNnw6Xb8vGrn9ccbP6goMPyEVv2j1maeXo";
                ExternalWallerAddressContext = "{\"PubKey\":\"0373d33c98cf7c2259e62194eb4937e5fcbf92f911e667320cd1741c86267087f8\"}";
                AssetId = "BTG";
                BlockchainMiningTime = 40;
            }
        }

        public class MoneroSettings : BlockchainSpecificModel
        {
            public MoneroSettings()
            {
                BlockchainIntegration = "Monero";
                BlockchainApi = "http://monero-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://monero-sign.autotests-service.svc.cluster.local/api";
                HotWallet = "9wytGvzosJ73Jvy5AvwkMSeQvocaWzjDTammvDZBLmb24KvkCyxoxAbMvJ8zrqGvMDjkJSjMCGtKJMFHzcVRmJMGPVjQ4FP";
                HotWalletKey = "ea7581947da5c6f27203531322830fd40f2e89ebc6729ca7a4ffda2b029e6708";
                HotWalletAddressContext = "";
                ExternalWalletAddress = "9wytGvzosJ73Jvy5AvwkMSeQvocaWzjDTammvDZBLmb24KvkCyxoxAbMvJ8zrqGvMDjkJSjMCGtKJMFHzcVRmJMGPVjQ4FP";
                ExternalWalletKey = "ea7581947da5c6f27203531322830fd40f2e89ebc6729ca7a4ffda2b029e6708";
                ExternalWallerAddressContext = "";
                AssetId = "XMR";
                BlockchainMiningTime = 7;
            }
        }
        public class DecredSettings : BlockchainSpecificModel
        {
            public DecredSettings()
            {
                BlockchainIntegration = "Decred";

                BlockchainApi = "http://lykke-service-decred-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://lykke-service-decred-sign.autotests-service.svc.cluster.local/api";
                HotWallet = "TsgbAonAUgtgGhTdi6qYwUDcL45ZuNnMFuK";
                HotWalletKey = "PtWUCdaSMrJ2et41GdUm125kPmN5n9gxKdVfm5ZLkcLeuJFVpuN8m";
                ExternalWalletAddress = "TsjGG5ybrDLeoeAvkxbZLVHyzBYr27Pb13W";
                ExternalWalletKey = "PtWUd45tMVbhvdWRubAutbWrrm5gfr7iA1fhi37n1xPeytYRHaG6N";
                ExternalWallerAddressContext = "";
                AssetId = "DCR";
                BlockchainMiningTime = 10;
            }
        }

        class EosSettings : BlockchainSpecificModel
        {
            public EosSettings()
            {
                BlockchainIntegration = "Eos";
                BlockchainApi = "http://eos-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://eos-signservice.autotests-service.svc.cluster.local/api";
                AssetId = "EOS";
                AssetAccuracy = 4;
                HotWallet = "herb";
                HotWalletKey = "5JcjhSQzfTUnQUXKCZupbBsGj6mkmTRxfzbMxiGBLxw9JjzXo66";
                ExternalWalletAddress = "insect";
                ExternalWalletKey = "5JKyVNU65er9y5xLKprVPcdYzqULkMwDKWDX6zokgX5dAAj7dVv";
                BlockchainMiningTime = 10;
            }
        }

        class BitcoinSettings : BlockchainSpecificModel
        {
            public BitcoinSettings()
            {
                BlockchainIntegration = "Bitcoin";
                BlockchainApi = "http://bitcoin-bil-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://bitcoin-bil-sign.autotests-service.svc.cluster.local/api";
                HotWallet = "2MwsEPnRBsYcqqvLSHkKodFEmn4sotLikf3";
                HotWalletKey = "cNfGePxvd13CFh5iGwUWStkJ2tLefs8Q1ZQipKc9XL198bYrFjz3";
                HotWalletAddressContext = "{\"PubKey\":\"02952dce2922b6e3638d9118541e2b2d50765d35691a8d4885396278f6f6d4a4cc\"}";
                AssetId = "BTC";
                ExternalWalletAddress = "2N3iTtRKvqqj1U77hLs9cEjttsXLK32zZPY";
                ExternalWalletKey = "cQG5HXZW2viTFDPsyPHS73fNN7gVJYmxPVQsMYG4j1XNWzD2fU6Z";
                ExternalWallerAddressContext = "{\"PubKey\":\"03dd1001f94328682857ac038d7ab5e2d97ad7cdeab07483de6eb6d678df5bfa42\"}";
                BlockchainMiningTime = 20;
                MaxWalletsForCashIn = 10;
            }
        }

        class QtumSettings : BlockchainSpecificModel
        {
            public QtumSettings()
            {
                BlockchainIntegration = "QTUM";
                BlockchainApi = "http://qtum-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://qtum-sign.autotests-service.svc.cluster.local/api";
                HotWallet = "qg3uFoqA5nFFvx4N6avNUaRw6ppdRpXfwg";
                HotWalletKey = "cUfNTJiPcsCbj4RdxLWE1vnBBn8k5fwem14JBSBnSWoAzKhTzGVa";
                HotWalletAddressContext = "";
                ExternalWalletAddress = "qL6XDDTKmoPYeK6QLq83tosygptN85Sehd";
                ExternalWalletKey = "cNXQfnthkJfBzzntW4Hq2EL4E43GRXjjqx66Eh5JKRcUY999KEC9";
                ExternalWallerAddressContext = "";
                AssetId = "QTUM";
                BlockchainMiningTime = 10;
                BuildSignBroadcastEWDW = 21;
            }
        }

        class SteemSettings : BlockchainSpecificModel
        {
            public SteemSettings()
            {
                BlockchainIntegration = "Steem";
                BlockchainApi = "http://steem-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://steem-signservice.autotests-service.svc.cluster.local/api";
                AssetId = "STEEM";
                AssetAccuracy = 3;
                HotWallet = "lykke.dev.test01";
                HotWalletKey = "5JavWA9FhGqijcWbKmjxoFDkMTSNteSQRcqHhtMCiRMjp7pQ7f2";
                ExternalWalletAddress = "lykke.dev.test02";
                ExternalWalletKey = "5HrwjF51dAbAPJNUy6oFqnjKmcJWVkFpqyJMocBycbn4Sd5Eiai";
                BlockchainMiningTime = 10;
            }
        }

        class EthereumSettings : BlockchainSpecificModel
        {
            public EthereumSettings()
            {
                BlockchainIntegration = "Ethereum";
                BlockchainApi = "http://ethereum-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://ethereum-signapi.autotests-service.svc.cluster.local/api";
                BlockchainMiningTime = 5;
                HotWallet = "0xec31eafd3818fbd7c14a423ee09090351d864ba1";
                HotWalletKey = "0x2df14f93b65c7fbe6623aef1f09183d704f7f24ad2d615eaecc9fd44f725b269";
                AssetId = "ETH";
                ExternalWalletAddress = "0x085e36cc33743be10246013d11b8fc1919a242db";
                ExternalWalletKey = "0x2c0b8b8f59e2ed253bb45c0974852cae982416823eb4f6775192f167e7b15825";
                BuildSignBroadcastEWDW = 1;
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
        public string HotWalletAddressContext { get; set; }
        public string AssetId { get; set; }
        public byte? AssetAccuracy { get; set; }
        public string ExternalWalletAddress { get; set; }
        public string ExternalWalletKey { get; set; }
        public string ExternalWallerAddressContext { get; set; }
        public long? BlockchainMiningTime { get; set; }
        public long? MaxWalletsForCashIn { get; set; }
        public long? SignExpiration { get; set; }
        public long? BuildSignBroadcastAttemptCount { get; set; }
        public long? RebuildAttemptCount { get; set; }
        public long? BuildSignBroadcastEWDW { get; set; }
    }
}
