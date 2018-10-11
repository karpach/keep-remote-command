using System;
using Newtonsoft.Json;

namespace Karpach.Remote.Keep.Command.Models
{
    public class Timestamps
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("deleted")]
        public DateTimeOffset Deleted { get; set; }

        [JsonProperty("updated")]
        public DateTimeOffset Updated { get; set; }

        [JsonProperty("userEdited")]
        public DateTimeOffset UserEdited { get; set; }

        [JsonProperty("trashed")]
        public DateTimeOffset Trashed { get; set; }
    }
}