using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Karpach.Remote.Keep.Command.Helpers
{
    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
        };
    }
}