
#nullable enable

using System;

namespace BeeSharp.Types
{
    public sealed class Union<T0, T1> : IEquatable<Union<T0, T1>>
    {
        private readonly Opt<T0> ofT0;
        private readonly Opt<T1> ofT1;

        public Union(T0 of) => this.ofT0 = Opt.Some(of);
        public Union(T1 of) => this.ofT1 = Opt.Some(of);

        public static implicit operator Union<T0, T1>(T0 of) => new Union<T0, T1>(of);
        public static implicit operator Union<T0, T1>(T1 of) => new Union<T0, T1>(of);

        public T Map<T>(Func<T0, T> m0, Func<T1, T> m1) =>
            this.ofT0.MapOrElse(m0,
            () => this.ofT1.Map(m1).Unwrap());

        public void Do(Action<T0> d0, Action<T1> d1) =>
            this.ofT0.DoOrElse(d0,
            () => this.ofT1.Do(d1));

        public bool Equals(Union<T0, T1>? other)
        {
            if(ReferenceEquals(other, null)) { return false; }
            if(ReferenceEquals(this, other)) { return true; }

            return this.ofT0.Equals(other.ofT0)
                && this.ofT1.Equals(other.ofT1);
        }

        public override bool Equals(object? other) => this.Equals(other as Union<T0, T1>);

        public override int GetHashCode()
            => HashCode.Combine(this.ofT0, this.ofT1);
    }

    public sealed class Union<T0, T1, T2> : IEquatable<Union<T0, T1, T2>>
    {
        private readonly Opt<T0> ofT0;
        private readonly Opt<T1> ofT1;
        private readonly Opt<T2> ofT2;

        public Union(T0 of) => this.ofT0 = Opt.Some(of);
        public Union(T1 of) => this.ofT1 = Opt.Some(of);
        public Union(T2 of) => this.ofT2 = Opt.Some(of);

        public static implicit operator Union<T0, T1, T2>(T0 of) => new Union<T0, T1, T2>(of);
        public static implicit operator Union<T0, T1, T2>(T1 of) => new Union<T0, T1, T2>(of);
        public static implicit operator Union<T0, T1, T2>(T2 of) => new Union<T0, T1, T2>(of);

        public T Map<T>(Func<T0, T> m0, Func<T1, T> m1, Func<T2, T> m2) =>
            this.ofT0.MapOrElse(m0,
            () => this.ofT1.MapOrElse(m1,
            () => this.ofT2.Map(m2).Unwrap()));

        public void Do(Action<T0> d0, Action<T1> d1, Action<T2> d2) =>
            this.ofT0.DoOrElse(d0,
            () => this.ofT1.DoOrElse(d1,
            () => this.ofT2.Do(d2)));

        public bool Equals(Union<T0, T1, T2>? other)
        {
            if(ReferenceEquals(other, null)) { return false; }
            if(ReferenceEquals(this, other)) { return true; }

            return this.ofT0.Equals(other.ofT0)
                && this.ofT1.Equals(other.ofT1)
                && this.ofT2.Equals(other.ofT2);
        }

        public override bool Equals(object? other) => this.Equals(other as Union<T0, T1, T2>);

        public override int GetHashCode()
            => HashCode.Combine(this.ofT0, this.ofT1, this.ofT2);
    }

    public sealed class Union<T0, T1, T2, T3> : IEquatable<Union<T0, T1, T2, T3>>
    {
        private readonly Opt<T0> ofT0;
        private readonly Opt<T1> ofT1;
        private readonly Opt<T2> ofT2;
        private readonly Opt<T3> ofT3;

        public Union(T0 of) => this.ofT0 = Opt.Some(of);
        public Union(T1 of) => this.ofT1 = Opt.Some(of);
        public Union(T2 of) => this.ofT2 = Opt.Some(of);
        public Union(T3 of) => this.ofT3 = Opt.Some(of);

        public static implicit operator Union<T0, T1, T2, T3>(T0 of) => new Union<T0, T1, T2, T3>(of);
        public static implicit operator Union<T0, T1, T2, T3>(T1 of) => new Union<T0, T1, T2, T3>(of);
        public static implicit operator Union<T0, T1, T2, T3>(T2 of) => new Union<T0, T1, T2, T3>(of);
        public static implicit operator Union<T0, T1, T2, T3>(T3 of) => new Union<T0, T1, T2, T3>(of);

