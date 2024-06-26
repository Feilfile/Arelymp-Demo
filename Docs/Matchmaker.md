##### <a href="../Documentation.md">< Main Page</a>

# Matchmaking

This documentation provides a comprehensive overview of implementing a matchmaking system using OpenMatch, C#, and the Edgegap Arbitrary API. The goal of this project is to create an efficient, scalable, and low-latency matchmaking service that can handle the demands of modern multiplayer games. By leveraging OpenMatch's flexible matchmaking framework and Edgegap's dynamic hosting solutions, we can achieve a robust matchmaking system that enhances the player experience.

The following Link: https://open-match.dev/site/docs/guides/matchmaker/ explains all the components of the matchmaker and how the matchmaking flow is working.

In order to understand how the matchmaker needs to be implemented the general architecture, specifically the matchmaking flow of this project needs to be explained:

##### Game Architecture
![image](./GameArchitecture.png)

##### Matchmaker
![image](./Openmatch-architecture.png)
Source: https://open-match.dev/site/docs/guides/matchmaker/


The matchmaker is connected to the central backend and all requests run though the backend instead of the matchmaker directly as it is implemented in the documentation. Therefore long polling requests are used from the user frontend (unity) and the central backend is doing the entire communication with the matchmaker. This has multiple advantages, the main advantage is that all requests are controlled by the backend and additional data that is required by the matchmaker is sent from the backend instead of the frontend and therefore can not be manipulated. Another advantage is that the request can be cancelled by triggering the cancellation token that is then deleting the matchmaking ticket. This prevents stale tickets and guarantees queue cancelling.

In order to implement the matchmaker we can use most of the default components and have to implement the following 3 Components:

Frontend: The Open Match FrontendService is used to create, delete and get details of the current state of a Ticket. Note that this is the only components that is accessible outside of the kubernetes cluster and therefore the interface of the matchmaker

Match Functions: The core matchmaking logic is implemented as a Match Function service. Open Match Backend triggers the Match Function service when it receives a request to generate Matches. The Match Function execution receives MatchProfiles, fetches Tickets that match the profile from QueryService and returns Matches.

Director: This is the component that understands the types of matches (MatchProfiles) that can be served and fetches matches from the Open Match Backend. The Director also interfaces with the DGS allocation system to fetch Game Servers for Matches and creating Game Server Assignments from these details in Open Match, via communicating with the Open Match Backend

### Frontend

The frontend component is implemented as a normal Asp.NET Core Web API using controllers but instead of using the normal endpoint some extra parameters like mode, playerIp and playerId is passed. The API Documentation of the frontendservice where the requests are sent is located here: https://open-match.dev/site/swaggerui/index.html?urls.primaryName=Frontend#/FrontendService/FrontendService_CreateTicket

The OpenMatchCreateTicketRequest has search fields that are used for the matchmaking algorithm like the mode that is passed in this example. The Extensions are used for additional variables that are used for other purposes. The playerIP is used for the Edgegap Arbitrary API to select the most optimal optimal server location and the playerId is passed as an environment variable to the dedicated server (docker container).

```C#
private async Task<string> CreateTicketToOpenMatch(string mode, string playerIP, string playerId)
{
    var playerIPBytes = ByteConverter.IpToBin(playerIP);
    var playerIdBytes = ByteConverter.StringToBytes(playerId);

    // Creating the payload
    OpenMatchCreateTicketRequest body = new OpenMatchCreateTicketRequest
    {
        Ticket = new OpenMatchTicket
        {
            SearchFields = new OpenMatchSearchFields { Tags = new string[] { mode } },
            Extensions = new Dictionary<string, ProtobufAny>
               {
                    {
                        "playerIp",
                        new ProtobufAny
                        {
                            TypeUrl = "type.googleapis.com/google.protobuf.StringValue",
                            Value = playerIPBytes
                        }
                    },
                    {
                        "playerId",
                        new ProtobufAny
                        {
                            TypeUrl = "type.googleapis.com/google.protobuf.StringValue",
                            Value = playerIdBytes
                        }
                    }

            }
        }
    };

    // Sending the request to Open Match Front End
    HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
        $"http://{_baseUrl}/v1/frontendservice/tickets",
        body
    );

    // Check if we were able to create the ticket
    if (response.StatusCode != HttpStatusCode.OK)
    {
        string msg = $"ERROR - Was not able to create a ticket: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
        Console.WriteLine(msg);
        throw new Exception(msg);
    }

    return await response.Content.ReadAsStringAsync();
}
```
The next code block show how the matchmaking is called from the central backend. Like explained earlier a long polling request is used that checks in an interval (every 5 seconds) if a match is found and if that is the case it returns the IP-Address of the dedicated server. 

