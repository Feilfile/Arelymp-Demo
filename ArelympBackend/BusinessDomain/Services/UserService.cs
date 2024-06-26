using BusinessDomain.Extensions;
using BusinessDomain.Services.Abstract;
using DataAccess;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using SharedLibrary;
using SharedLibrary.Enum;

namespace BusinessDomain.Services
{
    public class UserService : IUserService
    {
        private GameDbContext _dbContext;

        // TODO: MOVE IN CONFIG
        private readonly int defaultElo = 2000;
        private readonly int defaultLevel = 1;
        private readonly int defaultExp = 0;

        public UserService(GameDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<UserDto?> GetUserDataWithEquipAsync(string userId, Platform platform, CancellationToken cancellationToken)
        {
            var userEntity = await _dbContext.Users
                //.Include(u => u.Equip)
                .FirstOrDefaultAsync(u => u.Id == userId.AddPlatformPrefix(platform), cancellationToken);

            if (userEntity == null) 
            {
                return null;
            }
            
            return userEntity.ToUserDto();
        }

        public async Task<UserDto?> GetUserDataWithEquipAsync(string userId, CancellationToken cancellationToken)
        {
            var userEntity = await _dbContext.Users
                .Include(u => u.Equip)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (userEntity == null)
            {
                return null;
            }

            return userEntity.ToUserDto();
        }

        public async Task<UserDto> RegisterNewUserAsync(string userId, 
            Platform platform, 
            string username, 
            CancellationToken cancellationToken)
        {
            var uid = userId.AddPlatformPrefix(platform);
            User user = new User()
            {
                Id = uid,
                Name = username,
                Email = "TODO",
                Elo = default,
                Level = defaultLevel,
                Experience = defaultExp,
                Equip = new Equip
                {
                    UserId = uid,
                }

            };

            try
            {
                var res = await _dbContext.Users.AddAsync(user, cancellationToken);
                _ = await _dbContext.SaveChangesAsync(cancellationToken);
                return res.Entity.ToUserDto();
            } catch (Exception ex) 
            {
                throw ex;
            }
        }

        public async Task<UserDto?> GetUserDataAsync(string userId, CancellationToken cancellationToken)
        {
            var userEntity = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (userEntity == null)
            {
                return null;
            }

            return userEntity.ToUserDto();
        }
    }
}