        public T Map<T>(Func<T0, T> m0, Func<T1, T> m1, Func<T2, T> m2, Func<T3, T> m3) =>
            this.ofT0.MapOrElse(m0,
            () => this.ofT1.MapOrElse(m1,
            () => this.ofT2.MapOrElse(m2,
            () => this.ofT3.Map(m3).Unwrap())));

        public void Do(Action<T0> d0, Action<T1> d1, Action<T2> d2, Action<T3> d3) =>
            this.ofT0.DoOrElse(d0,
            () => this.ofT1.DoOrElse(d1,
            () => this.ofT2.DoOrElse(d2,
            () => this.ofT3.Do(d3))));

        public bool Equals(Union<T0, T1, T2, T3>? other)
        {
            if(ReferenceEquals(other, null)) { return false; }
            if(ReferenceEquals(this, other)) { return true; }

            return this.ofT0.Equals(other.ofT0)
                && this.ofT1.Equals(other.ofT1)
                && this.ofT2.Equals(other.ofT2)
                && this.ofT3.Equals(other.ofT3);
        }

        public override bool Equals(object? other) => this.Equals(other as Union<T0, T1, T2, T3>);

        public override int GetHashCode()
            => HashCode.Combine(this.ofT0, this.ofT1, this.ofT2, this.ofT3);
    }

    public sealed class Union<T0, T1, T2, T3, T4> : IEquatable<Union<T0, T1, T2, T3, T4>>
    {
        private readonly Opt<T0> ofT0;
        private readonly Opt<T1> ofT1;
        private readonly Opt<T2> ofT2;
        private readonly Opt<T3> ofT3;
        private readonly Opt<T4> ofT4;

        public Union(T0 of) => this.ofT0 = Opt.Some(of);
        public Union(T1 of) => this.ofT1 = Opt.Some(of);
        public Union(T2 of) => this.ofT2 = Opt.Some(of);
        public Union(T3 of) => this.ofT3 = Opt.Some(of);
        public Union(T4 of) => this.ofT4 = Opt.Some(of);

        public static implicit operator Union<T0, T1, T2, T3, T4>(T0 of) => new Union<T0, T1, T2, T3, T4>(of);
        public static implicit operator Union<T0, T1, T2, T3, T4>(T1 of) => new Union<T0, T1, T2, T3, T4>(of);
        public static implicit operator Union<T0, T1, T2, T3, T4>(T2 of) => new Union<T0, T1, T2, T3, T4>(of);
        public static implicit operator Union<T0, T1, T2, T3, T4>(T3 of) => new Union<T0, T1, T2, T3, T4>(of);
        public static implicit operator Union<T0, T1, T2, T3, T4>(T4 of) => new Union<T0, T1, T2, T3, T4>(of);

        public T Map<T>(Func<T0, T> m0, Func<T1, T> m1, Func<T2, T> m2, Func<T3, T> m3, Func<T4, T> m4) =>
            this.ofT0.MapOrElse(m0,
            () => this.ofT1.MapOrElse(m1,
            () => this.ofT2.MapOrElse(m2,
            () => this.ofT3.MapOrElse(m3,
            () => this.ofT4.Map(m4).Unwrap()))));

        public void Do(Action<T0> d0, Action<T1> d1, Action<T2> d2, Action<T3> d3, Action<T4> d4) =>
            this.ofT0.DoOrElse(d0,
            () => this.ofT1.DoOrElse(d1,
            () => this.ofT2.DoOrElse(d2,
            () => this.ofT3.DoOrElse(d3,
            () => this.ofT4.Do(d4)))));

        public bool Equals(Union<T0, T1, T2, T3, T4>? other)
        {
            if(ReferenceEquals(other, null)) { return false; }
            if(ReferenceEquals(this, other)) { return true; }

            return this.ofT0.Equals(other.ofT0)
                && this.ofT1.Equals(other.ofT1)
                && this.ofT2.Equals(other.ofT2)
                && this.ofT3.Equals(other.ofT3)
                && this.ofT4.Equals(other.ofT4);
        }

        public override bool Equals(object? other) => this.Equals(other as Union<T0, T1, T2, T3, T4>);

