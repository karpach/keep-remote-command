using System;
using System.Text.RegularExpressions;

namespace Karpach.Remote.Keep.Command.Helpers
{
    public static class StringExtensions
    {
        public static string NormalizeChromeProfileFolder(this string folder)
        {
            if (string.IsNullOrEmpty(folder))
            {
                return null;
            }
            string result = folder.Replace("\\", "/");
            result = Regex.Replace(result, "/Default/?$", "/");
            return result;
        }
    }
}