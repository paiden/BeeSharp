namespace BeeSharp.Extensions
{
    public static class StringExtensions
    {
        public static string EnsureEndsWith(this string s, char ending)
            => s.EndsWith(ending)
            ? s
            : s + ending;
    }
}
