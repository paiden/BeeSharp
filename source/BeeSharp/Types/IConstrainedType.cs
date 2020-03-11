using System;

namespace BeeSharp.Types
{
    public interface IConstrainedType<T> : IEquatable<T>, IComparable<T>, IPreventDefaultConstruction
    {
    }
}
