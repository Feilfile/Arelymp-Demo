using System.Text.Json.Serialization;

namespace Director.Models
{
    // Based on https://docs.edgegap.com/api/#operation/deploy
    public struct ArbitriumDeployResponse
    {
        [JsonPropertyName("request_id")]
        public string RequestId { get; set; }
    }

    // Based on https://docs.edgegap.com/api/#operation/deployment-status-get
    public struct ArbitriumDeploymentRequestStatusResponse
    {
        [JsonPropertyName("current_status")]
        public string CurrentStatus { get; set; }
        [JsonPropertyName("public_ip")]
        public string PublicIP { get; set; }
        [JsonPropertyName("ports")]
        public Dictionary<string, DeploymentPort> Ports { get; set; }

    }

    // Based on https://docs.edgegap.com/api/#operation/deployment-status-get
    public struct DeploymentPort
    {
        [JsonPropertyName("external")]
        public int External { get; set; }
        [JsonPropertyName("internal")]
        public int Internal { get; set; }
        [JsonPropertyName("protocol")]
        public string Protocol { get; set; }
    }

    // Based on OpenMatchAssignTicketsRequest https://open-match.dev/site/swaggerui/index.html?urls.primaryName=Backend
    public struct OpenMatchAssignTicketsRequest
    {
        [JsonPropertyName("assignments")]
        public OpenMatchAssignmentGroup[] Assignments { get; set; }
    }

    // Based on OpenMatchAssignmentGroup https://open-match.dev/site/swaggerui/index.html?urls.primaryName=Backend
    public struct OpenMatchAssignmentGroup
    {
        [JsonPropertyName("ticket_ids")]
        public string[] TicketIds { get; set; }
        [JsonPropertyName("assignment")]
        public OpenMatchAssignment Assignment { get; set; }
    }

    // Based on OpenMatchAssignment https://open-match.dev/site/swaggerui/index.html?urls.primaryName=Backend
    public struct OpenMatchAssignment
    {
        [JsonPropertyName("connection")]
        public string Connection { get; set; }
    }
}
