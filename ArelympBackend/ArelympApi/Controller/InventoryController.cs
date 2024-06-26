using BusinessDomain.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectMobileApi.Extensions;
using SharedLibrary;
using SharedLibrary.Enum;

namespace ProjectMobileApi.Controller
{
    [Authorize]
    [ApiController]
    [Route("Inventory")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService) 
        { 
            _inventoryService = inventoryService;
        }

        [HttpGet("Equip")]
        [ProducesResponseType(typeof(EquipDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEquipedItems(CancellationToken cancellationToken)
        {
            var userId = HttpContext.GetCompleteUserId();

            var equip = await _inventoryService.GetEquipOfUser(userId, cancellationToken);

            return Ok(equip);
        }


        /// <response code="204" nullable="true">No data.</response>
        [HttpPut("equip")]
        [ProducesResponseType(typeof(EquipDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(EquipDto), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateEquip([FromBody] PutEquipDto model, CancellationToken cancellationToken)
        {
            var userId = HttpContext.GetCompleteUserId();

            var equip = await _inventoryService.EquipItem(userId, model.ItemId, model.Tier, cancellationToken);

            return Ok(equip);
        }

        [HttpGet("all-items")]
        [ProducesResponseType(typeof(IList<ItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllItemsOfUser(CancellationToken cancellationToken) 
        {
            var userId = HttpContext.GetCompleteUserId();

            var response = await _inventoryService.GetAllItems(userId, cancellationToken);

            return Ok(response);
        }

        [HttpGet("all-items/{equipSlot}")]
        public async Task<IActionResult> GetAllItemsOfSlot([FromRoute] EquipSlot equipSlot, CancellationToken cancellationToken)
        {
            var userId = HttpContext.GetCompleteUserId();

            var items = await _inventoryService.GetAssignedItemsOfSlot(userId, equipSlot, cancellationToken);

            return Ok(items);
        }
    }
}
