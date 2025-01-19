using System;
using System.Collections.Generic;
using System.Text;

namespace BeeSharp.Utils
{
    public sealed class LambdaEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> equals;
        private readonly Func<T, int> getHashCode;

        public LambdaEqualityComparer(Func<T, T, bool> equals)
            : this(equals, x => x.GetHashCode())
        {
        }

        public LambdaEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode)
        {
            this.equals = equals;
            this.getHashCode = getHashCode;
        }

        public bool Equals(T x, T y)
            => this.equals(x, y);

        public int GetHashCode(T obj)
            => this.getHashCode(obj);
    }
}
