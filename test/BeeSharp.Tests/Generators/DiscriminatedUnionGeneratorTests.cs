using BeeSharp.SourceGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace BeeSharp.Tests.Generators;

public class DiscriminatedUnionGeneratorTests
{
    private const string MarkerSource = """
                                        namespace System
                                        {
                                            [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
                                            public class DiscriminatedUnionAttribute : Attribute
                                            {
                                            }
                                        }
                                        """;
    
    private const string TypesSource = """
                                       namespace MyTypes { class MyClass {} struct MyStruct {} }
                                       """;
    
    private const string Source = """
                                  using System;
                                  using MyTypes;

                                  namespace Test.TestNs.Ns 
                                  {
                                  [DiscriminatedUnion]
                                  internal partial class Union 
                                  {
                                  partial void U(MyClass A, MyStruct B);
                                  }
                                  }
                                  """;
    
    [Fact]
    void Foo()
    {
        var generator = new DiscriminatedUnionGenerator();
        
        var compilation = CSharpCompilation.Create("CSharpCodeGen.GenerateAssembly")
            .AddSyntaxTrees(CSharpSyntaxTree.ParseText(MarkerSource))
            .AddSyntaxTrees(CSharpSyntaxTree.ParseText(TypesSource))
            .AddSyntaxTrees(CSharpSyntaxTree.ParseText(Source))
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var driver = CSharpGeneratorDriver.Create(generator)
            .RunGeneratorsAndUpdateCompilation(compilation, out _, out var _);
    }
}