// The generator needs ahead of compilation time well known fully qualified name -> Just hi-jack System for this
// to not suddenly add BeeSharp related namespaces to the target project.
namespace System
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DiscriminatedUnionAttribute : Attribute
    {
    }
};

