using LykkePay.Resources.AssetPairRates;
using LykkePay.Resources.ConvertTransfer;
using LykkePay.Resources.GenerateAddress;
using LykkePay.Resources.GetBalance;
using LykkePay.Resources.Order;
using LykkePay.Resources.PostBack;
using LykkePay.Resources.Purchase;
using LykkePay.Resources.PurchaseStatus;
using LykkePay.Resources.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsCore.ServiceSettings;

namespace LykkePay.Api
{
    public class LykkePayApi
    {
        public ServiceSettingsApi settings => new ServiceSettingsApi();
        public AssetPairRates assetPairRates => new AssetPairRates();
        public Purchase purchase => new Purchase();
        public GenerateAddress generateAddress => new GenerateAddress();
        public GetBalance getBalance => new GetBalance();
        public Order order => new Order();
        public Transfer transfer => new Transfer();
        public LykkePay.Resources.Convert.Convert convert => new LykkePay.Resources.Convert.Convert();
        public PurchaseStatus purchaseStatus => new PurchaseStatus();
        public ConvertTransfer convertTransfer => new ConvertTransfer();
        public PostBack postBack => new PostBack();
    }
}
