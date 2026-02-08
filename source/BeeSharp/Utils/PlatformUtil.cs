using System;
using System.Runtime.InteropServices;

namespace BeeSharp.Utils;

internal static class PlatformUtil
{
    public static T PlatformSpecific<T>(T windows, T linux, T osx)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return windows;
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return linux;
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return osx;
        }
        else
        {
            throw new NotSupportedException("Code executed on unsupported platform.");
        }
    }
}