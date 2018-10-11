using Karpach.Remote.Keep.Command.Helpers;
using Newtonsoft.Json;

namespace Karpach.Remote.Keep.Command.Models
{
    public class ClientVersion
    {
        [JsonProperty("major")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Major { get; set; }

        [JsonProperty("build")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Build { get; set; }

        [JsonProperty("minor")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Minor { get; set; }

        [JsonProperty("revision")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Revision { get; set; }
    }
}