using System;

using static BeeSharp.Utils.PlatformUtil;

namespace BeeSharp.Internal
{
    #if WINDOWS
    internal static partial class PathStringUtils
    {
        public const char PathSeparator = '\\';
            
        public const char AltSeparator = '/';
        public const StringComparison PathComparisonType = StringComparison.OrdinalIgnoreCase;
    }
    #else
    internal static partial class PathStringUtils
    {
        public const char PathSeparator = '/';
            
        public const char AltSeparator = '/';
        public const StringComparison PathComparisonType = StringComparison.Ordinal;
    }
    #endif
}
