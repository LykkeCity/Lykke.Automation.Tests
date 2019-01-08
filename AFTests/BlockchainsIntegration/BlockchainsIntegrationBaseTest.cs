using BlockchainsIntegration.BlockchainWallets;
using BlockchainsIntegration.Api;
using BlockchainsIntegration.Sign;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.Tests;
using Newtonsoft.Json;
using System.IO;
using AFTests.BlockchainsIntegration;
using XUnitTestCommon.TestsCore;
using System.Linq;
using BlockchainsIntegration.Models;
using Lykke.Client.AutorestClient.Models;
using System.Net;
using XUnitTestCommon.ServiceSettings;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using XUnitTestCommon.RestRequests.Interfaces;
using XUnitTestCommon.RestRequests;

namespace AFTests.BlockchainsIntegrationTests
{
    [NonParallelizable]
    class BlockchainsIntegrationBaseTest : BaseTest
    {
        private static object _lock = new object();
        private static Queue<WalletCreationResponse> result = null;

        private static Lazy<BlockchainSpecificModel> _currentSettings
        {
            get
            {
                return new Lazy<BlockchainSpecificModel>
                                    (BlockchainSpecificSettingsFactory.BlockchainSpecificSettings(SpecificBlockchain()));
            }
        }

       protected static string SpecificBlockchain()
       {
            return Environment.GetEnvironmentVariable("BlockchainIntegration") ?? "kin"; //"monero"; //"RaiBlocks";//"bitshares";// "stellar-v2";//"Zcash"; //"Ripple";// "Dash"; "Litecoin";
        }

        #region test values

        protected static string proxyApi = "http://api.autotest-proxy.svc.cluster.local";
        protected static string blockchainCGI = "http://api.autotest-proxy.svc.cluster.local/cgi-bin2/dash.cgi";

        protected static string CurrentBlockchainCGI { get { return $"http://api.autotest-proxy.svc.cluster.local/cgi-bin2/{BlockChainName.ToLower()}.cgi"; } }

        public static void EnableBLockchainProxy()
        {
            Requests.For(CurrentBlockchainCGI).Get("").Build().Execute();
        }

        public static void DisableBLockchainProxy()
        {
            var BlockchainName1 = "ripple";
            var BlockchainName2 = "zcash";
            var cgiUrl = $"http://api.autotest-proxy.svc.cluster.local/cgi-bin2/{BlockchainName1.ToLower()}.cgi";

            if(BlockChainName.ToLower() == BlockchainName1)
                cgiUrl = $"http://api.autotest-proxy.svc.cluster.local/cgi-bin2/{BlockchainName2.ToLower()}.cgi";

            Requests.For(cgiUrl).Get("").Build().Execute();
        }

        protected static string BlockchainApi { get { return _currentSettings.Value.BlockchainApi; } }
        protected static BlockchainApi blockchainApi = new BlockchainApi(BlockchainApi);
        protected static BlockchainSign blockchainSign = new BlockchainSign(_currentSettings.Value.BlockchainSign);
        protected BlockchainWallets blockchainWallets = new BlockchainWallets();
        protected static string HOT_WALLET = _currentSettings.Value.HotWallet;
        protected static string HOT_WALLET_KEY = _currentSettings.Value.HotWalletKey;
        protected static string HOT_WALLET_CONTEXT = _currentSettings.Value.HotWalletAddressContext;
        protected static string BlockChainName = _currentSettings.Value.BlockchainIntegration;
        protected static string ASSET_ID = _currentSettings.Value.AssetId;
        protected static byte ASSET_ACCURACY = _currentSettings.Value.AssetAccuracy ?? 8;
        protected static string EXTERNAL_WALLET = _currentSettings.Value.ExternalWalletAddress;
        protected static string EXTERNAL_WALLET_KEY = _currentSettings.Value.ExternalWalletKey;
        protected static string EXTERNAL_WALLET_ADDRESS_CONTEXT = _currentSettings.Value.ExternalWallerAddressContext;
        private static double BASE_AMOUNT = _currentSettings.Value.BaseAmount ?? 0.20000001;
        private static double BASE_AMOUNT_WITH_FEE = _currentSettings.Value.BaseAmountWithFee ?? 0.29000001;
        protected static string AMOUNT = Convert.ToInt64(BASE_AMOUNT * Math.Pow(10, ASSET_ACCURACY)).ToString();
        protected static string AMOUT_WITH_FEE = Convert.ToInt64(BASE_AMOUNT_WITH_FEE * Math.Pow(10, ASSET_ACCURACY)).ToString();
        protected static long BLOCKCHAIN_MINING_TIME = _currentSettings.Value.BlockchainMiningTime ?? 10;
        protected static long MAX_WALLETS_FOR_CASH_IN = _currentSettings.Value.MaxWalletsForCashIn ?? 30;
        protected static long SIGN_EXPIRATION_SECONDS = _currentSettings.Value.SignExpiration ?? 0;
        protected static long REBUILD_ATTEMPT_COUNT = _currentSettings.Value.RebuildAttemptCount ?? 5;
        protected static long BUILD_SIGN_BROADCAST_EWDW = _currentSettings.Value.BuildSignBroadcastEWDW ?? 5;
        protected static bool SKIP_HISTORY_TESTS = _currentSettings.Value.SkipHistoryTests ?? true;

