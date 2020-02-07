using System;

namespace BeeSharp
{
    /// <summary>
    /// Members that return a type that implements this interface must be embedded in
    /// the using statement of a using <b>block</b> (using declaration not allowed).
    /// This should be used where the Dispose part of the type is used to run
    /// important logic.
    /// </summary>
    public interface IDisposeLocallyStrict : IDisposable
    {
    }

    /// <summary>
    /// Members that return a type that implements this interface must be embedded in
    /// a using statement.
    /// </summary>
    public interface IDisposeLocally : IDisposable
    {
    }

    /// <summary>
    /// Types implementing this interface are not allowed to be default constructed.
    /// </summary>
    /// <remarks>
    /// Should only be used on <b>structs</b>. For classes make the constructor private instead.
    /// </remarks>
    public interface IPreventDefaultConstruction
    {
    }
}