        public override int GetHashCode()
            => HashCode.Combine(this.ofT0, this.ofT1, this.ofT2, this.ofT3, this.ofT4);
    }

    public sealed class Union<T0, T1, T2, T3, T4, T5> : IEquatable<Union<T0, T1, T2, T3, T4, T5>>
    {
        private readonly Opt<T0> ofT0;
        private readonly Opt<T1> ofT1;
        private readonly Opt<T2> ofT2;
        private readonly Opt<T3> ofT3;
        private readonly Opt<T4> ofT4;
        private readonly Opt<T5> ofT5;

        public Union(T0 of) => this.ofT0 = Opt.Some(of);
        public Union(T1 of) => this.ofT1 = Opt.Some(of);
        public Union(T2 of) => this.ofT2 = Opt.Some(of);
        public Union(T3 of) => this.ofT3 = Opt.Some(of);
        public Union(T4 of) => this.ofT4 = Opt.Some(of);
        public Union(T5 of) => this.ofT5 = Opt.Some(of);

        public static implicit operator Union<T0, T1, T2, T3, T4, T5>(T0 of) => new Union<T0, T1, T2, T3, T4, T5>(of);
        public static implicit operator Union<T0, T1, T2, T3, T4, T5>(T1 of) => new Union<T0, T1, T2, T3, T4, T5>(of);
        public static implicit operator Union<T0, T1, T2, T3, T4, T5>(T2 of) => new Union<T0, T1, T2, T3, T4, T5>(of);
        public static implicit operator Union<T0, T1, T2, T3, T4, T5>(T3 of) => new Union<T0, T1, T2, T3, T4, T5>(of);
        public static implicit operator Union<T0, T1, T2, T3, T4, T5>(T4 of) => new Union<T0, T1, T2, T3, T4, T5>(of);
        public static implicit operator Union<T0, T1, T2, T3, T4, T5>(T5 of) => new Union<T0, T1, T2, T3, T4, T5>(of);

        public T Map<T>(Func<T0, T> m0, Func<T1, T> m1, Func<T2, T> m2, Func<T3, T> m3, Func<T4, T> m4, Func<T5, T> m5) =>
            this.ofT0.MapOrElse(m0,
            () => this.ofT1.MapOrElse(m1,
            () => this.ofT2.MapOrElse(m2,
            () => this.ofT3.MapOrElse(m3,
            () => this.ofT4.MapOrElse(m4,
            () => this.ofT5.Map(m5).Unwrap())))));

        public void Do(Action<T0> d0, Action<T1> d1, Action<T2> d2, Action<T3> d3, Action<T4> d4, Action<T5> d5) =>
            this.ofT0.DoOrElse(d0,
            () => this.ofT1.DoOrElse(d1,
            () => this.ofT2.DoOrElse(d2,
            () => this.ofT3.DoOrElse(d3,
            () => this.ofT4.DoOrElse(d4,
            () => this.ofT5.Do(d5))))));

        public bool Equals(Union<T0, T1, T2, T3, T4, T5>? other)
        {
            if(ReferenceEquals(other, null)) { return false; }
            if(ReferenceEquals(this, other)) { return true; }

            return this.ofT0.Equals(other.ofT0)
                && this.ofT1.Equals(other.ofT1)
                && this.ofT2.Equals(other.ofT2)
                && this.ofT3.Equals(other.ofT3)
                && this.ofT4.Equals(other.ofT4)
                && this.ofT5.Equals(other.ofT5);
        }

        public override bool Equals(object? other) => this.Equals(other as Union<T0, T1, T2, T3, T4, T5>);

        public override int GetHashCode()
            => HashCode.Combine(this.ofT0, this.ofT1, this.ofT2, this.ofT3, this.ofT4, this.ofT5);
    }

    public sealed class Union<T0, T1, T2, T3, T4, T5, T6> : IEquatable<Union<T0, T1, T2, T3, T4, T5, T6>>
    {
        private readonly Opt<T0> ofT0;
        private readonly Opt<T1> ofT1;
        private readonly Opt<T2> ofT2;
        private readonly Opt<T3> ofT3;
        private readonly Opt<T4> ofT4;
        private readonly Opt<T5> ofT5;
        private readonly Opt<T6> ofT6;