        protected static bool INCLUDE_FEE
        {
            get
            {
                if (_currentSettings.Value.IsIncludeFee.HasValue)
                    return _currentSettings.Value.IsIncludeFee.Value;
                var includeFee = true;

                try
                {
                    var extensionReponse = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired;

                    if (extensionReponse.HasValue)
                        includeFee = !extensionReponse.Value;
                } catch(Exception e)
                {
                    TestContext.Progress.WriteLine($"Exception in request for Public address Extension: {e.Message}. {e.StackTrace} ");
                }
                return includeFee;
            }
        }

        #endregion

        [SetUp]
        public void SetUp()
        {
            TestContext.Progress.WriteLine($"Started test {TestContext.CurrentContext.Test.Name}");
        }

        [OneTimeTearDown]
        public void SetProperty()
        {
            AllurePropertiesBuilder.Instance.AddPropertyPair("Date", DateTime.Now.ToString());
            try
            {
                var isAlive = blockchainApi.IsAlive.GetIsAlive().GetResponseObject();
                AllurePropertiesBuilder.Instance.AddPropertyPair("Env", isAlive.Env);
                AllurePropertiesBuilder.Instance.AddPropertyPair("Name", isAlive.Name);
                AllurePropertiesBuilder.Instance.AddPropertyPair("Version", isAlive.Version);
                AllurePropertiesBuilder.Instance.AddPropertyPair("ContractVersion", isAlive.ContractVersion);
            }
            catch (Exception) { /*do nothing*/}

            new Allure2Report().CreateEnvFile();
        }

        #region Helpers methods

        private static bool skipAddBalanceFromEWToDWAfterError = false;

        protected Queue<WalletCreationResponse> Wallets(long wallets  = 30)
        {
            lock (_lock)
            {
                if (result == null)
                {
                    result = new Queue<WalletCreationResponse>();

                    long maxWallets = wallets;
                    while(maxWallets > 0)
                    {
                        var cycleWallets = new Queue<WalletCreationResponse>();

                        for (var i = 0; i < Math.Min(MAX_WALLETS_FOR_CASH_IN, maxWallets); i++)
                        {
                            var wallet = blockchainSign.PostWallet();
                            if (wallet.StatusCode != HttpStatusCode.OK)
                                throw new Exception($"Cant create wallet. Got: {wallet.StatusCode}.  {wallet.Content}");
                            cycleWallets.Enqueue(wallet.GetResponseObject());
                        }

                        cycleWallets.ToList().ForEach(w => result.Enqueue(w));

                        if (!SetBalanceWIthManyOutputs(cycleWallets.ToList()))
                        {
                            cycleWallets.ToList().ForEach(w => AddCyptoToBalanceFromExternal(ref w, false));
                            cycleWallets.ToList().ForEach(w => {if(!w.PublicAddress.Contains("wasErronWhileFillWalletFromEW")) WaitForBalance(w.PublicAddress); });
                        }

                        maxWallets -= MAX_WALLETS_FOR_CASH_IN;
                    }
                }
                var balances = blockchainApi.Balances.GetBalances("500", null).GetResponseObject();

                result.ToList().ForEach(w =>
                {
                    TestContext.Out.WriteLine($"wallet {w.PublicAddress} balance: {balances.Items.FirstOrDefault(wallet => wallet.Address == w.PublicAddress)?.Balance}");
                });

                //after all

                if (blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired.HasValue &&
                    blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired.Value)
                {
                    result.ToList().ForEach(w =>
                    {
                        w.PrivateKey = HOT_WALLET_KEY;
                    });
                }
            }

            return result;
        }

