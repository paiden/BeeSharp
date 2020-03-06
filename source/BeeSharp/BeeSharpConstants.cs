namespace BeeSharp
{
    internal static class BeeSharpConstants
    {
        public const string DbgCond = "DEBUG";

        public static class Compare
        {
            public const int ThisPreceedes = -1;
            public const int Equal = 0;
            public const int ThisFollows = 1;
        }

        public class X
        {

        }

        public class Y
        {
            public void foox()
            {
                var x = new X();
            }
        }
    }
}
