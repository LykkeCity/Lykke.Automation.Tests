# Lykke.Automation.Tests

Api tests to validate functional of Blockchain Integration into Lykke system


### Prerequisites

Your system contains dotnet installed

### Installing

Clone this repository and switch into branch

Create properties.json into .\AFTests\bin\Debug\netcoreapp2.0\

Set content to properties.json with the following settings for your Blockchain

```
{
  "BlockchainIntegration": "Zcash", //Name
  "AssetId": "ZEC", //AssetId of your Blockchain
  "BlockchainApi": "", //URL to your blockchain api
  "BlockchainSign": "", //URL to your blockchain sign-service api
  "DepositWalletAddress": "", // Wallet with some crypto
  "DepositWalletKey": "", //key for DW 
  "HotWallet": "", //HotWallet in blockchain
  "HotWalletKey": "", key for this HW
  "ExternalWalletAddress": "", //Wallet that been created not in lykke system
  "ExternalWalletKey": "", // key
  "ExternalWallerAddressContext": "", // address context for EW 
  "BlockchainMiningTime": 10 // blockchain mining time(in minutes), time to change transaction state from inProgress to Complete 
}
```

## Running the tests

Open console into AFTests project and run a command

```
dotnet test AFTests.csproj --filter "TestCategory=BlockchainIntegration"
```

### Generate Report

After tests finished generate report by cmd

```
..\..\..\..\XUnitTestCommon\Reports\allure-cli\bin\allure generate
```

### View Report

Open report file(firefox)

```
AFTests\bin\Debug\netcoreapp2.0\allure-results\index.html
```
