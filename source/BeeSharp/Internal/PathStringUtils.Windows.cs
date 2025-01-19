using System;

namespace BeeSharp.Internal
{
    internal static partial class PathStringUtils
    {
        public const char PathSeparator = '\\';
        public const char AltSeparator = '/';
        public const StringComparison PathComparisonType = StringComparison.OrdinalIgnoreCase;
    }
}