If the cancellationToken is triggered it sends a DeleteTicket request to remove the ticket from the matchmaking queue.

```C#
        MatchmakerController.cs

        [HttpPost("queue/{mode}")]
        [ProducesResponseType(typeof(TextResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> JoinMatchmakingQueue([FromRoute] GameMode mode, CancellationToken cancellationToken)
        {
            var userId = HttpContext.GetCompleteUserId();

            var user = await _userService.GetUserDataAsync(userId, cancellationToken);

            if (user == null)
            {
                throw new ArgumentNullException("user model is null");
            }

            var ipAdress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

            var gameMode = $"mode.{mode.ToString().ToLower()}";

            var response = await _matchmakingService.ProcessMatchmakingAsync(user, ipAdress, gameMode, cancellationToken);

            return Ok(new TextResponseDto(response));
        }
```

```C#
    public class MatchmakingService : IMatchmakingService
    {
        private readonly HttpClient _httpClient;
        private readonly IMatchmakerApiClient _matchmakerClient;

        public MatchmakingService(IConfiguration configuration, HttpClient httpClient)
        {
            _httpClient = httpClient;
            var mmUrl = configuration["Matchmaking:Url"];
            var mmToken = configuration["Matchmaking:Token"];
            _httpClient.BaseAddress = new Uri(mmUrl!);
            _httpClient.DefaultRequestHeaders.Add("Authorization", mmToken!);
            _matchmakerClient = new MatchmakerApiClient(mmUrl, httpClient);
        }

        public async Task<string?> ProcessMatchmakingAsync(UserDto userId, string userIp, string gameMode, CancellationToken cancellationToken)
        {
            var createTicketPayload = new CreateTicketPayload
            {
                Mode = gameMode,
                IpAddress = userIp,
                PlayerId = userId.Id,
            };

            var response = await _matchmakerClient.TicketsPOSTAsync(createTicketPayload);
            var ticketId = response.Id;

            if (string.IsNullOrEmpty(ticketId))    
            { 
                throw new ArgumentNullException("Matchmakind ticket is null"); 
            };

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var ticket = await _matchmakerClient.TicketsGETAsync(ticketId, cancellationToken);
                    if (ticket?.Assignment?.Connection != null)
                    {
                        // Match found
                        return ticket.Assignment.Connection;
                    }
                    await Task.Delay(5000, cancellationToken); 
                }
            }
            catch (OperationCanceledException)
            {
                await _matchmakerClient.TicketsDELETEAsync(ticketId);
                throw;
            }
            return null;
        }
    }
```

### Matchfunction

The Matchfunction logic is pretty much identical to the one of the Edgegap documentation: https://docs.edgegap.com/docs/matchmaker/component-tutorial/match-function

### Director

Most of the implementation of the Edgegap Director implementation is used: https://docs.edgegap.com/docs/matchmaker/component-tutorial/director. The only thing that is changed is that configuration is read from a configuration[LINK] file.

Additionally the Environments variables are json serialized and sent to the docker container. 

