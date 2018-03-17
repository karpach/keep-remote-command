using Karpach.Remote.Commands.Base;

namespace Karpach.Remote.Keep.Command
{
    public class KeepCommandSettings : CommandSettingsBase
    {
        public string CommandName { get; set; }
        public int? ExecutionDelay { get; set; }
        public string ListId { get; set; }        
        public string GoogleUserName { get; set; }
        public string GooglePassword { get; set; }
        public bool Headless { get; set; }
        public string ChromeProfileFolder { get; set; }
    }
}   