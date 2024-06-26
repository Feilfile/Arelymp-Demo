using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDomain.Services.Abstract;

public interface IMatchmakingService
{
    Task<string> ProcessMatchmakingAsync(UserDto userId, string userIp, string gameMode, CancellationToken cancellationToken);
}
