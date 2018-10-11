using Newtonsoft.Json;

namespace Karpach.Remote.Keep.Command.Models
{
    public class UserInfo
    {
        [JsonProperty("labels")]
        public Label[] Labels { get; set; }
    }
}