```C#

        private static async Task Assign(IEnumerable<OpenMatchMatch> matches)
        {
            foreach (OpenMatchMatch match in matches)
            {
                // Getting Tickets ID and players IP
                string[] ticketsId = match.Tickets.Select(t => t.Id).ToArray();
                string[] ipList = match.Tickets.Select(t => Encoding.UTF8.GetString(t.Extensions["playerIp"].Value)).ToArray();
                string environment = match.Tickets.SerializeRequiredTicketData();
                

                // Deploying game server and getting ip
                string ip = await DeployAndGetServerIP(ipList, Appsettings.GameServerPort, environment);

                ...
            }
        }

```

```C#

    public static class TicketExtensions
    {
        public static string SerializeRequiredTicketData(this OpenMatchTicket[] tickets)
        {
            var environmentModels = tickets.Select(ticket => new EnvironmentModel
            {
                ticketId = ticket.Id,
                playerId = Encoding.UTF8.GetString(ticket.Extensions.GetValueOrDefault("playerId").Value)
            });

            return System.Text.Json.JsonSerializer.Serialize(environmentModels); //could also use Newtonsoft JSON
        }
    }

```

### Dockerization and Kubernetes setup

This section shows all the Dockerfiles to build the Docker containers for the kubernetes setup. A class library with all the the AppSettings is used and passed to the containers. The Configuration.json needs to be filled with the corresponding values.

Configuration.json

```json
{
  // Director
  "OpenMatchMatchFunctionHost": "arelymp-match-function",
  "OpenMatchMatchFunctionPort": 51502,
  "OpenMatchBackendService": "open-match-backend:51505",
  "GameServerPort": "7777",
  "AppName": "Arelymp",
  "AppVersion": "arelympv0.1.1",
  "ArbitriumAPI": "https://api.edgegap.com/",
  "ApiToken": "<apiToken>",
  "DeploymentTag": "Arelymp Test Version",

  // Frontend
  "FrontendBaseUrl": "open-match-frontend:51504",
  "FrontedApiKey": "<frontend-api-key>",

  // Match Function
  "OpenMatchQueryService": "open-match-query:51503",
  "MatchName": "basics-match-function"
}
```

Dockerfile.Director:

```yml

#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Update root CA to ensure outbound HTTPS requests don't fail
RUN apt-get update && \
    apt-get install -y ca-certificates && \
    update-ca-certificates && \
    apt-get clean

# Publish App for linux-x64
COPY ./Director/Director.csproj ./Director/
COPY ./Configuration/Configuration.csproj ./Configuration/

RUN dotnet restore ./Director/

COPY ./Director/ ./Director/
COPY ./Configuration/ ./Configuration/

RUN dotnet publish -c Release -r linux-x64 ./Director/Director.csproj

# Runtime Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Get publish from BuildImage
COPY --from=build ./Director/bin/Release/net8.0/linux-x64/publish /app

WORKDIR /app

ENTRYPOINT ["./Director"]   

```

Dockerfile.Frontend:

```yml

#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Update root CA to ensure outbound HTTPS requests don't fail
RUN apt-get update && \
    apt-get install -y ca-certificates && \
    update-ca-certificates && \
    apt-get clean

# Publish App for linux-x64
COPY ./Frontend/Frontend.csproj ./Frontend/
COPY ./Configuration/Configuration.csproj ./Configuration/

RUN dotnet restore ./Frontend/

COPY ./Frontend/ ./Frontend/
COPY ./Configuration/ ./Configuration/

RUN dotnet publish -c Release -r linux-x64 ./Frontend/Frontend.csproj

# Runtime Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Set Listen PORT https://andrewlock.net/5-ways-to-set-the-urls-for-an-aspnetcore-app/#environment-variables
ENV ASPNETCORE_URLS=http://*:51504

EXPOSE 51504

# Get publish from BuildImage
COPY --from=build ./Frontend/bin/Release/net8.0/linux-x64/publish /app

WORKDIR /app

ENTRYPOINT ["./Frontend"]   

```