        public Union(T0 of) => this.ofT0 = Opt.Some(of);
        public Union(T1 of) => this.ofT1 = Opt.Some(of);
        public Union(T2 of) => this.ofT2 = Opt.Some(of);
        public Union(T3 of) => this.ofT3 = Opt.Some(of);
        public Union(T4 of) => this.ofT4 = Opt.Some(of);
        public Union(T5 of) => this.ofT5 = Opt.Some(of);
        public Union(T6 of) => this.ofT6 = Opt.Some(of);

        public static implicit operator Union<T0, T1, T2, T3, T4, T5, T6>(T0 of) => new Union<T0, T1, T2, T3, T4, T5, T6>(of);
        public static implicit operator Union<T0, T1, T2, T3, T4, T5, T6>(T1 of) => new Union<T0, T1, T2, T3, T4, T5, T6>(of);
        public static implicit operator Union<T0, T1, T2, T3, T4, T5, T6>(T2 of) => new Union<T0, T1, T2, T3, T4, T5, T6>(of);
        public static implicit operator Union<T0, T1, T2, T3, T4, T5, T6>(T3 of) => new Union<T0, T1, T2, T3, T4, T5, T6>(of);
        public static implicit operator Union<T0, T1, T2, T3, T4, T5, T6>(T4 of) => new Union<T0, T1, T2, T3, T4, T5, T6>(of);
        public static implicit operator Union<T0, T1, T2, T3, T4, T5, T6>(T5 of) => new Union<T0, T1, T2, T3, T4, T5, T6>(of);
        public static implicit operator Union<T0, T1, T2, T3, T4, T5, T6>(T6 of) => new Union<T0, T1, T2, T3, T4, T5, T6>(of);

        public T Map<T>(Func<T0, T> m0, Func<T1, T> m1, Func<T2, T> m2, Func<T3, T> m3, Func<T4, T> m4, Func<T5, T> m5, Func<T6, T> m6) =>
            this.ofT0.MapOrElse(m0,
            () => this.ofT1.MapOrElse(m1,
            () => this.ofT2.MapOrElse(m2,
            () => this.ofT3.MapOrElse(m3,
            () => this.ofT4.MapOrElse(m4,
            () => this.ofT5.MapOrElse(m5,
            () => this.ofT6.Map(m6).Unwrap()))))));

        public void Do(Action<T0> d0, Action<T1> d1, Action<T2> d2, Action<T3> d3, Action<T4> d4, Action<T5> d5, Action<T6> d6) =>
            this.ofT0.DoOrElse(d0,
            () => this.ofT1.DoOrElse(d1,
            () => this.ofT2.DoOrElse(d2,
            () => this.ofT3.DoOrElse(d3,
            () => this.ofT4.DoOrElse(d4,
            () => this.ofT5.DoOrElse(d5,
            () => this.ofT6.Do(d6)))))));

        public bool Equals(Union<T0, T1, T2, T3, T4, T5, T6>? other)
        {
            if(ReferenceEquals(other, null)) { return false; }
            if(ReferenceEquals(this, other)) { return true; }

            return this.ofT0.Equals(other.ofT0)
                && this.ofT1.Equals(other.ofT1)
                && this.ofT2.Equals(other.ofT2)
                && this.ofT3.Equals(other.ofT3)
                && this.ofT4.Equals(other.ofT4)
                && this.ofT5.Equals(other.ofT5)
                && this.ofT6.Equals(other.ofT6);
        }

        public override bool Equals(object? other) => this.Equals(other as Union<T0, T1, T2, T3, T4, T5, T6>);

        public override int GetHashCode()
            => HashCode.Combine(this.ofT0, this.ofT1, this.ofT2, this.ofT3, this.ofT4, this.ofT5, this.ofT6);
    }

    public sealed class Union<T0, T1, T2, T3, T4, T5, T6, T7> : IEquatable<Union<T0, T1, T2, T3, T4, T5, T6, T7>>
    {
        private readonly Opt<T0> ofT0;
        private readonly Opt<T1> ofT1;
        private readonly Opt<T2> ofT2;
        private readonly Opt<T3> ofT3;
        private readonly Opt<T4> ofT4;
        private readonly Opt<T5> ofT5;
        private readonly Opt<T6> ofT6;
        private readonly Opt<T7> ofT7;

