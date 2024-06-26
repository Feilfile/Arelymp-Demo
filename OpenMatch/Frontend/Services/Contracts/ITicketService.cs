using static Frontend.Models.TicketModels;

namespace Frontend.Services.Contracts
{
    public interface ITicketService
    {
        Task<string> CreateTicket(CreateTicketPayload payload, CancellationToken cancellationToken);

        Task<string> GetTicket(string ticketId, CancellationToken cancellationToken);

        Task<string> DeleteTicket(string ticketId, CancellationToken cancellationToken);
    }
}
