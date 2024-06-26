using Director.Models;
using System.Text;

namespace Director.Extensions
{
    public static class TicketExtensions
    {
        public static string SerializeRequiredTicketData(this OpenMatchTicket[] tickets)
        {
            var environmentModels = tickets.Select(ticket => new EnvironmentModel
            {
                ticketId = ticket.Id,
                playerId = Encoding.UTF8.GetString(ticket.Extensions.GetValueOrDefault("playerId").Value)
            });

            return System.Text.Json.JsonSerializer.Serialize(environmentModels);
        }
    }
}
