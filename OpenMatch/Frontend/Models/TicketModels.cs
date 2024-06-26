using System.Net;
using System.Text.Json.Serialization;

namespace Frontend.Models
{
    public class TicketModels
    {
        // Represent the model tath we should receive for our create ticket endpoint
        public struct CreateTicketPayload
        {
            [JsonPropertyName("mode")]
            public string Mode { get; set; }

            public string IpAddress { get; set; }

            public string PlayerId { get; set; }
        }

        // Based On OpenMatchCreateTicketRequest in https://open-match.dev/site/swaggerui/index.html?urls.primaryName=Frontend
        public struct OpenMatchCreateTicketRequest
        {
            [JsonPropertyName("ticket")]
            public OpenMatchTicket Ticket { get; set; }
        }

        // Based On OpenMatchTicket in https://open-match.dev/site/swaggerui/index.html?urls.primaryName=Frontend
        public struct OpenMatchTicket
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }
            [JsonPropertyName("search_fields")]
            public OpenMatchSearchFields SearchFields { get; set; }
            [JsonPropertyName("extensions")]
            public Dictionary<string, ProtobufAny> Extensions { get; set; }
        }

        // Based On OpenMatchSearchFields in https://open-match.dev/site/swaggerui/index.html?urls.primaryName=Frontend
        public struct OpenMatchSearchFields
        {
            [JsonPropertyName("tags")]
            public string[] Tags { get; set; }
        }

        // Based On ProtobufAny in https://open-match.dev/site/swaggerui/index.html?urls.primaryName=Frontend
        public struct ProtobufAny
        {
            [JsonPropertyName("@type")]
            public string TypeUrl { get; set; }
            [JsonPropertyName("value")]
            public byte[] Value { get; set; }
        }
    }
}
