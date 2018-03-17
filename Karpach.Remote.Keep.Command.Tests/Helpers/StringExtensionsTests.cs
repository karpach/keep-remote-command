using Karpach.Remote.Keep.Command.Helpers;
using NUnit.Framework;

namespace Karpach.Remote.Keep.Command.Tests.Helpers
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [TestCase(null, ExpectedResult = null)]
        [TestCase("", ExpectedResult = null)]
        [TestCase("C:\\Tmp\\", ExpectedResult = "C:/Tmp/")]
        [TestCase("C:\\Tmp\\Default", ExpectedResult = "C:/Tmp/")]
        [TestCase("C:\\Tmp\\Default\\", ExpectedResult = "C:/Tmp/")]
        [TestCase("C:/Tmp/Default", ExpectedResult = "C:/Tmp/")]
        public string NormalizeChromeProfileFolder_Tests(string path)
        {
            return path.NormalizeChromeProfileFolder();
        }
    }
}