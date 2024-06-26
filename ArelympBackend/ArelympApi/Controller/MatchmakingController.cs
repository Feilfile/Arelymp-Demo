using BusinessDomain.Constants;
using BusinessDomain.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectMobileApi.Extensions;
using SharedLibrary;

namespace ArelympApi.Controller
{
    [Authorize]
    [ApiController]
    [Route("matchmaking")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class MatchmakingController : ControllerBase
    {
        private readonly IMatchmakingService _matchmakingService;

        private readonly IUserService _userService;

        public MatchmakingController(IMatchmakingService matchmakingService, IUserService inventoryService) 
        { 
            _matchmakingService = matchmakingService;
            _userService = inventoryService;
        }

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
    }
}
