using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace XUnitTestCommon
{
    public class ApiPaths
    {
        // API V2
        public static readonly string WALLETS_BASE_PATH = "/api/wallets";
        public static readonly string WALLETS_BALANCES_PATH = "/api/wallets/balances";
        public static readonly string WALLETS_TRADING_BALANCES_PATH = "/api/wallets/trading/balances";

        public static readonly string OPERATIONS_BASE_PATH = "/api/operations";
        public static readonly string OPERATIONS_DETAILS_PATH = "/api/operationsDetails";
        public static readonly string OPERATIONS_DETAILS_CREATE_PATH = "/api/operationsDetails/create";
        public static readonly string OPERATIONS_DETAILS_REGISTER_PATH = "/api/operationsDetails/register";
        public static readonly string OPERATIONS_TRANSFER_PATH = "/api/operations/transfer";
        public static readonly string OPERATIONS_CANCEL_PATH = "/api/operations/cancel";

        public static readonly string HFT_BASE_PATH = "/api/hft";

        public static readonly string CLIENT_BASE_PATH = "/api/client";
        public static readonly string CLIENT_REGISTER_PATH = "/api/client/register";

        public static readonly string ASSETS_BASE_PATH = "/api/assets";
        public static readonly string ASSETS_BASEASSET_PATH = "/api/assets/baseAsset";

        public static readonly string ISALIVE_BASE_PATH = "/api/isAlive";        

        public static readonly string TRANSACTION_HISTORY_BASE_PATH = "/api/transactionHistory";

        public static readonly string PLEDGES_BASE_PATH = "/api/pledges";

        public static readonly string TWITTER_BASE_PATH = "/api/twitter";

        public static readonly string REFERRAL_LINKS_BASE_PATH = "/api/refLinks";

        // API V2 prefix
        public static readonly string ASSETS_V2_BASE_PATH = "/api/v2/assets";
        public static readonly string ASSETS_DEFAULT_PATH = "/api/v2/assets/default";
        public static readonly string ASSET_ATTRIBUTES_PATH = "/api/v2/asset-attributes";
        public static readonly string ASSET_CATEGORIES_PATH = "/api/v2/asset-categories";
        public static readonly string ASSET_EXTENDED_INFO_PATH = "/api/v2/asset-extended-infos";
        public static readonly string ASSET_GROUPS_PATH = "/api/v2/asset-groups";
        public static readonly string ASSET_PAIRS_PATH = "/api/v2/asset-pairs";
        public static readonly string ASSET_SETTINGS_PATH = "/api/v2/asset-settings";
        public static readonly string ISSUERS_BASE_PATH = "/api/v2/issuers";

        public static readonly string CLIENTS_BASE_PATH = "/api/v2/clients";

        public static readonly string MARGIN_ASSET_BASE_PATH = "/api/v2/margin-assets";
        public static readonly string MARGIN_ASSET_PAIRS_PATH = "/api/v2/margin-asset-pairs";
        public static readonly string MARGIN_ISSUERS_PATH = "/api/v2/margin-issuers";

        public static readonly string WATCH_LIST_BASE_PATH = "/api/v2/watch-lists";
        public static readonly string WATCH_LIST_PREDEFINED_PATH = "/api/v2/watch-lists/predefined";
        public static readonly string WATCH_LIST_CUSTOM_PATH = "/api/v2/watch-lists/custom";
        public static readonly string WATCH_LIST_ALL_PATH = "/api/v2/watch-lists/all";
    }    
}
