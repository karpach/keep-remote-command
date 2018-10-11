using Newtonsoft.Json;

namespace Karpach.Remote.Keep.Command.Models
{
    public class NodeSettings
    {
        [JsonProperty("newListItemPlacement")]
        public string NewListItemPlacement { get; set; }

        [JsonProperty("graveyardState")]
        public string GraveyardState { get; set; }

        [JsonProperty("checkedListItemsPolicy")]
        public string CheckedListItemsPolicy { get; set; }
    }
}