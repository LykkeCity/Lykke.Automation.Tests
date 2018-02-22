# Lykke.Automation.Tests
To run tests against Blockchain integration:
1. Clone this branch locally
2. Now you have two options:
A). Create properties.json into .\AFTests\bin\Debug\netcoreapp2.0\
with the next simple content for your Blockchain
{
  "BlockchainIntegration": "Zcash", //Name
  "AssetId": "ZEC", //AssetId of your Blockchain
  "BlockchainApi": "", //URL to your blockchain api
  "BlockchainSign": "", //URL to your blockchain sign-service api
  "WalletAddress": "some-wallet-address", //Wallet with some crypto in your blockchain testnet
  "WalletKey": "cRPW3spyP9riDJWniNpcbDkiBjpLrhneSh2qTs3uSZUbm4HZLEyB" //Key of your Wallet with some crypto
}
B). Create Class with settings for your Blockchain in BlockchainsIntegrationBaseTest and create system var: BlockchainIntegration = Name(blockchain name)
3. Run tests with filter "BlockchainIntegration"
4. After all tests done open AFTests\bin\Debug\netcoreapp2.0\ and execute: 
..\..\..\..\XUnitTestCommon\Reports\allure-cli\bin\allure generate
5. Open \allure-reports\index.html and check results