Dockerfile.MatchFunction:

```yml

#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Update root CA to ensure outbound HTTPS requests don't fail
RUN apt-get update && \
    apt-get install -y ca-certificates && \
    update-ca-certificates && \
    apt-get clean

# Publish App for linux-x64
COPY ./MatchFunction/MatchFunction.csproj ./MatchFunction/
COPY ./Configuration/Configuration.csproj ./Configuration/

RUN dotnet restore ./MatchFunction/

COPY ./MatchFunction/ ./MatchFunction/
COPY ./Configuration/ ./Configuration/

RUN dotnet publish -c Release -r linux-x64 ./MatchFunction/MatchFunction.csproj

# Runtime Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Set Listen PORT https://andrewlock.net/5-ways-to-set-the-urls-for-an-aspnetcore-app/#environment-variables
ENV ASPNETCORE_URLS=http://*:51502

# Get publish from BuildImage
COPY --from=build ./MatchFunction/bin/Release/net8.0/linux-x64/publish /app

WORKDIR /app

ENTRYPOINT ["./MatchFunction"]   

```

kubernetes.yml
```yml

# Front End Component
apiVersion: apps/v1
kind: Deployment
metadata:
  name: arelymp-matchmaker-front-end
  namespace: open-match
  labels:
    app: arelymp-matchmaker-front-end
    component: frontend
spec:
  replicas: 1
  selector:
    matchLabels:
      app: arelymp-matchmaker-front-end
      component: frontend
  template:
    metadata:
      labels:
        app: arelymp-matchmaker-front-end
        component: frontend
    spec:
      containers:
      - name: arelymp-matchmaker-front-end
        image: openmatch-frontend:v1
        ports:
        - containerPort: 51504
---
kind: Service
apiVersion: v1
metadata:
  name: arelymp-matchmaker-front-end
  namespace: open-match
spec:
  type: LoadBalancer
  selector:
    app: arelymp-matchmaker-front-end
    component: frontend
  type: LoadBalancer
  ports:
  - port: 51504
    targetPort: 51504
---
# Match Function component
apiVersion: apps/v1
kind: Deployment
metadata:
  name: arelymp-match-function
  namespace: open-match
  labels:
    app: arelymp-match-function
    component: matchfunction
spec:
  replicas: 1
  selector:
    matchLabels:
      app: arelymp-match-function
      component: matchfunction
  template:
    metadata:
      labels:
        app: arelymp-match-function
        component: matchfunction
    spec:
      containers:
      - name: arelymp-match-function
        image: openmatch-match-function:v1
---
kind: Service
apiVersion: v1
metadata:
  name: arelymp-match-function
  namespace: open-match
  labels:
    app: arelymp-match-function
    component: matchfunction
spec:
  selector:
    app: arelymp-match-function
    component: matchfunction
  type: ClusterIP
  ports:
  - name: grpc
    protocol: TCP
    port: 50502
    targetPort: 50502
  - name: http
    protocol: TCP
    port: 51502
    targetPort: 51502
---
# Director component
apiVersion: apps/v1
kind: Deployment
metadata:
  name: arelymp-director
  namespace: open-match
  labels:
    app: arelymp-director
spec:
  replicas: 1
  selector:
    matchLabels:
      app: arelymp-director
  template:
    metadata:
      labels:
        app: arelymp-director
    spec:
      containers:
      - name: arelymp-director
        image: openmatch-director:v1

```

The following script is a simple shell script for updating the kubernetes cluster.

KubeImageUpdate.sh

```sh

#!/bin/bash

kubectl delete -f ./kubernetes.yml

docker build -f Dockerfile.Frontend -t openmatch-frontend:v1 .

docker build -f Dockerfile.Director -t openmatch-director:v1 .

docker build -f Dockerfile.MatchFunction -t openmatch-match-function:v1 .

kubectl apply -f ./kubernetes.yml

```
