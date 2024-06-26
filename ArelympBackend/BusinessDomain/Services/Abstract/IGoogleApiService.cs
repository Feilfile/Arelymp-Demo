using Google.Apis.Auth.OAuth2.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDomain.Services.Abstract
{
    public interface IGoogleApiService
    {
        public Task<string> GetPlayerId(string accessToken);

        public Task<TokenResponse> GetOauth2TokenResponse(string exchangeCode);
    }
}
