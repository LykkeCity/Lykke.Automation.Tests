using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.IO;

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

            if (blockchain == "bitcoincashsv")
                _settings = new BitcoinCashSvSettings();

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

            if (blockchain == "ethereumclassic")
                _settings = new EthereumClassicSettings();
            
            if (blockchain == "kin")
                _settings = new KinSettings();

            if (blockchain == "dynamic")
                _settings = new DynamicSettings();

            if (blockchain == "neo")
                _settings = new NeoSettings();

            if (blockchain == "neogas")
                _settings = new NeoGasSettings();

            if (blockchain == "nem")
                _settings = new NemSettings();
            
            if (blockchain == "rootstock")
                _settings = new RootstockSettings();

            if (blockchain == "icon")
                _settings = new IconSettings();

            TestContext.Progress.WriteLine($"propeties.json: {JsonConvert.SerializeObject(_settings)}");

            return _settings;
        }

        class LitecoinSettings : BlockchainSpecificModel
        {
            public LitecoinSettings()
            {
                BlockchainApi = "http://litecoin-api.bcn-autotests.svc.cluster.local/api";
                BlockchainSign = "http://litecoin-sign.bcn-autotests.svc.cluster.local/api";
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
                BlockchainApi = "http://dash-api.bcn-autotests.svc.cluster.local/api";
                BlockchainSign = "http://dash-sign.bcn-autotests.svc.cluster.local/api";
                AssetId = "DASH";
                
                ExternalWalletAddress = "yUxfKGuF2RNDZERju3XHw5Hq7VcPCvfrVM";
                ExternalWalletKey = "cW9HyVvJnZJRMHRWod2Jw2YdsBR7ziXh4oQhx5USAsJHhMmmD2wB";
                
                HotWallet = "yS86Bea1Y9UX4FZMNn8Uhcp1Dd3vqYvWWL";
                HotWalletKey = "cPjbiB55NXBBE3bRexQy3Tw1Gy2RrRt3kXjZTriPhYGodaRAroLW";

                BlockchainMiningTime = 5;
                BaseAmount = 0.0021M;
                BaseAmountWithFee = 0.0022m;
                BuildSignBroadcastEWDW = 40;
                
            }
        }

        class ZcashSettings : BlockchainSpecificModel
        {
            public ZcashSettings()
            {
                BlockchainIntegration = "Zcash";
                BlockchainApi = "http://zcash-api.bcn-autotests.svc.cluster.local/api";
                BlockchainSign = "http://zcash-sign-service.bcn-autotests.svc.cluster.local/api";
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
                BlockchainApi = "http://ripple-api.bcn-autotests.svc.cluster.local/api";
                BlockchainSign = "http://ripple-sign-service.bcn-autotests.svc.cluster.local/api";
                ExternalWalletAddress = "rZtEKAxS4MtZkJnicDZ4xjp3Cxx5aMsVH";
                ExternalWalletKey = "ss2ySSW5YCM79oknv7tfeu6ozdL73";
                HotWallet = "rLE4YpGHFEcavni2rSzzSGcu8GZjgMxC73";
                HotWalletKey = "shoH8PDxgWyZgQ2t6DbBEU7vxLabg";
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
                BlockchainApi = "http://stellar-api.bcn-autotests.svc.cluster.local/api";
                BlockchainSign = "http://stellar-sign.bcn-autotests.svc.cluster.local/api";
                //BlockchainApi = "http://localhost:5000/api";
                //BlockchainSign = "http://localhost:5002/api";
                HotWallet = "GBQ5KWCH7WIWPF4DFDGBQFDTAZKTNVSXNZO54XC6NPYN747KP3Y5CDEE";
                HotWalletKey = "SDV3KQ2ACQNZDIVMGWTABRHIS7MNGODCGMB2UFQYDGUH625CGPTK3ILI";
                AssetId = "XLM";
                AssetAccuracy = 7;
                ExternalWalletAddress = "GALZEEA3D5C7UODT376CIJHGV52IEOHZJVDQ6SICJSGTBQP47Q5M2L5V";
                ExternalWalletKey = "SDZ3F5GDXJIHH2S4C22FKX257MRHQCPMLXPUZASBQHOXERYPRVHNWK37";
                BlockchainMiningTime = 3;
            }
        }

        class BitsharesSettings : BlockchainSpecificModel
        {
            public BitsharesSettings()
            {
                BlockchainIntegration = "Bitshares";
                BlockchainApi = "http://bitshares-api.bcn-autotests.svc.cluster.local/api";
                BlockchainSign = "http://bitshares-sign.bcn-autotests.svc.cluster.local/api";
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
                BlockchainApi = "http://bitcoincash-api.bcn-autotests.svc.cluster.local/api";
                BlockchainSign = "http://bitcoincash-sign.bcn-autotests.svc.cluster.local/api";
                HotWallet = "mnKfRMF5R5x3qY2sNkUymhaj5WSNNHB9EF";
                HotWalletKey = "cUXBjJxK5KNf8xvx3UYKZi1WASGPYqZLjBsat7spFY48QHezJRmR";
                ExternalWalletAddress = "mnXmzoZEN8jc3xn5WJXwWV9jc4P9xeLi2C";
                ExternalWalletKey = "cMmDCGs1TKLf3VUBSiULr7Pj4xsiWyNUnMpUMdxrTZtb174fLfaS";
                ExternalWallerAddressContext = "";
                AssetId = "BCH";
                BuildSignBroadcastEWDW = 30;
            }
        }

        public class DynamicSettings : BlockchainSpecificModel
        {
            public DynamicSettings()
            {
                BlockchainIntegration = "Dynamic";
                BlockchainApi = "http://dynamic-api.bcn-autotests.svc.cluster.local/api";
                BlockchainSign = "http://dynamic-sign.bcn-autotests.svc.cluster.local/api";
                HotWallet = "DU7GMsQKTE7RBXqjNcGFQVYmKX1bms94As"; //"DS7BHYT6UEP86AsjXy8qxcxKQLc1eNLcdq";
                HotWalletKey = "MmapGVkWYsQEo1oEqWGyY5ydzhr1QMxrNDvPHWU2XKfwsAKQHMzQ"; //"QTEZi6Vv3XuCa6rs5YmYHyRPA61V3R7hqJqWbcY3WhGYh4sxbXxz";
                ExternalWalletAddress = "DPfsWEPkcicRfC6vYt8c62NC5HPTBWFe68"; //"DNMkbse4GY4wygwVQtJZdtGuuJ9orz26wD";
                ExternalWalletKey = "MrWGHVZAm5KpHwMGApPnJRCLw7bLsC9MWuRuqjxG7UFpc3Cebogy"; //"QVafiaf51gf7peRFfmviitHKNu9o3PwqnV6v6W8nq3t4g2SLBNNg";
                ExternalWallerAddressContext = "";
                AssetId = "DYN";
                BuildSignBroadcastEWDW = 30;
                BlockchainMiningTime = 30;
                BaseAmount = 0.002M;
                BaseAmountWithFee = 0.003M;
            }
        }

        public class BitcoinCashSvSettings : BlockchainSpecificModel
        {
            public BitcoinCashSvSettings()
            {
                BlockchainIntegration = "BitcoinCashSv";
                BlockchainApi = "http://bitcoincashsv-api.bcn-autotests.svc.cluster.local/api";
                BlockchainSign = "http://bitcoincashsv-sign.bcn-autotests.svc.cluster.local/api";
                HotWallet = "mo2NRoxmmDk5op65gFhUaatM3YkNBQU46e";
                HotWalletKey = "cNYf8ghMpQ5LhEjZY7vaawf4HumDntdbh76QZXgwkjQPMwgbZj9Z";
                ExternalWalletAddress = "muN2yJgd4xoAZGsQ6VrNT6S8uqv4wcznwW";
                ExternalWalletKey = "cV5nmu7gdQJAPyS43puNnv88FHrbakDdWPQreg2RTSabJJT3cBRc";
                ExternalWallerAddressContext = "";
                AssetId = "BCHSV";
                BuildSignBroadcastEWDW = 30;
            }
        }

        public class BitcoinGoldSettings : BlockchainSpecificModel
        {
            public BitcoinGoldSettings()
            {
                BlockchainIntegration = "BitcoinGold";
                BlockchainApi = "http://bitcoingold-api.bcn-autotests.svc.cluster.local/api";
                BlockchainSign = "http://bitcoingold-sign.bcn-autotests.svc.cluster.local/api";
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

                BlockchainApi = "http://dcr-api.bcn-autotests.svc.cluster.local/api";
                BlockchainSign = "http://dcr-signservice.bcn-autotests.svc.cluster.local/api";
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
                BlockchainApi = "http://eos-api.bcn-autotests.svc.cluster.local/api";
                BlockchainSign = "http://eos-signservice.bcn-autotests.svc.cluster.local/api";
                AssetId = "EOS";
                AssetAccuracy = 4;
                HotWallet = "lykketest111";
                HotWalletKey = "5JQwrpty3wrET2EqF91bbevU7zqS8GdaCu82w7NTGxW6UTpBUg1";
                ExternalWalletAddress = "lykketest222";
                ExternalWalletKey = "5JHW3H4DMB2iknvZrxWub69aUaNk16qNghWcBiPAeSLDRZ1peUv";
                BlockchainMiningTime = 10;
            }
        }

        class BitcoinSettings : BlockchainSpecificModel
        {
            public BitcoinSettings()
            {
                BlockchainIntegration = "Bitcoin";
                BlockchainApi = "http://bitcoin-api.bcn-autotests.svc.cluster.local/api";
                BlockchainSign = "http://bitcoin-sign.bcn-autotests.svc.cluster.local/api";
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
                BlockchainApi = "http://qtum-api.bcn-autotests.svc.cluster.local/api";
                BlockchainSign = "http://qtum-sign.bcn-autotests.svc.cluster.local/api";
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
                BlockchainApi = "http://steem-api.bcn-autotests.svc.cluster.local/api";
                BlockchainSign = "http://steem-signservice.bcn-autotests.svc.cluster.local/api";
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
                AssetAccuracy = 15;
                BaseAmount = 0.3150000001m;
                BaseAmountWithFee = 0.63000001m;
            }
        }
        
        class EthereumClassicSettings : BlockchainSpecificModel
        {
            public EthereumClassicSettings()
            {
                BlockchainIntegration = "EthereumClassic";
                BlockchainApi = "http://etc-api.bcn-autotests.svc.cluster.local/api";
                BlockchainSign = "http://etc-signservice.bcn-autotests.svc.cluster.local/api";
                BlockchainMiningTime = 5;
                HotWallet = "0xec31eafd3818fbd7c14a423ee09090351d864ba1";
                HotWalletKey = "0x2df14f93b65c7fbe6623aef1f09183d704f7f24ad2d615eaecc9fd44f725b269";
                AssetId = "ETC";
                ExternalWalletAddress = "0x085e36cc33743be10246013d11b8fc1919a242db";
                ExternalWalletKey = "0x2c0b8b8f59e2ed253bb45c0974852cae982416823eb4f6775192f167e7b15825";
                BuildSignBroadcastEWDW = 1;
                AssetAccuracy = 16;
            }
        }
        
        class KinSettings : BlockchainSpecificModel
        {
            public KinSettings()
            {
                BlockchainIntegration = "Kin";
                BlockchainApi = "http://kin-api.bcn-autotests.svc.cluster.local/api";
                BlockchainSign = "http://kin-sign-service.bcn-autotests.svc.cluster.local/api";
                HotWallet = "GBHU4Y36P3CBFIHNRLBQX77FL2AL3ZUM2JWBJOKABEVYPZS2DXQU76XP";
                HotWalletKey = "SAFPNCO2QXUELIBSBEGFTC6EKVZKEJJXBJHBO34NJZEQP3P2KQPJCFGQ";
                AssetId = "KIN";
                BaseAmount = 1;
                BaseAmountWithFee = 3;
                ExternalWalletAddress = "GA2JRV7KOKFFKLCOBRD52KHMFRKWP2JZKQU5UVASOHEC7AUT6SQMXRIH";
                ExternalWalletKey = "SDYKPMGFCY2KXCVVMQRJT6QJMYTLNBI2XW6PYP54RJCNSHUBFG732DTL";
            }
        }

        class NeoSettings : BlockchainSpecificModel
        {
            public NeoSettings()
            {
                BlockchainIntegration = "Neo";
                BlockchainApi = "http://neo-api.bcn-autotests.svc.cluster.local/api";
                BlockchainSign = "http://neo-sign.bcn-autotests.svc.cluster.local/api";
                HotWallet = "AHcpWnhDuNmHu2oidfp6czjwUYTMmBMgM9";
                HotWalletKey = "L4GhVsEAiEFhf7FDPrHGZz6EjjYCzMpJpvoNVPHXP2PToedkfqwM";
                AssetId = "Neo";
                ExternalWalletAddress = "AKZne44EVeuqvrq2aYaxZkoytT9SyhB21D";
                ExternalWalletKey = "L3Cy53zR75sAr2RcKmiP5hUajx2Wtzkgm8qePKaRrzAGpeLjTjeV";
                BuildSignBroadcastEWDW = 30;
                AssetAccuracy = 0;
                BaseAmount = 1;
                BaseAmountWithFee = 2;
                IsIncludeFee = false;
            }
        }

        class NeoGasSettings : BlockchainSpecificModel
        {
            public NeoGasSettings()
            {
                BlockchainIntegration = "Gas";
                BlockchainApi = "http://neo-api.bcn-autotests.svc.cluster.local/api";
                BlockchainSign = "http://neo-sign.bcn-autotests.svc.cluster.local/api";
                HotWallet = "APLgnEwjrACiPBHQ1B4HX1yG48C2Kcx1Z7";
                HotWalletKey = "KxagYyVtdaq1c2NgD9JgSjDyn1eyPcog5mLJRK2rapGmkgy4YFQP";
                AssetId = "Gas";
                ExternalWalletAddress = "AJgum3HvPQFLHUvan62s7UqTsZ6eTKgB1c";
                ExternalWalletKey = "L3Wi7GzVxtmQdypHZcahYRsTkshQsiXigCjUSZWGCrengfwQNzim";
                BuildSignBroadcastEWDW = 30;
                AssetAccuracy = 8;
                IsIncludeFee = false;
            }
        }

        class NemSettings : BlockchainSpecificModel
        {
            public NemSettings()
            {
                BlockchainIntegration = "Nem";
                BlockchainApi = "http://nem-api.bcn-autotests.svc.cluster.local/api";
                BlockchainSign = "http://nem-signservice.bcn-autotests.svc.cluster.local/api";
                HotWallet = "TBEUNFVOS2SMZLADUDE5FID3FRLCKBVI22EJFUIE";
                HotWalletKey = "3BB74D5BA33483D4F0B7896DB8E40BB510B6CBA568F3192674092957CE7C8E89";
                AssetId = "nem:xem";
                ExternalWalletAddress = "TCBPVIDJRSXNFEZ5UWFAEKVTXX73FWNS7CTZMLD2";
                ExternalWalletKey = "BEC2E62007860D5573476EC98941F9473A45BA0BEFAB9C570CCD7C08DDD5BDA0";
                AssetAccuracy = 6;
                BlockchainMiningTime = 30;
            }
        }
        
        class RootstockSettings : BlockchainSpecificModel
        {
            public RootstockSettings()
            {
                BlockchainIntegration = "Rootstock";
                BlockchainApi = "http://rbtc-api.bcn-autotests.svc.cluster.local/api";
                BlockchainSign = "http://rbtc-signservice.bcn-autotests.svc.cluster.local/api";
                BlockchainMiningTime = 5;
                HotWallet = "0x8251d72e0c0b4e5d4c3787eae851c01445147efb";
                HotWalletKey = "0x52a48c911e9e87c2ba7a567b729ac4815b4823dbda742b90d65dc44a7b4b26e0";
                AssetId = "RBTC";
                ExternalWalletAddress = "0x1fd443b40bbc3f5d23da3c4d651e64cdb8e20a9f";
                ExternalWalletKey = "0x6e5ef59ea5941c2b9119b17521fb81f4fbdebf8ca9a13ccde0adfab80c607445";
                BuildSignBroadcastEWDW = 1;
                AssetAccuracy = 18;
                BaseAmount = 0.00000252m;
                BaseAmountWithFee = 0.00000378m;
            }
        }

        class IconSettings : BlockchainSpecificModel
        {
            public IconSettings()
            {
                BlockchainIntegration = "Icon";
                BlockchainApi = "http://icon-api.bcn-autotests.svc.cluster.local/api";
                BlockchainSign = "http://icon-signservice.bcn-autotests.svc.cluster.local/api";
                HotWallet = "hxe8db3bc33564f5b07cd52e37e2d762d3073a2c1d";
                HotWalletKey = "0x0097c4f4d729383129fa305616e95ba3aca86f8ddb069cadeab4ae9c3d4f57fa3d";
                AssetId = "ICX";
                ExternalWalletAddress = "hxa0c8aad540a8b85175b59d98585e0f79ffbc2502";
                ExternalWalletKey = "0x00847549f225a48531c16b89ac6057734e07cc174f43dd9d8616014ea6c1044e70";
                AssetAccuracy = 18;
                MaxWalletsForCashIn = 5;
                BaseAmount = 0.008m;
                BaseAmountWithFee = 0.009m;
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
        public decimal? BaseAmount { get; set; }
        public decimal? BaseAmountWithFee { get; set; }
        public bool? IsIncludeFee { get; set; }
        public bool? SkipHistoryTests { get; set; }
        public string GetBalancesTakeValue { get; set; }
    }
}
