using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using sogs.domain.Domains;
using sogs.service.Interfaces;

namespace sogs.service.Services
{
    public class SpotifyAuth : ISpotifyAuth
    {
        private string clientId;
        private string clientSecret;
        private string redirectUri;
        private string accessToken;

        public SpotifyAuth(string clientId, string clientSecret, string redirectUri)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.redirectUri = redirectUri;
        }

        public string GetAuthorizationUrl()
        {
            var scopes = "user-read-playback-state user-modify-playback-state";
            var url = $"https://accounts.spotify.com/authorize?client_id={clientId}&response_type=code&redirect_uri={redirectUri}&scope={scopes}";
            return url;
        }

        public async Task ExchangeCodeForToken(string code)
        {
            var request = new RestRequest("https://accounts.spotify.com/api/token");
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", code);
            request.AddParameter("redirect_uri", redirectUri);
            request.AddParameter("client_id", clientId);
            request.AddParameter("client_secret", clientSecret);
            request.Method = Method.Post;

            var client = new RestClient();
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var content = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
                accessToken = content.Access_token;
            }
            else
            {
                throw new Exception("Failed to exchange code for token");
            }
        }

        public string GetAccessToken()
        {
            return accessToken;
        }
    }
}