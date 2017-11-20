using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace Karpach.Remote.Keep.Command.Tests
{
    [TestFixture]
    public class KeepCommandTests
    {
        [Test]
        public void Test()
        {
            Assembly assembly = typeof(KeepCommandSettings).Assembly;
            string iniFilePath = $"{(object)Path.GetDirectoryName(assembly.Location)}\\{(object)assembly.GetName().Name}.ini";
            File.WriteAllText(iniFilePath, @"
[Karpach.Remote.Keep.Command.KeepCommandSettings]
CommandName = Keep
ExecutionDelay = 1000
ListId = 12345678ccc.12345678cccccccc
ChromeProfileFolder = c:\Users\ComputerName\AppData\Local\Google\Chrome\User Data\Profile Test
Id = 41d5f7ea-6e02-4c91-8c25-5100e1240952                
            ");
            var command = new KeepCommand();            
            //command.ShowSettings();
            command.RunCommand("Конфеты");            
        }
    }
}
