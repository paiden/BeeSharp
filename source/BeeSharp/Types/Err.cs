using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using BeeSharp.Utils;

namespace BeeSharp.Types
{
    public enum ErrorType
    {
        Unspecifed,
        InvalidArgument,
        Invalidoperation,
        IndexOutOfBounds,
    }

    public sealed class Err : IEquatable<Err>, IEqualityComparer<Err>
    {
        public static readonly IEqualityComparer<Err> Comparer = EqualityComparer<Err>.Default;

        public Err? Inner { get; }

        public ErrorType Type { get; }

        public string Message { get; }

        public string StackTrace { get; }

        private Err(ErrorType type, string message, string stackTrace, Err? inner = null)
        {
            this.Type = type;
            this.Message = message;
            this.StackTrace = stackTrace;
            this.Inner = inner;
        }

        public static Err InvalidOp(string message)
            => new Err(ErrorType.Invalidoperation, message, new StackTrace().ToString());

        public static Err Unspecified(string message)
            => new Err(ErrorType.Unspecifed, message, new StackTrace().ToString());

        public static Err FromException(Exception exc)
            => new Err(ErrorType.Unspecifed, exc.Message, exc.StackTrace);

        public static Err InvalidArgument(string message)
            => new Err(ErrorType.InvalidArgument, message, new StackTrace().ToString());

        public static implicit operator bool(Err err) => err != null;

        public R<T> AsResOf<T>() => R<T>.Err(this);

        public bool Equals([AllowNull]Err x, [AllowNull]Err y)
            => x == y || (x?.Equals(y) ?? false);

        public bool Equals(Err? other)
            => other != null
            && other.Message == this.Message
            && other.Type == this.Type;

        public override bool Equals(object? obj)
            => this.Equals(obj as Err);

        public override int GetHashCode()
            => Hash.Get(this.Type, this.Message);

        public int GetHashCode(Err obj)
            => obj.GetHashCode();

        public Err Wrap(Err inner)
            => new Err(this.Type, this.Message, new StackTrace().ToString(), inner);

        public override string ToString() => this.Message;
    }
}
