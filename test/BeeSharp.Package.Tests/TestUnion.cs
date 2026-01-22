namespace BeeSharp.Package.Test;

[DiscriminatedUnion]
internal partial class TestUnion
{
    partial void Union(int X, double Y);
}