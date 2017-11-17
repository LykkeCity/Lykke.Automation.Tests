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

    public interface IIssuerRepository
    {
        Task RegisterIssuerAsync(IIssuer issuer);
        Task EditIssuerAsync(string id, IIssuer issuer);
        Task<IEnumerable<IIssuer>> GetAllIssuersAsync();
        Task<IIssuer> GetIssuerAsync(string id);
    }
}
