namespace BeeSharp.Utils
{
    public static class Hash
    {
        private const int HashPrime = 31;
        private const int NullHashPrime = 17;

        public static int Get<T0, T1>(T0 a0, T1 a1)
        {
            unchecked
            {
                var h0 = a0?.GetHashCode() ?? NullHashPrime;
                var h1 = a1?.GetHashCode() ?? NullHashPrime;
                return HashPrime * h0 + h1;

            }
        }

        public static int Get<T0, T1, T2>(T0 a0, T1 a1, T2 a2)
            => Get(Get(a0, a1), a2);

        public static int Get<T0, T1, T2, T3>(T0 a0, T1 a1, T2 a2, T3 a3)
            => Get(Get(a0, a1, a2), a3);
    }
}
