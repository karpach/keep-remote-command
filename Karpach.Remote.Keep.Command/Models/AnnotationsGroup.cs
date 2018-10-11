using Newtonsoft.Json;

namespace Karpach.Remote.Keep.Command.Models
{
    public class AnnotationsGroup
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }
    }
}