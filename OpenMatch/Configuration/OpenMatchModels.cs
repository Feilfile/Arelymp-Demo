using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Configuration
{
    // Based On OpenMatchTicket in https://open-match.dev/site/swaggerui/index.html?urls.primaryName=MatchFunction
    public struct OpenMatchTicket
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("assignment")]
        public Assignmnt Assignment { get; set; }

        [JsonPropertyName("search_fields")]
        public OpenMatchSearchFields SearchFields { get; set; }

        [JsonPropertyName("extensions")]
        public Dictionary<string, ProtobufAny> Extensions { get; set; }

        [JsonPropertyName("create_time")]
        public string CreateTime { get; set; }
    }

    public struct Assignmnt
    {
        [JsonPropertyName("connection")]
        public string Connection { get; set; }
    }

    // Based On OpenMatchSearchFields in https://open-match.dev/site/swaggerui/index.html?urls.primaryName=Query
    public struct OpenMatchSearchFields
    {
        [JsonPropertyName("tags")]
        public string[] Tags { get; set; }
    }

    // Based On ProtobufAny in https://open-match.dev/site/swaggerui/index.html?urls.primaryName=MatchFunction
    public struct ProtobufAny
    {
        [JsonPropertyName("@type")]
        public string TypeUrl { get; set; }

        [JsonPropertyName("value")]
        public byte[] Value { get; set; }
    }
}
