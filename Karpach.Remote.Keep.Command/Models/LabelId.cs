using System;
using Newtonsoft.Json;

namespace Karpach.Remote.Keep.Command.Models
{
    public class LabelId
    {
        [JsonProperty("deleted")]
        public DateTimeOffset Deleted { get; set; }

        [JsonProperty("labelId")]
        public string LabelIdLabelId { get; set; }
    }
}