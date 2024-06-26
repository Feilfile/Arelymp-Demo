using BusinessDomain.Matchmaker;
using BusinessDomain.Services.Abstract;
using Microsoft.Extensions.Configuration;
using SharedLibrary;

namespace BusinessDomain.Services;

public class MatchmakingService : IMatchmakingService
{
    private readonly HttpClient _httpClient;
    private readonly IMatchmakerApiClient _matchmakerClient;

    public MatchmakingService(IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient;
        var mmUrl = configuration["Matchmaking:Url"];
        var mmToken = configuration["Matchmaking:Token"];
        _httpClient.BaseAddress = new Uri(mmUrl!);
        _httpClient.DefaultRequestHeaders.Add("Authorization", mmToken!);
        _matchmakerClient = new MatchmakerApiClient(mmUrl, httpClient);
    }

    public async Task<string?> ProcessMatchmakingAsync(UserDto userId, string userIp, string gameMode, CancellationToken cancellationToken)
    {
        var createTicketPayload = new CreateTicketPayload
        {
            Mode = gameMode,
            IpAddress = userIp,
            PlayerId = userId.Id,
        };

        var response = await _matchmakerClient.TicketsPOSTAsync(createTicketPayload);
        var ticketId = response.Id;

        if (string.IsNullOrEmpty(ticketId))    
        { 
            throw new ArgumentNullException("Matchmakind ticket is null"); 
        };

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var ticket = await _matchmakerClient.TicketsGETAsync(ticketId, cancellationToken);
                if (ticket?.Assignment?.Connection != null)
                {
                    // Match found
                    return ticket.Assignment.Connection;
                }
                await Task.Delay(5000, cancellationToken); 
            }
        }
        catch (OperationCanceledException)
        {
            await _matchmakerClient.TicketsDELETEAsync(ticketId);
            throw;
        }
        return null;
    }
}
