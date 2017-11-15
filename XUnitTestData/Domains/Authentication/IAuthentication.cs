using System.Threading.Tasks;

namespace XUnitTestData.Domains.Authentication
{
    public interface IAuthentication
    {
        string baseAuthUrl { get; set; }
        string authPath { get; set; }
        int authTokenTimeout { get; set; }
        User authentication { get; set; }
        string authToken { get; }

        Task<bool> Authenticate();
        Task<bool> UpdateToken();
    }
}