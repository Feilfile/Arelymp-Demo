using BusinessDomain.Constants;
using BusinessDomain.Models.Authentication;
using BusinessDomain.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjectMobileApi.Extensions;
using SharedLibrary;
using SharedLibrary.Enum;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ArelympApi.Controller;

[ApiController]
[Route("api/[Controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _configuration;

    private readonly IGoogleApiService _googleApiService;

    private readonly IUserService _userService;

    public AuthenticationController(IConfiguration configuration, IGoogleApiService googleApiService, IUserService userService)
    {
        _configuration = configuration;
        _googleApiService = googleApiService;
        _userService = userService;
    }

    [HttpPost("receive-code")]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TextResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ReceiveCode([FromForm] string code, CancellationToken cancellationToken)
    {
        try
        {
            var tokenResponse = await _googleApiService.GetOauth2TokenResponse(code);

            if (tokenResponse == null)
            {
                return BadRequest(new TextResponseDto("Failed to exchange code for token."));
            }

            var playerId = await _googleApiService.GetPlayerId(tokenResponse.AccessToken);

            var userDto = await _userService.GetUserDataWithEquipAsync(playerId, Platform.Google, cancellationToken);

            //if (userDto == null) { return Ok(new AuthenticationResponse(null, null)); }

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

    //#if DEBUG
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
    [HttpGet("dev-authentication/{userId}")]
    public async Task<IActionResult> DevAuth([FromRoute] string userId, CancellationToken cancellationToken)
    {
        var token = GenerateCustomJwt(userId);
        var userDto = await _userService.GetUserDataWithEquipAsync(userId, Platform.Google, cancellationToken);

        return Ok(new AuthenticationResponse(userDto, token));
    }
    //#endif

    private string GenerateCustomJwt(string userId)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Settings:JwtSecretKey"]); // Replace with a secure secret key
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(RequestHeaders.Platform, Platform.Google.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0,
                DateTimeKind.Utc)).TotalSeconds.ToString(), ClaimValueTypes.Integer64),
            new Claim(JwtRegisteredClaimNames.Exp, (DateTime.UtcNow + TimeSpan.FromHours(1)).Subtract(new DateTime(1970, 1, 1, 0, 0, 0,
                DateTimeKind.Utc)).TotalSeconds.ToString(), ClaimValueTypes.Integer64),
        };

        var token = new JwtSecurityToken(
            issuer: "https://arelymp.com",
            audience: "https://arelymp.com",
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}