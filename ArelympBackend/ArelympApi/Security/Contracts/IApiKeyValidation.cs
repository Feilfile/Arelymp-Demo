namespace ArelympApi.Security.Contracts
{
    public interface IApiKeyValidation
    {
        bool IsValidApiKey(string apiKey);
    }
}
