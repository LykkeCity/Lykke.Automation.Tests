using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTestData.Domains.Assets
{
    public interface IIssuer : IDictionaryItem
    {
        string Id { get; }
        string Name { get; }
        string IconUrl { get; }
    }

    public class Issuer : IIssuer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }


        public static Issuer Create(string id, string name, string iconUrl)
        {
            return new Issuer
            {
                Id = id,
                Name = name,
                IconUrl = iconUrl
            };
        }

        public static Issuer CreateDefault()
        {
            return new Issuer
            {

            };
        }

    }

    public interface IIssuerRepository
    {
        Task RegisterIssuerAsync(IIssuer issuer);
        Task EditIssuerAsync(string id, IIssuer issuer);
        Task<IEnumerable<IIssuer>> GetAllIssuersAsync();
        Task<IIssuer> GetIssuerAsync(string id);
    }
}
