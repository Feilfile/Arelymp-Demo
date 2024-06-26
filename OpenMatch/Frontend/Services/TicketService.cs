using Frontend.Services.Contracts;
using System.Net;
using static Frontend.Models.TicketModels;
using System.Text.Json.Serialization;
using Frontend.Utils;


namespace Frontend.Services
{
    public class TicketService : ITicketService
    {
        private readonly HttpClient _httpClient;

        private const string _baseUrl = "open-match-frontend:51504";

        public TicketService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> CreateTicket(CreateTicketPayload payload, CancellationToken cancellationToken)
        {
            Console.WriteLine("Creating ticket...");

            string response = await CreateTicketToOpenMatch(payload.Mode, payload.IpAddress, payload.PlayerId);

            return response;
        }

        public async Task<string> GetTicket(string ticketId, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync($"http://{_baseUrl}/v1/frontendservice/tickets/{ticketId}", cancellationToken);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                string msg = $"ERROR - Was not able to get ticket {ticketId}: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
                Console.WriteLine(msg);
                throw new Exception(msg);
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteTicket(string ticketId, CancellationToken cancellationToken)
        {
            var response = await _httpClient.DeleteAsync($"http://{_baseUrl}/v1/frontendservice/tickets/{ticketId}", cancellationToken);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                string msg = $"ERROR - Was not able to delete ticket {ticketId}: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
                Console.WriteLine(msg);
                throw new Exception(msg);
            }

            return await response.Content.ReadAsStringAsync();
        }

        private async Task<string> CreateTicketToOpenMatch(string mode, string playerIP, string playerId)
        {
            var playerIPBytes = ByteConverter.IpToBin(playerIP);
            var playerIdBytes = ByteConverter.StringToBytes(playerId);

            // Creating the payload
            OpenMatchCreateTicketRequest body = new OpenMatchCreateTicketRequest
            {
                Ticket = new OpenMatchTicket
                {
                    SearchFields = new OpenMatchSearchFields { Tags = new string[] { mode } },
                    Extensions = new Dictionary<string, ProtobufAny>
                       {
                            {
                                "playerIp",
                                new ProtobufAny
                                {
                                    TypeUrl = "type.googleapis.com/google.protobuf.StringValue",
                                    Value = playerIPBytes
                                }
                            },
                            {
                                "playerId",
                                new ProtobufAny
                                {
                                    TypeUrl = "type.googleapis.com/google.protobuf.StringValue",
                                    Value = playerIdBytes
                                }
                            }

                    }
                }
            };

            // Sending the request to Open Match Front End
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
                $"http://{_baseUrl}/v1/frontendservice/tickets",
                body
            );

            // Check if we were able to create the ticket
            if (response.StatusCode != HttpStatusCode.OK)
            {
                string msg = $"ERROR - Was not able to create a ticket: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
                Console.WriteLine(msg);
                throw new Exception(msg);
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}
