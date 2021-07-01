using System.Diagnostics;

namespace BeeSharp.Types
{
    public record Phantom<T, TPhantomTrait>
    {
        private readonly T value;

        public Phantom(T value)
        {
            this.value = value;
        }

        public static implicit operator T(Phantom<T, TPhantomTrait> phantom)
            => phantom.value;
    }
}
