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

        public static string CheckMinLengthOf(this string s, int len, string argName)
            => s.Length >= len
            ? s
            : throw new ArgumentException($"Expected string '{argName}' to have a minimum length of '{len}' but it has value '{s}' with a length of '{s.Length}'.");
    }
}
