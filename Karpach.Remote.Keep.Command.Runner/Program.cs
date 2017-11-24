using System.IO;
using System.Reflection;

namespace Karpach.Remote.Keep.Command.Runner
{
    public class Program
    {
        static void Main(string[] args)
        {
            Assembly assembly = typeof(KeepCommandSettings).Assembly;
            string iniFilePath = $"{(object)Path.GetDirectoryName(assembly.Location)}\\{(object)assembly.GetName().Name}.ini";
            File.WriteAllText(iniFilePath, @"
[Karpach.Remote.Keep.Command.KeepCommandSettings]
CommandName = Keep
ExecutionDelay = 1000
ListId = 15660dde079.b79ba1bae1031212
GoogleUserName = test@gmail.com
GooglePassword = 12345
Id = 41d5f7ea-6e02-4c91-8c25-5100e1240952             
            ");
            var command = new KeepCommand();
            //command.ShowSettings();
            command.RunCommand("Sweets");
        }
    }
}
