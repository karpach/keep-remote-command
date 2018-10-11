using Karpach.Remote.Keep.Command.Helpers;
using Newtonsoft.Json;

namespace Karpach.Remote.Keep.Command.Models
{
    public class Node
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("annotationsGroup")]
        public AnnotationsGroup AnnotationsGroup { get; set; }

        [JsonProperty("isPinned")]
        public bool IsPinned { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("nodeSettings")]
        public NodeSettings NodeSettings { get; set; }

        [JsonProperty("sortValue")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long SortValue { get; set; }

        [JsonProperty("labelIds")]
        public LabelId[] LabelIds { get; set; }

        [JsonProperty("parentId")]
        public string ParentId { get; set; }

        [JsonProperty("isArchived")]
        public bool IsArchived { get; set; }

        [JsonProperty("timestamps")]
        public Timestamps Timestamps { get; set; }

        [JsonProperty("serverId")]
        public string ServerId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}