        public Union(T0 of) => this.ofT0 = Opt.Some(of);
        public Union(T1 of) => this.ofT1 = Opt.Some(of);
        public Union(T2 of) => this.ofT2 = Opt.Some(of);
        public Union(T3 of) => this.ofT3 = Opt.Some(of);
        public Union(T4 of) => this.ofT4 = Opt.Some(of);
        public Union(T5 of) => this.ofT5 = Opt.Some(of);
        public Union(T6 of) => this.ofT6 = Opt.Some(of);
        public Union(T7 of) => this.ofT7 = Opt.Some(of);

        public static implicit operator Union<T0, T1, T2, T3, T4, T5, T6, T7>(T0 of) => new Union<T0, T1, T2, T3, T4, T5, T6, T7>(of);
        public static implicit operator Union<T0, T1, T2, T3, T4, T5, T6, T7>(T1 of) => new Union<T0, T1, T2, T3, T4, T5, T6, T7>(of);
        public static implicit operator Union<T0, T1, T2, T3, T4, T5, T6, T7>(T2 of) => new Union<T0, T1, T2, T3, T4, T5, T6, T7>(of);
        public static implicit operator Union<T0, T1, T2, T3, T4, T5, T6, T7>(T3 of) => new Union<T0, T1, T2, T3, T4, T5, T6, T7>(of);
        public static implicit operator Union<T0, T1, T2, T3, T4, T5, T6, T7>(T4 of) => new Union<T0, T1, T2, T3, T4, T5, T6, T7>(of);
        public static implicit operator Union<T0, T1, T2, T3, T4, T5, T6, T7>(T5 of) => new Union<T0, T1, T2, T3, T4, T5, T6, T7>(of);
        public static implicit operator Union<T0, T1, T2, T3, T4, T5, T6, T7>(T6 of) => new Union<T0, T1, T2, T3, T4, T5, T6, T7>(of);
        public static implicit operator Union<T0, T1, T2, T3, T4, T5, T6, T7>(T7 of) => new Union<T0, T1, T2, T3, T4, T5, T6, T7>(of);

        public T Map<T>(Func<T0, T> m0, Func<T1, T> m1, Func<T2, T> m2, Func<T3, T> m3, Func<T4, T> m4, Func<T5, T> m5, Func<T6, T> m6, Func<T7, T> m7) =>
            this.ofT0.MapOrElse(m0,
            () => this.ofT1.MapOrElse(m1,
            () => this.ofT2.MapOrElse(m2,
            () => this.ofT3.MapOrElse(m3,
            () => this.ofT4.MapOrElse(m4,
            () => this.ofT5.MapOrElse(m5,
            () => this.ofT6.MapOrElse(m6,
            () => this.ofT7.Map(m7).Unwrap())))))));

        public void Do(Action<T0> d0, Action<T1> d1, Action<T2> d2, Action<T3> d3, Action<T4> d4, Action<T5> d5, Action<T6> d6, Action<T7> d7) =>
            this.ofT0.DoOrElse(d0,
            () => this.ofT1.DoOrElse(d1,
            () => this.ofT2.DoOrElse(d2,
            () => this.ofT3.DoOrElse(d3,
            () => this.ofT4.DoOrElse(d4,
            () => this.ofT5.DoOrElse(d5,
            () => this.ofT6.DoOrElse(d6,
            () => this.ofT7.Do(d7))))))));

        public bool Equals(Union<T0, T1, T2, T3, T4, T5, T6, T7>? other)
        {
            if(ReferenceEquals(other, null)) { return false; }
            if(ReferenceEquals(this, other)) { return true; }

            return this.ofT0.Equals(other.ofT0)
                && this.ofT1.Equals(other.ofT1)
                && this.ofT2.Equals(other.ofT2)
                && this.ofT3.Equals(other.ofT3)
                && this.ofT4.Equals(other.ofT4)
                && this.ofT5.Equals(other.ofT5)
                && this.ofT6.Equals(other.ofT6)
                && this.ofT7.Equals(other.ofT7);
        }

        public override bool Equals(object? other) => this.Equals(other as Union<T0, T1, T2, T3, T4, T5, T6, T7>);

        public override int GetHashCode()
            => HashCode.Combine(this.ofT0, this.ofT1, this.ofT2, this.ofT3, this.ofT4, this.ofT5, this.ofT6, this.ofT7);
    }

}