        private bool SetBalanceWIthManyOutputs(List<WalletCreationResponse> wallets)
        {
            var run = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().AreManyOutputsSupported;
            if (run == null || !run.Value)
                return false;

            List<TransactionOutputContract> transactions = new List<TransactionOutputContract>();

            wallets.ForEach(w => transactions.Add(new TransactionOutputContract() { Amount = AMOUT_WITH_FEE, ToAddress = w.PublicAddress }));
            var request = new BuildTransactionWithManyOutputsRequest()
            {
                AssetId = ASSET_ID,
                OperationId = Guid.NewGuid(),
                FromAddress = EXTERNAL_WALLET,
                FromAddressContext = EXTERNAL_WALLET_ADDRESS_CONTEXT,
                Outputs = transactions
            };

            wallets.ForEach(w =>
                blockchainApi.Balances.PostBalances(w.PublicAddress));

            var response = blockchainApi.Operations.PostTransactionsManyOutputs(request);
            response.Validate.StatusCode(HttpStatusCode.OK);

            var signResponse = blockchainSign.PostSign(new SignRequest() { TransactionContext = response.GetResponseObject().TransactionContext, PrivateKeys = new List<string>() { EXTERNAL_WALLET_KEY } });

            var broadcastRequset = new BroadcastTransactionRequest() { OperationId = request.OperationId, SignedTransaction = signResponse.GetResponseObject().SignedTransaction };
            var broadcatedResponse = blockchainApi.Operations.PostTransactionsBroadcast(broadcastRequset);

            wallets.ForEach(w => WaitForBalance(w.PublicAddress));

            return true;
        }

