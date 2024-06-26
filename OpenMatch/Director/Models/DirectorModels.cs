using System.Text.Json.Serialization;

namespace Director.Models
{
    public struct OpenMatchMatchProfile
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("pools")]
        public OpenMatchPool[] Pools { get; set; }
        [JsonPropertyName("extensions")]
        public Dictionary<string, ProtobufAny> Extensions { get; set; }
    }

    // Based On ProtobufAny in https://open-match.dev/site/swaggerui/index.html?urls.primaryName=Backend
    public struct OpenMatchPool
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("tag_present_filters")]
        public TagPresentFilter[] TagPresentFilters { get; set; }
    }

    // Based On TagPresentFilter in https://open-match.dev/site/swaggerui/index.html?urls.primaryName=Backend
    public struct TagPresentFilter
    {
        [JsonPropertyName("tag")]
        public string Tag { get; set; }
    }

    // Based On ProtobufAny in https://open-match.dev/site/swaggerui/index.html?urls.primaryName=Backend
    public struct ProtobufAny
    {
        [JsonPropertyName("@type")]
        public string TypeUrl { get; set; }
        [JsonPropertyName("value")]
        public byte[] Value { get; set; }
    }


    public struct MMFConfig
    {
        [JsonPropertyName("host")]
        public string Host { get; set; }
        [JsonPropertyName("port")]
        public int Port { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    // Based On Stream result in https://open-match.dev/site/swaggerui/index.html?urls.primaryName=Backend
    public struct OpenMatchStreamResult<T>
    {
        [JsonPropertyName("result")]
        public T Result { get; set; }
    }

    // Based On OpenMatchFetchMatchesResponse in https://open-match.dev/site/swaggerui/index.html?urls.primaryName=Backend
    public struct OpenMatchFetchMatchesResponse
    {
        [JsonPropertyName("match")]
        public OpenMatchMatch Match { get; set; }
    }

    // Based On OpenMatchMatch in https://open-match.dev/site/swaggerui/index.html?urls.primaryName=Backend
    public struct OpenMatchMatch
    {
        [JsonPropertyName("match_id")]
        public string MatchId { get; set; }
        [JsonPropertyName("match_profile")]
        public string MatchProfile { get; set; }
        [JsonPropertyName("match_function")]
        public string MatchFunction { get; set; }
        [JsonPropertyName("tickets")]
        public OpenMatchTicket[] Tickets { get; set; }
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
}
