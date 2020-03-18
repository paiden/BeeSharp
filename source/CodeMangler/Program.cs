using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommandLine;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeMangler
{
    public static class Program
    {

        private const string AcceesModPreprocA1 = @"
#if BEES_INTERNAL
    internal
#else
    public
#endif
    {0} ";

        private const string OutputText = @"
#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using static System.Diagnostics.Debug;

using BeeSharp.Extensions;
using BeeSharp.Internal;
using BeeSharp.Utils;
using BeeSharp.Validation;

using static BeeSharp.Internal.PathStringUtils;

";


        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(o =>
                {
                    var files = Directory.GetFiles(o.InputDir.Trim(), "*.cs", SearchOption.AllDirectories);

                    StringBuilder sb = new StringBuilder(OutputText);

                    foreach (var f in files.Where(f => !InputFilters.Any(fp => f.Contains(fp, System.StringComparison.OrdinalIgnoreCase))))
                    {
                        var nameSpaces = GetNamespaceNodes(f);

                        if (AddPreprocStatements(f))
                        {
                            var ppn = GetPreProcName(f);
                            sb.AppendLine($"#if (!BEES_OPTIN && !BEES_OPTOUT_{ppn}) || (BEES_OPTIN && BEES_OPTIN_{ppn})")
                                .AppendLine();
                        }

                        foreach (var ns in nameSpaces)
                        {
                            sb.Append(ns.ToString())
                                .AppendLine()
                                .AppendLine();
                        }

                        if (AddPreprocStatements(f))
                        {
                            sb.AppendLine("#endif")
                                .AppendLine();
                        }
                    }

                    var dir = Path.GetDirectoryName(o.OutputFile);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    // Add access Modifiers
                    sb.Replace("public class ", string.Format(AcceesModPreprocA1, "class"));
                    sb.Replace("public struct ", string.Format(AcceesModPreprocA1, "struct"));
                    sb.Replace("public sealed class ", string.Format(AcceesModPreprocA1, "sealed class"));
                    sb.Replace("public static class ", string.Format(AcceesModPreprocA1, "static class"));
                    sb.Replace("public interface ", string.Format(AcceesModPreprocA1, "interface"));
                    sb.Replace("public enum ", string.Format(AcceesModPreprocA1, "enum"));

                    File.WriteAllText(o.OutputFile, sb.ToString(), Encoding.UTF8);

                    if (o.Template)
                    {
                        sb.Replace("namespace BeeSharp", "namespace $rootnamespace$")
                            .Replace("using BeeSharp", "using $rootnamespace$");
                        File.WriteAllText($"{o.OutputFile}.pp", sb.ToString(), Encoding.UTF8);
                    }
                });
        }

        private static IEnumerable<NamespaceDeclarationSyntax> GetNamespaceNodes(string file)
        {
            var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(file));

            return tree.GetRoot().DescendantNodes()
                .OfType<NamespaceDeclarationSyntax>();
        }

        private static string GetPreProcName(string path)
        {
            path = path.Replace(".g.cs", ".cs");
            var file = Path.GetFileNameWithoutExtension(path);

            var dotSuffix = file.IndexOf(".");
            if (dotSuffix > 0)
            {
                file = file.Substring(0, dotSuffix);
            }

            return file.ToUpperInvariant();
        }

        private static bool AddPreprocStatements(string filePath)
            => !NoPreProcessor.Any(p => filePath.EndsWith(p, System.StringComparison.OrdinalIgnoreCase));

        private static readonly List<string> NoPreProcessor = new List<string>()
        {
            "AnalyzerMarkers.cs",
            "AssemblyInfo.cs",
            "BeeSharpConstants.cs",
            "Error.cs",
        };

        private static List<string> InputFilters { get; } = new List<string>()
        {
            @"\obj\",
            @"\bin\",
        };
    }
}
