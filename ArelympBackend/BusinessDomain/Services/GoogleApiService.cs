using BusinessDomain.ExternalModels;
using BusinessDomain.Services.Abstract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using BusinessDomain.Constants;
using Google.Apis.Auth.OAuth2.Responses;
using Microsoft.Extensions.Caching.Memory;

namespace BusinessDomain.ExternaServices
{
    public class GoogleApiService : IGoogleApiService
    {
        private readonly HttpClient _httpClient;

        //private IMemoryCache _cache;

        private readonly string _applicationId;

        private readonly string _clientId;

        private readonly string _clientSecret;

        public GoogleApiService(IConfiguration configuration, HttpClient httpClient/*, IMemoryCache memoryCache*/) 
        { 
            _httpClient = httpClient;
            //_cache = memoryCache;
            _applicationId = configuration["GoogleOAuthSettings:ApplicationId"]!;
            _clientId = configuration["GoogleOAuthSettings:WebserverClientId"]!;
            _clientSecret = configuration["GoogleOAuthSettings:WebserverClientSecret"]!;
        }

        public async Task<string> GetPlayerId(string accessToken)
        {
            var url = $"{Urls.GoogleApi}/{_applicationId}/verify";

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(requestMessage);
            var googleIdentityString = await response.Content.ReadAsStringAsync();
            var googleIdentity = JsonConvert.DeserializeObject<GoogleIdentity>(googleIdentityString);

            return googleIdentity.Player_id;
        }

        public async Task<TokenResponse> GetOauth2TokenResponse(string exchangeCode)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Urls.GoogleOauth2Api);

            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("code", exchangeCode),
                new KeyValuePair<string, string>("client_id", _clientId),
                new KeyValuePair<string, string>("client_secret", _clientSecret),
                new KeyValuePair<string, string>("redirect_uri", ""),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
            });

            requestMessage.Content = requestContent;

            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                // Handle error
                return null;
            }

            var tokenResponseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TokenResponse>(tokenResponseString);
        }
    }
}
