using Newtonsoft.Json;

namespace Karpach.Remote.Keep.Command.Models
{
    public class Capability
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}