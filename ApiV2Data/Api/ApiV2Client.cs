using System;
using System.Collections.Generic;
using System.Text;

namespace ApiV2Data.Api
{
    public class ApiV2Client
    {
        public Affiliate Affiliate => new Affiliate();
        public AssetPairs AssetPairs => new AssetPairs();
        public Assets Assets => new Assets();
        public CandlesHistory CandlesHistory => new CandlesHistory();
        public Catalogs Catalogs => new Catalogs();
        public Client Client => new Client();
        public ClientAccountRecovery ClientAccountRecovery => new ClientAccountRecovery();
        public Deposits Deposits => new Deposits();
        public Dialogs Dialogs => new Dialogs();
        public Dictionary Dictionary => new Dictionary();
        public HFT HFT => new HFT();
        public History History => new History();
        public IsAlive IsAlive => new IsAlive();
        public Market Market => new Market();
        public Markets Markets => new Markets();
        public Operations Operations => new Operations();
        public Orderbook Orderbook => new Orderbook();
        public Orders Orders => new Orders();
        public PaymentMethods PaymentMethods => new PaymentMethods();
        public SecondFactorAuth SecondFactorAuth => new SecondFactorAuth();
        public Wallets wallets => new Wallets();
        public Watchlists Watchlists => new Watchlists();
        public Withdrawals Withdrawals => new Withdrawals();

        public CustomRequests CustomRequests => new CustomRequests();
    }
}
