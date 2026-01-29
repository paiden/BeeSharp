using System.CodeDom.Compiler;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace BeeSharp.SourceGenerators;

record UnionMember
{
    public UnionMember(IParameterSymbol param)
    {
        Name = param.Name;
        Type = param.Type.ToString();
        IsValueType = param.Type.IsValueType;
    }

    public string Type { get; }
    public string Name { get; }
    public bool IsValueType { get; }
};

class DiscriminatedUnion
{
    public string BeeSharpNamespace;
    public string Namespace;
    public string Name;
    public List<UnionMember> Members = new List<UnionMember>();
};

[Generator]
public class DiscriminatedUnionGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var unionsToGenerate = context.SyntaxProvider.ForAttributeWithMetadataName(
            "System.DiscriminatedUnionAttribute",
            predicate: (node, _) => true,
            transform: static (ctx, _) => GetUnionGenerationTargetFromNode(ctx)
        );

        IncrementalValueProvider<string?> rootNamespace = context
            .AnalyzerConfigOptionsProvider
            // Retrieve the RootNamespace property
            .Select((AnalyzerConfigOptionsProvider c, CancellationToken _) =>
                c.GlobalOptions.TryGetValue("build_property.RootNamespace", out var nameSpace)
                    ? nameSpace
                    : null);

        context.RegisterSourceOutput(unionsToGenerate, (spc, union) => GenerateUnion(spc, union, rootNamespace));
    }

    private static DiscriminatedUnion? GetUnionGenerationTargetFromNode(GeneratorAttributeSyntaxContext context)
    {
        
        var semantic = context.SemanticModel;
        var syntax = context.TargetNode;

        var resultType = semantic.Compilation.GetSymbolsWithName(symbol => symbol == "BeeSharpTypeAttribute")
            .OfType<INamedTypeSymbol>()
            .FirstOrDefault();
        
        if (semantic.GetDeclaredSymbol(syntax) is not INamedTypeSymbol classSymbol)
        {
            return null;
        }

        if (classSymbol.GetMembers().SingleOrDefault(s => s.Name == "U") is not IMethodSymbol unionDecl)
        {
            return null;
        }

        if (!unionDecl.IsPartialDefinition)
        {
            return null;
        }
        
        var union = new DiscriminatedUnion()
        {
            Name = classSymbol.Name,
            Namespace = classSymbol.ContainingNamespace.ToString(),
            BeeSharpNamespace = resultType?.ContainingNamespace.ToString() ?? "Namespace detection failed. Is BeeSharpTypeAttribute existing in generator target project?",
        };
        foreach (var p in unionDecl.Parameters)
        {
            union.Members.Add(new UnionMember(p));
        }

        return union;
    }

    private static void GenerateUnion(SourceProductionContext spc, DiscriminatedUnion? discriminatedUnion,
        IncrementalValueProvider<string?> rootNamespace)

    {
        if (discriminatedUnion == null)
        {
            return;
        }

        using var rawWriter = new StringWriter();
        using var writer = new IndentedTextWriter(rawWriter);
        GenerateUnionBody(writer, discriminatedUnion);
        spc.AddSource($"{discriminatedUnion.Name}.g.cs", SourceText.From(rawWriter.ToString(), Encoding.UTF8));
    }

    private static void GenerateUnionBody(IndentedTextWriter writer, DiscriminatedUnion union)
    {
        writer.WriteLine(
            $$"""
            #nullable enable
            
            using System;
            
            namespace {{union.Namespace}};
            
            partial class {{union.Name}}
            {
            """);
        writer.Indent++;
        GenerateMembers(writer, union.Members);
        writer.WriteLine();
        GenerateConstructors(writer, union);
        writer.WriteLine();
        GenerateImplicitCasts(writer, union);
        writer.WriteLine();
        GenerateMethods(writer, union);
        writer.Indent--;
        writer.WriteLine();
        writer.WriteLine("}");
    }

    private static void GenerateConstructors(IndentedTextWriter writer, DiscriminatedUnion union)
    {
        foreach (var m in union.Members)
        {
            GenerateConstructor(writer, union, m);
            writer.WriteLine();
        }

        static void GenerateConstructor(IndentedTextWriter writer, DiscriminatedUnion union, UnionMember member)
        {
            writer.WriteLine($"public {union.Name}({member.Type} {member.Name.ToLower()})");
            writer.WriteLine("{");
            writer.Indent++;
            if (member.IsValueType is false)
            {
                writer.WriteLine($"ArgumentNullException.ThrowIfNull({member.Name.ToLower()});");
            }

            writer.WriteLine($"this.{member.Name} = {member.Name.ToLower()};");
            writer.Indent--;
            writer.WriteLine("}");
        }
    }

    private static void GenerateMembers(IndentedTextWriter writer, IReadOnlyList<UnionMember> members)
    {
        foreach (var m in members)
        {
            writer.WriteLine($"private {m.Type}? {m.Name} {{ get; }}");
        }
    }

    private static void GenerateMethods(IndentedTextWriter writer, DiscriminatedUnion union)
    {
        GenerateMap(writer, union.Members);
        writer.WriteLine();
        writer.WriteLine();
        GenerateUnwrapOrThrows(writer, union.Members);
        writer.WriteLine();
        writer.WriteLine();
        GenerateUnwrap(writer, union);
        writer.WriteLine();
        writer.WriteLine();
        GenerateToString(writer, union);
    }

    private static void GenerateUnwrap(IndentedTextWriter writer, DiscriminatedUnion union)
    {
        foreach (var m in union.Members)
        {
            GenerateUnwrapForMember(writer, union.BeeSharpNamespace, m);
        }
        
        static void GenerateUnwrapForMember(IndentedTextWriter writer, string beeTypesNamespace, UnionMember member)
        {
            writer.WriteLine($"public {beeTypesNamespace}.R<{member.Type}> Unwrap{member.Name}()");
            writer.WriteLine("{");
            writer.Indent++;
            string accessValue = member.IsValueType ? ".Value" : string.Empty;
            writer.WriteLine($"return this.{member.Name} is null");
            writer.WriteLine($"    ? {beeTypesNamespace}.Err.InvalidOp(\"Boom\")");
            writer.WriteLine($"    : this.{member.Name}{accessValue};");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
        }
    } 
    
    private static void GenerateUnwrapOrThrows(IndentedTextWriter writer, IReadOnlyList<UnionMember> members)
    {
        foreach (var m in members)
        {
            GenerateUnwrapOrThrow(writer, m);
            writer.WriteLine();
        }

        static void GenerateUnwrapOrThrow(IndentedTextWriter writer, UnionMember member)
        {
            writer.WriteLine($"public {member.Type} UnwrapOrThrow{member.Name}()");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine($"if (this.{member.Name} is null) {{ throw new InvalidOperationException(); }}");
            string accessValue = member.IsValueType ? ".Value" : string.Empty;
            writer.WriteLine($"return this.{member.Name}{accessValue};");
            writer.Indent--;
            writer.WriteLine("}");
        }
    }

    private static void GenerateMap(IndentedTextWriter writer, IReadOnlyList<UnionMember> members)
    {
        writer.WriteLine(
            """
            public T Map<T>(
            """);
        writer.Indent++;
        MapArgs(writer, members);
        writer.WriteLine(")");
        writer.Indent--;
        writer.WriteLine("{");
        writer.Indent++;
        MapInvokes(writer, members);
        writer.WriteLine("return default!; // never reached");
        writer.Indent--;
        writer.Write("}");

        static void MapArgs(IndentedTextWriter writer, IReadOnlyList<UnionMember> members)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < members.Count; i++)
            {
                var m = members[i];
                writer.Write($"Func<{m.Type}, T> map{m.Name}");
                if (i < members.Count - 1)
                {
                    writer.WriteLine(",");
                }
            }
        }

        static void MapInvokes(IndentedTextWriter writer, IReadOnlyList<UnionMember> members)
        {
            for (int i = 0; i < members.Count; i++)
            {
                var m = members[i];
                string accessValue = m.IsValueType ? ".Value" : string.Empty;
                writer.WriteLine(
                    $"if (this.{m.Name} is not null) {{ return map{m.Name}(this.{m.Name}{accessValue}); }}");
            }
        }
    }

    private static void GenerateToString(IndentedTextWriter writer, DiscriminatedUnion union)
    {
        writer.WriteLine("public override string ToString()");
        writer.WriteLine("{");
        writer.Indent++;

        foreach (var m in union.Members)
        {
            GenerateToStringForMember(writer, m);
        }

        writer.WriteLine("return default!; // never reached");
        writer.Indent--;
        writer.WriteLine("}");

        static void GenerateToStringForMember(IndentedTextWriter writer, UnionMember member)
        {
            writer.WriteLine($$$$"""
                           if(this.{{{{member.Name}}}} is not null) { return $"U{{{ this.{{{{member.Name}}}} }}}"; }
                           """);
        }
    }

    private static void GenerateImplicitCasts(IndentedTextWriter writer, DiscriminatedUnion union)
    {
        foreach (var m in union.Members)
        {
            WriteCastsForMember(writer, union, m);
        }

        static void WriteCastsForMember(IndentedTextWriter writer, DiscriminatedUnion union, UnionMember member)
        {
            writer.WriteLine($"""
                              public static implicit operator {union.Name}({member.Type} value) => new(value);
                              """);
        }
    }

    private static string MemberTypeArgList(DiscriminatedUnion union)
        => string.Join(", ", union.Members.Select(m => m.Type));
}