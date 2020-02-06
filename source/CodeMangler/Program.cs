using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommandLine;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeMangler
{
    public static class Program
    {
        private const string OutputText = @"
#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using BeeSharp.Utils;

";


        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(o =>
                {
                    var files = Directory.GetFiles(o.InputDir, "*.cs", SearchOption.AllDirectories);

                    StringBuilder sb = new StringBuilder(OutputText);

                    foreach (var f in files)
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

                    File.WriteAllText(o.OutputFile, sb.ToString(), Encoding.UTF8);
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
    }
}
