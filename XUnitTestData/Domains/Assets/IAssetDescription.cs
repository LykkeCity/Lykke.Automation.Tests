using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTestData.Domains.Assets
{
    public interface IAssetDescription : IDictionaryItem
    {
        string Id { get; }
        string AssetId { get; }
        string AssetClass { get; }
        string Description { get; }
        string IssuerName { get; set; }
        string NumberOfCoins { get; }
        string MarketCapitalization { get; }
        [Required]
        int PopIndex { get; }
        string AssetDescriptionUrl { get; }
        string FullName { get; }
    }

    public class AssetDescription : IAssetDescription
    {
        public string Id { get; set; }
        public string AssetId { get; set; }
        public string AssetClass { get; set; }
        public string Description { get; set; }
        public string IssuerName { get; set; }
        public string NumberOfCoins { get; set; }
        public string MarketCapitalization { get; set; }
        [Required]
        public int PopIndex { get; set; }
        public string AssetDescriptionUrl { get; set; }
        public string FullName { get; set; }


        public static AssetDescription CreateDefault(string id)
        {
            return new AssetDescription
            {
                Id = id,
                PopIndex = 0,
                MarketCapitalization = string.Empty,
                Description = string.Empty,
                IssuerName = string.Empty,
                AssetClass = string.Empty,
                NumberOfCoins = string.Empty,
                AssetDescriptionUrl = string.Empty
            };
        }

        public static AssetDescription Create(IAssetDescription src, IIssuer issuer)
        {
            return new AssetDescription
            {
                Id = src.Id,
                AssetId = src.AssetId,
                AssetClass = src.AssetClass,
                Description = src.Description,
                IssuerName = issuer?.Name,
                MarketCapitalization = src.MarketCapitalization,
                NumberOfCoins = src.NumberOfCoins,
                PopIndex = src.PopIndex,
                AssetDescriptionUrl = src.AssetDescriptionUrl,
                FullName = src.FullName
            };
        }
    }

    public interface IAssetDescriptionRepository
    {

        Task SaveAsync(IAssetDescription src);
        Task<IAssetDescription> GetAssetExtendedInfoAsync(string id);
        Task<IEnumerable<IAssetDescription>> GetAllAsync();
    }


    public static class AssetExtendedInfoExt
    {
        public static async Task<IAssetDescription> GetAssetExtendedInfoOrDefaultAsync(this IAssetDescriptionRepository table, string id)
        {
            if (id == null)
                return AssetDescription.CreateDefault(null);
            var aei = await table.GetAssetExtendedInfoAsync(id);
            return aei ?? AssetDescription.CreateDefault(null);
        }
    }
}
