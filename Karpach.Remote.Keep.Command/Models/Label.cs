using Newtonsoft.Json;

namespace Karpach.Remote.Keep.Command.Models
{
    public class Label
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("mainId")]
        public string MainId { get; set; }

        [JsonProperty("timestamps")]
        public Timestamps Timestamps { get; set; }
    }
}