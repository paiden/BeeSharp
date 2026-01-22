using System;
using BeeSharp.Tests.Unions.Types;

namespace BeeSharp.Compile.Test.Union;

[DiscriminatedUnion]
internal partial class SimpleUnion
{
    partial void U(UnionClassType A, UnionStructType B, int C);
}