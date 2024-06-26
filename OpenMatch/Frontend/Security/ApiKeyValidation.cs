using Frontend.Constants;

namespace ArelympApi.Security
{
    public class ApiKeyValidation : IApiKeyValidation
    {
        private readonly IConfiguration _configuration;

        public ApiKeyValidation(IConfiguration configuration)
        {
            this._configuration = configuration;
        }


        public bool IsValidApiKey(string inputApiKey)
        {
            if (string.IsNullOrEmpty(inputApiKey)) 
            {
                return false;
            }

            var apiKey = _configuration.GetValue<string>(RequestHeaders.ApiKeyName);

            if (apiKey == null || inputApiKey != apiKey) 
            {
                return false;
            }

            return true;
        }
    }
}
