
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Diagnostics.Debug;

namespace BeeSharp.Internal
{
    internal static partial class PathStringUtils
    {
        private const string CurrentDir = ".";
        private const string ParentDir = "..";

        public static string Normalize(string s)
        {
            Assert(s.IndexOf(AltSeparator) < 0, "String should already be fixed");
            Assert(s.Trim().Length == s.Length, "String should already be fixed");

            var components = ExtractUsefulComponents(s);
            return ConstructPathFrom(components, s);
        }

        private static string ConstructPathFrom(IList<string> components, string original)
        {
            bool addTrailingSepChar = original.Length > 0 && original[original.Length - 1] == PathSeparator;

            var sb = new StringBuilder();

            for (int i = 0; i < original.Length; i++)
            {
                if (original[i] == PathSeparator)
                {
                    sb.Append(PathSeparator);
                }
                else
                {
                    break;
                }
            }

            for (int i = 0; i < components.Count; i++)
            {
                sb.Append(components[i]);
                if (addTrailingSepChar || i != components.Count - 1)
                {
                    sb.Append(PathSeparator);
                }
            }

            return sb.ToString();
        }

        private static IList<string> ExtractUsefulComponents(string s)
        {
            var pathParts = s.Split(PathSeparator, StringSplitOptions.RemoveEmptyEntries);
            var pathElements = new List<string>();

            foreach (var pathPart in pathParts)
            {
                if (pathPart == ParentDir)
                {
                    if (IsAtDriveRoot(pathElements) || IsAtNetRoot(pathElements, s)) // Drive letter, stop at root (windows quirks stuff) do nothing
                    {
                        continue;
                    }
                    else if (pathElements.Count == 1 && pathElements[0] == CurrentDir)
                    {
                        // '.\..' -> '..\'
                        pathElements[0] = ParentDir;
                    }
                    else if (IsLastElementAFolder(pathElements))
                    {
                        pathElements.RemoveAt(pathElements.Count - 1);
                    }
                    else
                    {
                        pathElements.Add(pathPart);
                    }
                }
                else if (pathPart == CurrentDir && pathElements.Any())
                {
                    // '.\.' -> '' (when not in beginning)
                    continue;
                }
                else
                {
                    pathElements.Add(pathPart);
                }
            }

            return pathElements;
        }

        private static bool IsLastElementAFolder(IList<string> pathElements)
        {
            var last = pathElements.LastOrDefault();
            var foo = last != null && last != ParentDir;
            return foo;
        }

        private static bool IsAtNetRoot(IList<string> pathElements, string s) =>
            pathElements.Count == 1 && s.StartsWith(@"\\" + pathElements[0]);

        private static bool IsAtDriveRoot(List<string> pathElements) =>
            pathElements.Count == 1 && pathElements[0].Length > 1 && pathElements[0][1] == ':';
    }
}
