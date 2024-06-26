using SharedLibrary;
using SharedLibrary.Enum;

namespace BusinessDomain.Services.Abstract
{
    public interface IInventoryService
    {
        public Task<IList<ItemDto?>> GetAssignedItemsOfSlot(string userId, EquipSlot slot, CancellationToken cancellationToken);
        
        public Task<IList<ItemDto?>> GetAllItems(string userId, CancellationToken cancellationToken);

        public Task<EquipDto> GetEquipOfUser(string userId, CancellationToken cancellationToken);

        public Task<EquipDto?> EquipItem(string userId, int item, int? tier, CancellationToken cancellationToken);
    }
}
