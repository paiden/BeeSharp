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

    public sealed class Error : IEquatable<Error>, IEqualityComparer<Error>
    {
        public static readonly IEqualityComparer<Error> Comparer = EqualityComparer<Error>.Default;

        public Error? Inner { get; }

        public ErrorType Type { get; }

        public string Message { get; }

        public string StackTrace { get; }

        private Error(ErrorType type, string message, string stackTrace, Error? inner = null)
        {
            this.Type = type;
            this.Message = message;
            this.StackTrace = stackTrace;
            this.Inner = inner;
        }

        public static Error InvalidOp(string message)
            => new Error(ErrorType.Invalidoperation, message, new StackTrace().ToString());

        public static Error Unspecified(string message)
            => new Error(ErrorType.Unspecifed, message, new StackTrace().ToString());

        public static Error FromException(Exception exc)
            => new Error(ErrorType.Unspecifed, exc.Message, exc.StackTrace);

        public static implicit operator bool(Error err) => err != null;

        public Res<T> AsResOf<T>() => Res<T>.Err(this);

        public bool Equals([AllowNull]Error x, [AllowNull]Error y)
            => x == y || (x?.Equals(y) ?? false);

        public bool Equals(Error? other)
            => other != null
            && other.Message == this.Message
            && other.Type == this.Type;

        public override bool Equals(object? obj)
            => this.Equals(obj as Error);

        public override int GetHashCode()
            => Hash.Get(this.Type, this.Message);

        public int GetHashCode(Error obj)
            => obj.GetHashCode();

        public Error Wrap(Error inner)
            => new Error(this.Type, this.Message, new StackTrace().ToString(), inner);

        public override string ToString() => this.Message;
    }
}
