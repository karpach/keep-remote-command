using Newtonsoft.Json;

namespace Karpach.Remote.Keep.Command.Models
{
    public class RequestHeader
    {
        [JsonProperty("clientSessionId")]
        public string ClientSessionId { get; set; }

        [JsonProperty("clientVersion")]
        public ClientVersion ClientVersion { get; set; }

        [JsonProperty("clientPlatform")]
        public string ClientPlatform { get; set; }

        [JsonProperty("capabilities")]
        public Capability[] Capabilities { get; set; }
    }
}