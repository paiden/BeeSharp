using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace BeeSharp.Threading
{
    /// <summary>
    /// Allow to declare field as a locked. Such a variable can only be read / written by
    /// acquiring it via special access methods that will create read / write locks and return a
    /// access scope that has to be disposed to release the lock.
    /// </summary>
    /// <remarks>
    /// Not using the scope in conjunction with a using statement will have a hight risk of causing
    /// deadlocks.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public sealed class Locked<T>
    {
        private readonly ReaderWriterLockSlim padlock = new ReaderWriterLockSlim();

#pragma warning disable S2933 // Fields that are only assigned in the constructor should be "readonly"
        // (false positive, inner class accesses the field in write manner)
        [AllowNull]
        private T value;
#pragma warning restore S2933 // Fields that are only assigned in the constructor should be "readonly"

        public Locked(T value)
        {
            this.value = value;
        }


        public IWriteAccessScope WriteAccess() => new WriteAccessScope(this);

        public IReadAccessScope ReadAccess() => new ReadAccessScope(this);

        public interface IWriteAccessScope : IDisposable, IDisposeLocallyStrict
        {
            T Value { get; set; }
        }

        public interface IReadAccessScope : IDisposable, IDisposeLocallyStrict
        {
            T Value { get; }
        }

        private sealed class WriteAccessScope : IWriteAccessScope
        {
            private readonly Locked<T> target;

            public WriteAccessScope(Locked<T> target)
            {
                this.target = target;
                this.target.padlock.EnterWriteLock();
            }

            [MaybeNull]
            [AllowNull]
            public T Value
            {
                get => this.target.value;
                set => this.target.value = value;
            }

            public void Dispose()
            {
                this.target.padlock.ExitWriteLock();
            }
        }

        private sealed class ReadAccessScope : IReadAccessScope
        {
            private readonly Locked<T> target;

            public ReadAccessScope(Locked<T> target)
            {
                this.target = target;
                Monitor.Enter(target.padlock);
            }

            [MaybeNull]
            public T Value => this.target.value;

            public void Dispose()
                => Monitor.Exit(target.padlock);
        }
    }
}
