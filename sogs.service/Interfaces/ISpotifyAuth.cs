using System.Threading.Tasks;

namespace sogs.service.Interfaces
{
    public interface ISpotifyAuth
    {
        string GetAuthorizationUrl();
        Task ExchangeCodeForToken(string code);
        string GetAccessToken();
    }
}