using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BeeSharp.Utils;

namespace BeeSharp.Types
{
    public static class R
    {
        public static R<T> Ok<T>(T res) => R<T>.Ok(res);
    }
    
    public readonly struct R<T> : IEquatable<R<T>>
    {
        private static readonly IEqualityComparer<T> ResEqComparer = EqualityComparer<T>.Default;

        private readonly T res;
        private readonly Err? err;

        public bool IsOk => err == null;

        public bool IsErr => err != null;

        public static R<T> Ok(T res) => new(res);

        public static R<T> Err(Err err) => new(err);
        
        public static R<T> Try(Func<T> init)
        {
            try
            {
                var value = init();
                return Ok(value);
            }
            catch (Exception ex)
            {
                return Err(Types.Err.FromException(ex));
            }
        }

        public static implicit operator R<T>(T res)
            => R<T>.Ok(res);
        
        public static implicit operator R<T>(Err err)
            => new(err);

        public R<T> Or(R<T> or)
            => this.IsOk
                ? this
                : or;

        public T Or(T or)
            => this.IsOk
                ? this.res
                : or;

        public R<T> OnErrorRun(Action<Err> action)
        {
            if (this.err is not null)
            {
                action(this.err);
            }

            return this;
        }

        public R<T> OrWrapError(Err newErr) => this.err != null
            ? Err(newErr.Wrap(this.err))
            : this;
        
        [OverloadResolutionPriority(1)]
        public R<TNew> Map<TNew>(Func<T, R<TNew>> op)
            => this.IsOk
                ? op(this.res)
                : new R<TNew>(this.err);
        
        public R<TNew> Map<TNew>(Func<T, TNew> op)
            => this.IsOk
                ? new R<TNew>(op(this.res))
                : new R<TNew>(this.err);

        public TNew MapOr<TNew>(Func<T, TNew> op, TNew defaultValue)
            => this.IsOk
                ? op(this.res)
                : defaultValue;

        public T UnwrapOrThrow()
            => this.IsOk
                ? this.res
                : throw new InvalidOperationException($"Cannot unwrap result on 'Error' with value '{this.err}'");

        public T UnwrapOr(T or)
            => this.IsOk
                ? this.res
                : or;

        public Err UnwrapErrOrThrow()
            => this.err ??
               throw new InvalidOperationException($"Cannot unwrap error on 'Ok' with value '{this.res}'.");

        private R(T value)
            : this(value, err: null)
        {
        }

        private R(Err? err)
            : this(default, err)
        {
        }

        private R(T? res, Err? err)
        {
            this.res = res;
            this.err = err;
        }

        public override string ToString()
            => this.IsOk
                ? $"Ok({this.res})"
                : $"Err({this.err!.Message})";

        public bool Equals(R<T> other)
        {
            return (this.IsErr, other.IsErr) switch
            {
                (true, true) => this.err!.Equals(other.err),
                (false, false) => this.res!.Equals(other.res),
                _ => false,
            };
        }

        public override bool Equals(object? obj)
        {
            if (!(obj == null || obj.GetType() != typeof(R<T>)))
            {
                return false;
            }

            return this.Equals((R<T>)obj);
        }

        public override int GetHashCode()
            => Hash.Get(this.err, this.res);
    }

    public static class RExtensions
    {
        public static R<TNew> CastBase<TOld, TNew>(this R<TOld> r) where TOld : TNew
            => r.IsOk ? R<TNew>.Ok(r.UnwrapOrThrow()) : R<TNew>.Err(r.UnwrapErrOrThrow());
    }
}