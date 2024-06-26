using DataAccess;
using Microsoft.EntityFrameworkCore;
using SharedLibrary;

namespace BusinessDomain.Services.Abstract
{
    public interface ICharacterLoadoutService
    {
        Task<CharacterLoadoutDto> GetCharacterLoadoutAsync(GameDbContext dbContext,
            string userId,
            int newCharacterId,
            CancellationToken cancellationToken);

        Task UpdateCharacterSkinAsync(DbContext dbContext,string userId, int characterId, int characterSkinId, CancellationToken cancellationToken);

        Task UpdateWeaponSkinAsync(DbContext dbContext,string userId, int characterId, int weaponSkinId, CancellationToken cancellationToken);
    }
}
