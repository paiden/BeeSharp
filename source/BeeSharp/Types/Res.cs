using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using BeeSharp.Utils;

namespace BeeSharp.Types
{
    public static class Res
    {
        public static Res<Unit> Err(Error err) => Res<Unit>.Err(err);

        public static Res<Unit> Ok() => Res<Unit>.Ok(Unit.U);

        public static Res<T> Ok<T>(T res) => Res<T>.Ok(res);

        public static Res<T> Try<T>(Func<T> f)
        {
            try
            {
                var r = f();
                return Res<T>.Ok(r);
            }
            catch (Exception exc)
            {
                return Res<T>.Err(Error.FromException(exc));
            }
        }
    }

    public sealed class Res<T> : IEquatable<Res<T>>
    {
        private static readonly IEqualityComparer<T> resEqComparer = EqualityComparer<T>.Default;

        private readonly Opt<T> res;
        private readonly Error? err;

        public bool IsOk => this.res.IsSome;

        public bool IsErr => err != null;

        public static Res<T> Ok(T res) => new Res<T>(res);

        public static Res<T> Err(Error err) => new Res<T>(err);

        public static implicit operator Res<T>(Error err)
            => new Res<T>(err);

        public Res<TNew> And<TNew>(Res<TNew> n) => this.res
            ? n
            : new Res<TNew>(this.err);

        public Res<TNew> And<TNew>(TNew n) => this.res
            ? Res.Ok(n)
            : new Res<TNew>(this.err);

        public Res<TNew> And<TNew>(Func<T, Res<TNew>> op) => this.res
            ? op(this.res.Unwrap())
            : new Res<TNew>(this.err);

        public Res<TNew> And<TNew>(Func<T, TNew> op) => this.res
            ? Res.Ok(op(this.res.Unwrap()))
            : new Res<TNew>(this.err);

        public Res<TNew> Cast<TNew>(Func<T, TNew> cast)
        {
            var val = this.res.IsSome ? Opt.Of(cast(this.res.Unwrap())) : Opt.None<TNew>();
            return new Res<TNew>(val, this.err);
        }

        public bool Contains(T v) => this.res && resEqComparer.Equals(this.res.Unwrap(), v);

        public bool ContainsErr(Error err) => this.err != null && Error.Comparer.Equals(this.err, err);

        public Res<T> Or(Res<T> or) => this.err != null
            ? or
            : this;

        public Res<T> Or(T or) => this.err != null
            ? Res.Ok(or)
            : this;

        public Res<T> Or(Func<Error, Res<T>> op) => this.err != null
            ? op(this.err)
            : new Res<T>(this.res.Unwrap());

        public Res<T> OrWrapError(Error newError) => this.err != null
            ? Res<T>.Err(newError.Wrap(this.err))
            : this;

        public Res<TNew> Map<TNew>(Func<T, Res<TNew>> op) => this.res
            ? op(this.res.Unwrap())
            : new Res<TNew>(this.err);

        public Res<TNew> Map<TNew>(Func<T, TNew> op) => this.res
            ? new Res<TNew>(op(this.res.Unwrap()))
            : new Res<TNew>(this.err);

        public Res<TNew> MapOr<TNew>(Func<T, Res<TNew>> op, Func<Error, Res<TNew>> orElse) => this.res
            ? op(this.res.Unwrap())
            : orElse(this.err!);

        public T Unwrap() => this.res
            ? this.res.Unwrap()
            : throw new InvalidOperationException($"Cannot unwrap result on 'Error' with value '{this.err}'");
        
        public T UnwrapOr(T or) => this.res
            ? this.res.Unwrap()
            : or;

        public Error UnwrapErr()
            => this.err ?? throw new InvalidOperationException($"Cannot unwrap error on 'Ok' with value '{this.res.Unwrap()}'.");

        public static Res<T> InvalidOperation(string message)
            => new Res<T>(Error.InvalidOp(message));

        private Res(T value)
            : this(Opt.Some(value), err: null)
        {
        }

        private Res(Error? err)
            : this(Opt.None<T>(), err)
        {
        }

        private Res(Opt<T> res, Error? err)
        {
            this.res = res;
            this.err = err;
        }

        public override string ToString() => this.res
            ? $"Ok({this.res})"
            : $"Err({this.err!.Message})";

        public void Deconstruct(out T res, out Error? err)
        {
            res = this.res.IsSome ? this.res.Unwrap() : default!;
            err = this.err ?? default;
        }

        public bool Equals([AllowNull] Res<T> other)
        {
            if (other == null) { return false; }

            return (this.IsErr, other.IsErr) switch
            {
                (true, true) => this.err!.Equals(other.err),
                (false, false) => this.res.Equals(other.res),
                _ => false,
            };
        }

        public override bool Equals(object? obj)
            => this.Equals(obj as Res<T>);

        public override int GetHashCode()
            => Hash.Get(this.err, this.res);
    }
}
