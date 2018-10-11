using Karpach.Remote.Keep.Command.Models;
using Newtonsoft.Json;

namespace Karpach.Remote.Keep.Command.Helpers
{
    public static class Serialize
    {
        public static string ToJson(this KeepRequest self) => JsonConvert.SerializeObject(self, Converter.Settings);
        public static KeepResponse FromJson(string json) => JsonConvert.DeserializeObject<KeepResponse>(json, Converter.Settings);
    }
}