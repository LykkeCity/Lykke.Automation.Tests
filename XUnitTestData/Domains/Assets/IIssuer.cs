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
}
