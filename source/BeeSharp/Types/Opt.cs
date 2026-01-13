using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using BeeSharp.Utils;

namespace BeeSharp.Types
{
    public static class Opt
    {
        public static Opt<T> Some<T>(T value) => new Opt<T>(value);

        public static Opt<T> None<T>() => Opt<T>.None;

        public static Opt<T> Of<T>(T value) => value == null ? None<T>() : Some(value);
    }

    public struct Opt<T>
        : IEquatable<Opt<T>>, IComparable<Opt<T>>
    {
        public static readonly Opt<T> None = new Opt<T>(default, false);

        private static readonly IEqualityComparer<T> someEqComparer = EqualityComparer<T>.Default;
        private static readonly Comparer<T> someComp = Comparer<T>.Default;

        [AllowNull]
        private readonly T some;

        public Opt([AllowNull] T value)
            : this(value, value != null)
        {
        }

        public static implicit operator bool(Opt<T> o) => o.IsSome;

        public bool IsSome { get; }

        public bool IsNone => !this.IsSome;

        public Opt<TNew> And<TNew>(Opt<TNew> and) => this.IsNone
            ? Opt<TNew>.None
            : and;

        public Opt<TNew> And<TNew>(TNew and) => this.IsNone
            ? Opt<TNew>.None
            : Opt.Some(and);

        public Opt<TNew> AndThen<TNew>(Func<T, Opt<TNew>> then) => this.IsNone
            ? Opt<TNew>.None
            : then(this.some);

        public Opt<TNew> AndThen<TNew>(Func<T, TNew> then) => this.IsNone
            ? Opt<TNew>.None
            : Opt.Some(then(this.some));

        public void Do(Action<T> a)
        {
            if (this.IsSome)
            {
                a(this.some);
            }
            else
            {
                throw new InvalidOperationException("Cannot run operation on 'None'.");
            }
        }

        public void DoOrElse(Action<T> a, Action or)
        {
            if (this.IsSome) { a(this.some); } else { or(); }
        }

        public bool Contains(T c) => this.IsSome && someEqComparer.Equals(this.some, c);

        public T Expect(string message) => this.IsSome
            ? this.some
            : throw new InvalidOperationException(message);

        public void ExpectNone(string message)
        {
            if (!this.IsNone)
            {
                throw new InvalidOperationException(message);
            }
        }

        public Opt<T> Filter(Func<T, bool> predicate) => this.IsSome && predicate(this.some)
            ? this
            : None;

        public Opt<TNew> Map<TNew>(Func<T, Opt<TNew>> map) => this.IsSome
            ? map(this.some)
            : Opt<TNew>.None;

        public Opt<TNew> Map<TNew>(Func<T, TNew> map) => this.IsSome
            ? Opt.Some(map(this.some))
            : Opt<TNew>.None;

        public Opt<TNew> MapOrElse<TNew>(Func<T, TNew> map, Func<Opt<TNew>> orElse) => this.IsSome
            ? Opt.Some(map(this.some))
            : orElse();

        public U MapOr<U>(Func<T, U> map, U def) => this.IsSome
            ? map(this.some)
            : def;

        public U MapOrElse<U>(Func<T, U> map, Func<U> def) => this.IsSome
            ? map(this.some)
            : def();

        public Res<T> OkOr(Error err) => this.IsSome
            ? Res.Ok(this.some)
            : Res<T>.Err(err);

        public Res<T> OkOrElse(Func<Error> err) => this.IsSome
            ? Res.Ok(this.some)
            : Res<T>.Err(err());

        public Opt<T> Or(Opt<T> or) => this.IsNone
            ? or
            : this;

        public Opt<T> Or(T or) => this.IsNone
            ? Opt.Some(or)
            : this;

        public Opt<T> OrElse(Func<Opt<T>> or) => this.IsNone
            ? or()
            : this;

        public Opt<T> OrElse(Func<T> or) => this.IsNone
            ? Opt.Some(or())
            : this;

        public T Unwrap() => this.IsSome
            ? this.some
            : throw new InvalidOperationException("Failed to unwrap 'Some' as option is a 'None'.");

        public void UnwrapNone()
        {
            if (this.IsSome)
            {
                throw new InvalidOperationException($"Failed to unwrap 'None' as option is 'Some({this.some})'.");
            }
        }

        public T UnwrapOr(T or) => this.IsSome
            ? this.some
            : or;

        public T UnwrapOrElse(Func<T> or) => this.IsSome
            ? this.some
            : or();

        public Opt<T> Xor(Opt<T> other)
        {
            return (this.IsSome, other.IsSome) switch
            {
                (false, true) => other,
                (true, false) => this,
                _ => Opt<T>.None,
            };
        }
        private Opt([AllowNull] T value, bool hasValue)
        {
            this.some = value;
            this.IsSome = hasValue;
        }

        public int CompareTo(Opt<T> other)
            => (this.IsSome, other.IsSome) switch
            {
                (false, false) => BeeSharpConstants.Compare.Equal,
                (false, true) => BeeSharpConstants.Compare.ThisFollows,
                (true, false) => BeeSharpConstants.Compare.ThisPreceedes,
                _ => someComp.Compare(this.some, other.some),
            };

        public static bool operator ==(Opt<T> x, Opt<T> y) => x.Equals(y);

        public static bool operator !=(Opt<T> x, Opt<T> y) => !x.Equals(y);

        public static bool operator <(Opt<T> x, Opt<T> y) => x.CompareTo(y) < 0;

        public static bool operator >(Opt<T> x, Opt<T> y) => x.CompareTo(y) > 0;

        public static bool operator <=(Opt<T> x, Opt<T> y) => x < y || x == y;

        public static bool operator >=(Opt<T> x, Opt<T> y) => x > y || x == y;

        public bool Equals(Opt<T> other)
            => this.IsSome == other.IsSome
            && Equals(this.some, other.some);


        public override bool Equals(object? obj)
            => obj is Opt<T> opt && this.Equals(opt);

        public override int GetHashCode()
            => Hash.Get(this.some, this.IsSome);

        public override string ToString()
            => this.IsNone ? "None" : $"Some({this.some})";
    }

    public static class OptExtensions
    {
        public static Opt<T> Flatten<T>(this Opt<Opt<T>> o) => o.IsNone
            ? Opt<T>.None
            : o.Unwrap();

        public static Res<Opt<T>> Transpose<T>(this Opt<Res<T>> o)
        {
            if (o.IsNone) { return Res.Ok(Opt<T>.None); }

            var res = o.Unwrap();
            if (res.IsOk) { return Res.Ok(Opt.Some(res.Unwrap())); }
            else { return Res<Opt<T>>.Err(res.UnwrapErr()); }
        }

        public static Opt<T> ToOpt<T>(this T obj) => new Opt<T>(obj);
    }
}
