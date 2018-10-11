using System;
using Newtonsoft.Json;

namespace Karpach.Remote.Keep.Command.Models
{
    public class KeepRequest
    {
        [JsonProperty("clientTimestamp")]
        public DateTimeOffset ClientTimestamp { get; set; }

        [JsonProperty("requestHeader")]
        public RequestHeader RequestHeader { get; set; }

        [JsonProperty("nodes")]
        public Node[] Nodes { get; set; }

        [JsonProperty("targetVersion")]
        public string TargetVersion { get; set; }
    }
}