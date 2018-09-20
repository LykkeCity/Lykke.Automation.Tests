using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.ApiV2.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace ApiV2Data.Api
{
    public class Withdrawals : ApiBase
    {
        public IResponse<WithdrawalCryptoInfoModel> GetWithDrawalsCryptoAssetId(string assetId, string token) =>
            Request.Get($"/withdrawals/crypto/{assetId}/info").WithBearerToken(token).Build().Execute<WithdrawalCryptoInfoModel>();

        public IResponse<WithdrawalCryptoFeeModel> GetWithdrawalsCryptoAssetIdFee(string assetId, string token) =>
            Request.Get($"/withdrawals/crypto/{assetId}/fee").WithBearerToken(token).Build().Execute<WithdrawalCryptoFeeModel>();

        public IResponse<WithdrawalCryptoAddressValidationModel> GetWithdrawalsCryptoAssetIdValidateAddress(string assetId, string baseAddress, string addressExtension, string token) =>
            Request.Get($"/withdrawals/crypto/{assetId}/validateAddress")
            .AddQueryParameterIfNotNull("baseAddress", baseAddress)
            .AddQueryParameterIfNotNull("addressExtension", addressExtension).WithBearerToken(token).Build().Execute<WithdrawalCryptoAddressValidationModel>();

        public IResponse<List<string>> GetWithdrawalsCryptoAvailable(string token) =>
            Request.Get("/withdrawals/crypto/available").WithBearerToken(token).Build().Execute<List<string>>();
    }
}
