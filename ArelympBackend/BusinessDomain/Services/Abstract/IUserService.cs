using BusinessDomain.Constants;
using SharedLibrary;
using SharedLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDomain.Services.Abstract
{
    public interface IUserService
    {
        public Task<UserDto?> GetUserDataAsync(string userId, CancellationToken cancellationToken);

        public Task<UserDto?> GetUserDataWithEquipAsync(string userId, CancellationToken cancellationToken);

        public Task<UserDto?> GetUserDataWithEquipAsync(string userId, Platform platform, CancellationToken cancellationToken);

        public Task<UserDto> RegisterNewUserAsync(string userId, Platform platfotm, string username, CancellationToken cancellationToken);
    }
}
