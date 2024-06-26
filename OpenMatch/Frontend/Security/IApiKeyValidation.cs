namespace ArelympApi.Security
{
    public interface IApiKeyValidation
    {
        bool IsValidApiKey(string apiKey);
    }
}
