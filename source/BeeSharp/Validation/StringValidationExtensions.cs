using System;

namespace BeeSharp.Validation
{
    public static class StringValidationExtensions
    {
        public static bool IsTrimmed(this string s)
            => s.Trim().Length == s.Length;

        public static string CheckTrimmed(this string s, string argName)
            => s.IsTrimmed()
            ? s
            : throw new ArgumentException($"Expected string '{argName}' to be trimmed but it has untrimmed value of '{s}'.");
    }
}