        protected void AddCyptoToBalanceFromExternal(ref WalletCreationResponse wallet, bool wait = true)
        {
            bool wasError = false;
            var transferSupported = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsTestingTransfersSupported;
            var recieveSupport = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsReceiveTransactionRequired;
            if (skipAddBalanceFromEWToDWAfterError)
                wasError = skipAddBalanceFromEWToDWAfterError;
            else
            {
                if (transferSupported != null && transferSupported.Value)
                {
                    blockchainApi.Balances.PostBalances(wallet.PublicAddress);
                    TestingTransferRequest request = new TestingTransferRequest() { amount = AMOUT_WITH_FEE, assetId = ASSET_ID, fromAddress = EXTERNAL_WALLET, fromPrivateKey = EXTERNAL_WALLET_KEY, toAddress = wallet.PublicAddress };
                    var response = blockchainApi.Testing.PostTestingTransfer(request);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        System.Threading.Thread.Sleep(5);
                        response = blockchainApi.Testing.PostTestingTransfer(request);
                    }

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        wasError = true;
                        skipAddBalanceFromEWToDWAfterError = wasError;
                    }

                }
                else if (BlockChainName == "RaiBlocks" || (recieveSupport != null && recieveSupport.Value)) //raiblocks - temp. will be removed after capablities enabled
                {
                    AddCryptoToWalletWithRecieveTransaction(wallet.PublicAddress, wallet.PrivateKey, wait);
                }
                else
                {
                    wasError = BuildSignBroadcastEWDW(wallet, wait: wait);
                    skipAddBalanceFromEWToDWAfterError = wasError;
                }
            }
            if (wait && !wasError)
                WaitForBalance(wallet.PublicAddress);
            if (wasError)
                wallet.PublicAddress = $"{wallet.PublicAddress}_wasErronWhileFillWalletFromEW";
        }

        public static string GetWalletCorrectName(string currentName)
        {
            if (currentName != null && currentName.Contains("_wasErronWhileFillWalletFromEW"))
                currentName = currentName.Replace("_wasErronWhileFillWalletFromEW", "");
            return currentName;
        }

        /// <summary>
        /// Make Build, Sign, Broadcast and return true in case error happend or false otherwise
        /// </summary>
        /// <param name="wallet"></param>
        /// <param name="attemptCount"></param>
        /// <returns></returns>
        public bool BuildSignBroadcastEWDW(WalletCreationResponse wallet, bool wait = false, string amount=null)
        {
            blockchainApi.Balances.PostBalances(wallet.PublicAddress);

            if (amount == null)
                amount = AMOUT_WITH_FEE;

            int i = 0;

            while (i < BUILD_SIGN_BROADCAST_EWDW)
            {
                var model = new BuildSingleTransactionRequest()
                {
                    Amount = amount,
                    AssetId = ASSET_ID,
                    FromAddress = EXTERNAL_WALLET,
                    IncludeFee = false,
                    OperationId = Guid.NewGuid(),
                    ToAddress = wallet.PublicAddress,
                    FromAddressContext = EXTERNAL_WALLET_ADDRESS_CONTEXT
                };
                BuildTransactionResponse responseTransaction = new BuildTransactionResponse() { TransactionContext = null };

                var singleTransactionResponse = blockchainApi.Operations.PostTransactions(model);
                if (singleTransactionResponse.StatusCode != HttpStatusCode.OK)
                {
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(30));
                    i++;
                    continue;
                }
                
                responseTransaction = singleTransactionResponse.GetResponseObject();

                string operationId = model.OperationId.ToString();

                var signResponse = blockchainSign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { EXTERNAL_WALLET_KEY }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

                var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(30));
                    i++;
                    continue;
                }

                var getResponse = blockchainApi.Operations.GetOperationId(operationId);

                if (getResponse.StatusCode != HttpStatusCode.OK || getResponse.GetResponseObject().State == BroadcastedTransactionState.Failed || (getResponse.GetResponseObject().ErrorCode.HasValue && getResponse.GetResponseObject().ErrorCode.Value ==  TransactionExecutionError.buildingShouldBeRepeated))
                {
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(30));
                    i++;
                    continue;
                }

                if (wait)
                    WaitForOperationGotCompleteStatus(operationId);

                return false;
            }
            
            return true;
        }

        protected void AddCryptoToWalletWithRecieveTransaction(string walletAddress, string walletKey, bool wait = true)
        {
            //build send transaction
            blockchainApi.Balances.PostBalances(walletAddress);
            var model = new BuildSingleTransactionRequest()
            {
                Amount = AMOUT_WITH_FEE,
                AssetId = ASSET_ID,
                FromAddress = EXTERNAL_WALLET,
                IncludeFee = false,
                OperationId = Guid.NewGuid(),
                ToAddress = walletAddress,
                FromAddressContext = EXTERNAL_WALLET_ADDRESS_CONTEXT
            };

            var responseTransaction = blockchainApi.Operations.PostTransactions(model).GetResponseObject();
            string operationId = model.OperationId.ToString();

            var signResponse = blockchainSign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { EXTERNAL_WALLET_KEY }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

            var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });

            // wait for wallet present in history

            var broadcastedSendTransaction = blockchainApi.Operations.GetOperationId(operationId).GetResponseObject();

            int i = 0;
            while(i++ < 150 && broadcastedSendTransaction.State == BroadcastedTransactionState.InProgress)
            {
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
                broadcastedSendTransaction = blockchainApi.Operations.GetOperationId(operationId).GetResponseObject();
            }

            Assert.That(broadcastedSendTransaction.State, Is.EqualTo(BroadcastedTransactionState.Completed));
            
            //BuildSingleReceiveTransactionRequest recieve transaction

            var reciveModel = new BuildSingleReceiveTransactionRequest() { operationId = Guid.NewGuid(), sendTransactionHash = broadcastedSendTransaction.Hash };
            var recieve = blockchainApi.Operations.PostTranstactionSingleRecieve(reciveModel).GetResponseObject();
            var signReciveResponse = blockchainSign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { walletKey }, TransactionContext = recieve.transactionContext}).GetResponseObject();

            var responseRecieve = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = reciveModel.operationId, SignedTransaction = signReciveResponse.SignedTransaction });

            if (wait)
                WaitForBalance(walletAddress);
        }

        public void WaitForOperationGotCompleteStatus(string operationId)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed < TimeSpan.FromMinutes(BLOCKCHAIN_MINING_TIME))
            {
                var currentState = blockchainApi.Operations.GetOperationId(operationId).GetResponseObject().State;
                if (currentState != BroadcastedTransactionState.Completed)
                {
                    if(currentState == BroadcastedTransactionState.Failed)
                        break;
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
                }
                else
                    break;
            }
            sw.Stop();
        }

        public void WaitForBalanceBlockIncreased(string walletAddress, long startBlock)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed < TimeSpan.FromMinutes(BLOCKCHAIN_MINING_TIME))
            {
                var currentBlock = blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == walletAddress)?.Block;
                if (startBlock  < currentBlock)
                    return;
                    
                else
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
            }
            sw.Stop();
        }

        public void WaitForBalanceOnWallet(string walletAddress, string balance)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed < TimeSpan.FromMinutes(BLOCKCHAIN_MINING_TIME))
            {
                var currentBalance = blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == walletAddress)?.Balance;
                if (currentBalance == balance)
                    return;

                else
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
            }
            sw.Stop();
        }

        public WalletBalanceContract BalanceResponse(string wallet) => blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet);


        public void WaitForBalance(string wallet)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed < TimeSpan.FromMinutes(BLOCKCHAIN_MINING_TIME))
            {
                if (!blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.Any(w => w.Address == wallet))
                {
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
                }
                else
                    break;
            }
            sw.Stop();
        }

        public (IResponse response, Guid operationId, string transactionContext, string signedTransaction) BuildSignBroadcastSingleTransaction(string walletFrom, string walletFromAddressContext, string walletfromKey, string walletTo, string amount = null, bool includeFee = false)
        {
            var operationId = Guid.NewGuid();
            var model = new BuildSingleTransactionRequest()
            {
                Amount = amount ?? AMOUNT,
                AssetId = ASSET_ID,
                FromAddress = walletFrom,
                FromAddressContext = walletFromAddressContext,
                IncludeFee = includeFee,
                OperationId = operationId,
                ToAddress = walletTo
            };

            var responseTransaction = blockchainApi.Operations.PostTransactions(model).GetResponseObject();
            var transactionContext = responseTransaction.TransactionContext;

            var signResponse = blockchainSign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { walletfromKey }, TransactionContext = transactionContext }).GetResponseObject();
            var signedTransaction = signResponse.SignedTransaction;

            var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = operationId, SignedTransaction = signedTransaction });

            return (response, operationId, transactionContext, signedTransaction);
        }

        public string TransferCryptoBetweenWallets(WalletCreationResponse walletFrom, string walletTo, bool includeFee = true, string amount = null)
        {
            if (walletFrom == null)
                return null;

            var balance = WalletBalance(walletFrom);

            if (balance == null)
                return null;

            if (amount == null)
                amount = balance;

            var model = new BuildSingleTransactionRequest()
            {
                Amount = amount,
                AssetId = ASSET_ID,
                FromAddress = walletFrom.PublicAddress,
                IncludeFee = includeFee,
                OperationId = Guid.NewGuid(),
                ToAddress = walletTo,
                FromAddressContext = walletFrom.AddressContext
            };
          
            var singleTransactionResponse = blockchainApi.Operations.PostTransactions(model);
            var responseTransaction = singleTransactionResponse.GetResponseObject();

            var signResponse = blockchainSign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { walletFrom.PrivateKey }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

            blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });

            return model.OperationId.ToString();
        }

        public string WalletBalance(WalletCreationResponse wallet) => blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance;

        public bool BuildSignBroadcastBetweenWallets(WalletCreationResponse walletFrom, string walletTo, int attemptCount=10,bool wait = false, string amount = null)
        {
            blockchainApi.Balances.PostBalances(walletFrom.PublicAddress);

            if (amount == null)
                amount = AMOUNT;

            int i = 0;

            while (i < attemptCount)
            {
                var model = new BuildSingleTransactionRequest()
                {
                    Amount = amount,
                    AssetId = ASSET_ID,
                    FromAddress = walletFrom.PublicAddress,
                    IncludeFee = false,
                    OperationId = Guid.NewGuid(),
                    ToAddress = walletTo,
                    FromAddressContext = walletFrom.AddressContext
                };
                BuildTransactionResponse responseTransaction = new BuildTransactionResponse() { TransactionContext = null };

                var singleTransactionResponse = blockchainApi.Operations.PostTransactions(model);
                if (singleTransactionResponse.StatusCode != HttpStatusCode.OK)
                {
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(30));
                    i++;
                    continue;
                }

                responseTransaction = singleTransactionResponse.GetResponseObject();

                string operationId = model.OperationId.ToString();

                var signResponse = blockchainSign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { walletFrom.PrivateKey }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

                var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(30));
                    i++;
                    continue;
                }

                var getResponse = blockchainApi.Operations.GetOperationId(operationId);

                if (getResponse.StatusCode != HttpStatusCode.OK || getResponse.GetResponseObject().State == BroadcastedTransactionState.Failed || (getResponse.GetResponseObject().ErrorCode.HasValue && getResponse.GetResponseObject().ErrorCode.Value == TransactionExecutionError.buildingShouldBeRepeated))
                {
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(30));
                    i++;
                    continue;
                }

                if (wait)
                    WaitForOperationGotCompleteStatus(operationId);

                return false;
            }

            return true;
        }
        #endregion
    }
}
