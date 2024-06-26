using Director.Extensions;
using Director.Models;
using Matchmaker.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Director
{
    /*public static class Constant
    {
        public const string OpenMatchMatchFunctionHost = "basics-match-function";
        public const int OpenMatchMatchFunctionPort = 51502;
        public const string OpenMatchBackendService = "open-match-backend:51505";
        // Game server data
        public const string GameServerPort = "7777";         // Port Name (Port number if you port is not named) | E.G. 25565 
        public const string AppName = "Arelymp";                // E.G. MySuperGame 
        public const string AppVersion = "arelympv0.1.1";          // E.G. V1
        // You MUST have a forward slash (/) at the end of your URL
        public const string ArbitriumAPI = "https://api.edgegap.com/";      // E.G. https://api.edgegap.com/
        // You MUST NOT have prefix "token" in your API token value
        // token 08230a25-0fdb-4f56-917b-0a58ec35cbaf INVALID
        // 08230a25-0fdb-4f56-917b-0a58ec35cbaf VALID
        public const string ApiToken = "token token d13daf58-abe8-42e2-8649-16e1df8b0a15";              // E.G. 08230a25-0fdb-4f56-917b-0a58ec35cbaf
        // This tag will be associated with your deployemnt.
        // You can change it for anything you want!
        public const string DeploymentTag = "Arelymp Test Version"; // E.G. Open Match Tutorial
    }*/

    public static class Director
    {
        private static AppSettings AppSettings = null!;

        public static void Initialize(AppSettings config)
        {
            AppSettings = config;
        }

        private static readonly ILogger logger = LoggerFactory.Create(
            logBuilder => logBuilder.AddConsole(
                configuration =>
                {
                    configuration.TimestampFormat = "[HH:mm:ss] ";
                })
            ).CreateLogger("Director");

        public static void Start()
        {
            while (true)
            {
                logger.LogInformation("Creating matches...");
                MatchPlayers();
                Thread.Sleep(5000);
            }
        }

        private static async Task MatchPlayers()
        {
            foreach (OpenMatchMatchProfile profile in GetProfiles())
            {
                try
                {
                    IList<OpenMatchMatch> matches = await Fetch(profile);

                    logger.LogInformation($"Generated {matches.Count} matches for profile {profile.Name}");

                    Assign(matches).Wait();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);
                }
            }
        }
        /// <summary>
        /// Get all the possible profiles
        /// </summary>
        private static IEnumerable<OpenMatchMatchProfile> GetProfiles()
        {
            string[] modes = new string[] { "mode.casual", "mode.ranked", "mode.private" };

            foreach (string mode in modes)
            {
                yield return new OpenMatchMatchProfile
                {
                    Name = $"{mode}_profile",
                    Pools = new OpenMatchPool[]
                    {
                new OpenMatchPool
                {
                    Name = $"pool_{mode}",
                    TagPresentFilters = new TagPresentFilter[] { new TagPresentFilter {  Tag = mode } }
                }
                    }
                };
            }
        }

        private static async Task<IList<OpenMatchMatch>> Fetch(OpenMatchMatchProfile profile)
        {
            HttpClient client = new HttpClient();

            // Making request object
            var body = new
            {
                config = new MMFConfig
                {
                    Host = AppSettings.OpenMatchMatchFunctionHost,
                    Port = AppSettings.OpenMatchMatchFunctionPort,
                    Type = "REST",
                },
                profile = profile

            };

            List<OpenMatchMatch> proposals = new List<OpenMatchMatch>();

            // Getting match proposals
            try
            {
                // Sending the request to Open Match Backend
                HttpResponseMessage response = await client.PostAsJsonAsync(
                    $"http://{AppSettings.OpenMatchBackendService}/v1/backendservice/matches:fetch",
                    body
                );

                string stringBody = await response.Content.ReadAsStringAsync();
                IEnumerable<string> results = stringBody.Trim().Split('\n').Where(s => s is not null && s.Trim() != "");

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception($"Resquest Error {(int)response.StatusCode} - {stringBody}");
                }

                // Extracting propsals from response
                foreach (string result in results)
                {
                    var matchResult = JsonSerializer.Deserialize<OpenMatchStreamResult<OpenMatchFetchMatchesResponse>>(result);
                    proposals.Add(matchResult.Result.Match);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to fetch matches for profile {profile.Name}, got {ex.Message}", ex);
            }

            return proposals;
        }

        /// <summary>
        /// Deploy a game server with CoDeMa API and assign its IP to match's tickets
        /// </summary>
        private static async Task Assign(IEnumerable<OpenMatchMatch> matches)
        {
            foreach (OpenMatchMatch match in matches)
            {
                // Getting Tickets ID and players IP
                string[] ticketsId = match.Tickets.Select(t => t.Id).ToArray();
                string[] ipList = match.Tickets.Select(t => Encoding.UTF8.GetString(t.Extensions["playerIp"].Value)).ToArray();
                string environment = match.Tickets.SerializeRequiredTicketData();
                

                // Deploying game server and getting ip
                string ip = await DeployAndGetServerIP(ipList, AppSettings.GameServerPort, environment);

                // Assign game server to tickets
                try
                {
                    HttpClient client = new HttpClient();

                    OpenMatchAssignTicketsRequest body = new OpenMatchAssignTicketsRequest
                    {
                        Assignments = new OpenMatchAssignmentGroup[]
                        {
                    new OpenMatchAssignmentGroup
                    {
                        TicketIds = ticketsId,
                        Assignment = new OpenMatchAssignment{ Connection = ip },
                    }
                        }
                    };

                    // Sending the request to Open Match Backend
                    HttpResponseMessage response = await client.PostAsJsonAsync(
                        $"http://{AppSettings.OpenMatchBackendService}/v1/backendservice/tickets:assign",
                        body
                    );

                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new Exception($"Resquest Error {(int)response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Could not assign ticket", ex);
                }

                logger.LogInformation($"Assigned server {ip} to match {match.MatchId}");
            }
        }

        /// <summary>
        /// Deploy the Game server and fetch its IP when the server is ready
        /// </summary>
        private static async Task<string> DeployAndGetServerIP(string[] IPList, string gamePort, string environment)
        {
            // Creating API Client to communicate with arbitrium
            HttpClient client = new HttpClient();
            // Add authorization
            client.DefaultRequestHeaders.Add("authorization", $"token {AppSettings.ApiToken}");

            string requestId;

            try
            {
                // Creating Arbitrium deploy request
                // Based on https://docs.edgegap.com/api/#operation/deploy
                var body = new
                {
                    app_name = AppSettings.AppName,
                    version_name = AppSettings.AppVersion,
                    ip_list = IPList,
                    tags = new string[] { AppSettings.DeploymentTag },
                    env_vars = new object[]
                    {
                        new
                        {
                            key = "PLAYERLIST",
                            value = environment,
                            us_hidden = true
                        }
                    }
                };


                // Deploying
                // Sending the request to Arbitrium
                HttpResponseMessage response = await client.PostAsJsonAsync(
                    $"{AppSettings.ArbitriumAPI}v1/deploy",
                    body
                );

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception($"Resquest Error {(int)response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                }

                requestId = (await response.Content.ReadFromJsonAsync<ArbitriumDeployResponse>()).RequestId;
            }
            catch (Exception ex)
            {
                throw new Exception($"ERROR: Could not deploy game server, err: {ex.Message}", ex);
            }

            double timeout = 30.0;
            DateTime start = DateTime.UtcNow;

            string? status = "";
            ArbitriumDeploymentRequestStatusResponse responseBody = new();

            // Waiting for the server to be ready
            while (status != "Status.READY" && (DateTime.UtcNow - start).TotalSeconds <= timeout)
            {
                try
                {
                    // Based on https://develop-docs.edgegap.com/api/#operation/deployment-status-get
                    // Sending the request to Arbitrium
                    HttpResponseMessage response = await client.GetAsync($"{AppSettings.ArbitriumAPI}v1/status/{requestId}");

                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new Exception($"Resquest Error {(int)response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                    }

                    responseBody = await response.Content.ReadFromJsonAsync<ArbitriumDeploymentRequestStatusResponse>();
                    status = responseBody.CurrentStatus;
                    await Task.Delay(1000); //  let's wait a bit
                }
                catch (Exception ex)
                {
                    throw new Exception($"ERROR: Could not fetch status, err: {ex.Message}", ex);
                }
            }

            if ((DateTime.UtcNow - start).TotalSeconds > timeout)
            {
                throw new Exception($"ERROR: Timeout while waiting for deployment");
            }

            return $"{responseBody.PublicIP}:{responseBody.Ports[AppSettings.GameServerPort].External}";
        }
    }
}
