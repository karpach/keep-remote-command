using Newtonsoft.Json;

namespace Karpach.Remote.Keep.Command.Models
{
    public class KeepResponse
    {
        [JsonProperty("toVersion")]
        public string ToVersion { get; set; }        

        [JsonProperty("userInfo")]
        public UserInfo UserInfo { get; set; }

        [JsonProperty("nodes")]
        public Node[] Nodes { get; set; }
    }
}