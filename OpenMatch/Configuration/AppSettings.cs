namespace Matchmaker.Configuration
{
    public class AppSettings
    {
        #region Director

        public string OpenMatchMatchFunctionHost { get; set; } = null!;

        public int OpenMatchMatchFunctionPort { get; set; } 

        public string OpenMatchBackendService { get; set; } = null!;

        // Game server data
        public string GameServerPort { get; set; } = null!; // Port Name (Port number if you port is not named) | E.G. 25565 

        public string AppName { get; set; } = null!; // E.G. MySuperGame 

        public string AppVersion { get; set; } = null!; // E.G. V1

        // You MUST have a forward slash (/) at the end of your URL
        public string ArbitriumAPI { get; set; } = null!; // E.G. https://api.edgegap.com/

        // You MUST NOT have prefix "token" in your API token value
        // token 08230a25-0fdb-4f56-917b-0a58ec35cbaf INVALID
        // 08230a25-0fdb-4f56-917b-0a58ec35cbaf VALID
        public string ApiToken { get; set; } = null!; // E.G. 08230a25-0fdb-4f56-917b-0a58ec35cbaf

        // This tag will be associated with your deployemnt.
        // You can change it for anything you want!
        public string DeploymentTag { get; set; } = null!; // E.G. Open Match Tutorial

        #endregion

        #region Frontend

        public string FrontendBaseUrl { get; set; } = null!;

        public string FrontedApiKey { get; set; } = null!;

        #endregion

        #region MatchFunction

        public string OpenMatchQueryService { get; set; } = null!;

        public string MatchName { get; set; } = null!;

        #endregion
    }
}
