using System.Threading.Tasks;

namespace XUnitTestData.Domains.Authentication
{
    public interface IAuthentication
    {
        string BaseAuthUrl { get; set; }
        string AuthPath { get; set; }
        int AuthTokenTimeout { get; set; }
        User AuthUser { get; set; }
        string AuthToken { get; }

        Task<bool> Authenticate();
        Task<bool> UpdateToken(bool forceUpdate);
    }
}