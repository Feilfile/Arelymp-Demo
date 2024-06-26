##### <a href="../Documentation.md">< Main Page</a>
# 1. Authentication:

This project uses a centralized backend that authorizes all requests. There are two groups that communicate with the backend: the user and the game server instances. The game server uses an API key to authenticate with the backend and only uses the endpoints defined in GameServerController.cs.

Since no communication is required inside the game, the game server only communicates at the beginning of the match to load all user data of the matched players and after the game to store the rewards of the match.

To understand the user/client authorization of the game server, the authorization of the user needs to be explained first.

## User Authentication inside the Mobile-App
Since the game is a mobile app, the authorization is a bit more complex because mobile apps usually do not use simple authentication methods with a username and password. Most mobile apps are downloaded from an app store and use some kind of identity provider for authentication ([Link to Authentication Provider]). Since every request needs to be authorized in a custom backend (ASP.Net Core backend), the OAuth2 PKCE is used for authentication.

The following picture shows the authorization flow:

![image](./OAuth2PKCE.png)
##### Source: https://blog.postman.com/pkce-oauth-how-to/


The authentication takes place when the app is started and is done in a separate scene. The authorization code generation and validation is done by the google cloud. The following code snippets show how the authentication is implemented by the frontend (unity) and backend (Asp.Net Core)

##### GooglePlayLogin.cs
```C# 
public void SignIn()
{
    // Step 1: This authenticates against the Google Cloud using the Google Play API
    PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
}

internal void ProcessAuthentication(SignInStatus status)
{
    if (status != SignInStatus.Success)
    {
        Debug.LogError("Unable to reach the Google Play API");
    }

    var id = PlayGamesPlatform.Instance.GetUserId();

    // Step 2: Requests the Authorization Code that is required to authenticate to the Arelymp backend
    PlayGamesPlatform.Instance.RequestServerSideAccess(false, async code =>
    {
        if (string.IsNullOrEmpty(code))
        {
            Debug.LogError("Failed to obtain server-side access code.");
            return;
        }

        // Step 3: Sends the code to the Backend via HttpClient
        var authResponse = await _webService.ArelympClient.ReceiveCodeAsync(code);

        if (authResponse == null || authResponse.Token == "")
        {
            throw new ArgumentNullException("AuthToken is null");
        }

        // Saves the token for this session
        _webService.AddAuthenticationToken(authResponse.Token);

        if (authResponse.User == null)
        {
            OpenRegistrationPanel();
            Debug.Log("Starting register process");
        } 
        else
        {
            _cacheService.User = authResponse.User;
            SceneManager.LoadScene(SceneNames.MainMenu);
        }
    });
}
 ```

 The following snippet shows how the backend processes the code. The code ###Link### is located here.
 ProducesResponseType is an attribute for the API documentation (Swagger) that helps for the API client implementation/generation. 

##### AuthenticationContoller.cs
 ```C#
    [HttpPost("receive-code")]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TextResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ReceiveCode([FromForm] string code, CancellationToken cancellationToken)
    {
        try
        {
            // Sends code to the Google Cloud to authenticate
            var tokenResponse = await _googleApiService.GetOauth2TokenResponse(code);

            if (tokenResponse == null)
            {
                return BadRequest(new TextResponseDto("Failed to exchange code for token."));
            }

            var playerId = await _googleApiService.GetPlayerId(tokenResponse.AccessToken);
            var userDto = await _userService.GetUserDataWithEquipAsync(playerId, Platform.Google, cancellationToken);

            // Creates a custom JWT for future requests
            var customJwt = GenerateCustomJwt(playerId);

            return Ok(new AuthenticationResponse(userDto, customJwt));
        }
        catch (Exception ex)
        {
            return BadRequest(new TextResponseDto(ex.Message));
        }
    }

    [HttpPost("register-google")]
    [Authorize]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TextResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RegisterGoogle([FromBody] RegisterModel registerModel, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetRawUserId();
        var platform = HttpContext.GetPlatform();

        //TODO: Validate name format
        //TODO: Validate for existing name

        var userDto = await _userService.GetUserDataWithEquipAsync(userId, platform, cancellationToken);

        if (userDto != null)
        {
            return BadRequest(new TextResponseDto("You already have a registered Account"));
        }

        userDto = await _userService.RegisterNewUserAsync(userId, platform, registerModel.UserName, cancellationToken);

        return Ok(userDto);
    }

 ```

##### GoogleApiService.cs
 ```C#
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
 ```

By generating a custom JWT for future requests of the current session, the [Authorize] Attribute can be used for authentication inside the endpoints/controller when using the corresponding nuget packages.



### User (Client) to Unity Mirror Gameserver (Server) Authentication

In order to authenticate to unity mirror server that is started and managed by the OpenMatch matchmaker. For more info of the matchmaker go to the section ###LINK###

![image](./ClientServerAuth.png)
##### Source: Matthias Feil

The Mirror NetworkManager default authentication method needs to be disabled by overriding it and throwing an error if this unsupported method is executed.

##### CustomNetworkManager.cs
```C#
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            Debug.LogError("The default connection method is not supported");
        }
```

A separate authentication method needs to be added that also passes an CustomAddPlayerMessage where the token is passed. Whenever the player tries to connect the token is passed and challenged to the backend (Step 2 of the diagram).

```C#
        public override void OnStartServer()
        {
            NetworkServer.RegisterHandler<CustomAddPlayerMessage>(OnAuthMessageReceived);

            _webService = ServiceLocator.Instance.Resolve<IWebService>();
        }

        public async void OnAuthMessageReceived(NetworkConnectionToClient conn, CustomAddPlayerMessage msg)
        {
            Debug.Log($"{conn.address} joined... validating token");

            var result = await _webService.ValidateUserTokenForId(msg.UserToken);

            if (result == null)
            {
                Debug.LogError("Token validation failed. Disconnecting...");
                conn.Disconnect();
                return;
            }

            var playerInstance = Instantiate(playerPrefab);
            NetworkServer.AddPlayerForConnection(conn, playerInstance);

            SetupPlayer(result);
        }
```

#### GameServerController.cs
```C#
        [HttpGet("Get-Equip-By-Token")]
        [Authorize]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEquipByToken(CancellationToken cancellationToken)
        {
            var userId = HttpContext.GetRawUserId();
            var platform = HttpContext.GetPlatform();

            var user = await _userService.GetUserDataWithEquipAsync(userId, platform, cancellationToken);

            user.Id = HttpContext.GetCompleteUserId();
        
            return Ok(user);
        }
```

If the authentication succeeded the player/client gets setup and informed by the gameserver.

Additional Resources:
https://github.com/google/play-unity-plugins
https://mirror-networking.gitbook.io/docs