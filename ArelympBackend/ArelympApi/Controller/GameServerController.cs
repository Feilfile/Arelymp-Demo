using ArelympApi.Security;
using BusinessDomain.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectMobileApi.Extensions;
using SharedLibrary;

namespace ArelympApi.Controller
{
    [ApiController]
    [Route("GameServer")]
    [ApiKey]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class GameServerController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        private readonly IUserService _userService;

        public GameServerController(IInventoryService inventoryService, IUserService userService) 
        { 
            _inventoryService = inventoryService;
            _userService = userService;
        }

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

        [HttpGet("Get-Equip-List")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Dictionary<string, EquipDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEquipList([FromQuery] string ids, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                return BadRequest("No IDs provided.");
            }

            var idArray = ids.Split(',');  // Split the comma-separated string into an array
            var result = new Dictionary<string, EquipDto>();

            foreach (var id in idArray) 
            {
                result.Add(id, await _inventoryService.GetEquipOfUser(id, cancellationToken));
            }

            return Ok(result);
        }

        #if DEBUG
        [HttpGet()]    
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Test(CancellationToken cancellationToken)
        {
            return Ok("TestResult");
        }
        #endif
    }
}
