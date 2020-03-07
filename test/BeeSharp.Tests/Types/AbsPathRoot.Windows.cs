#if !BEES_UNIX


using BeeSharp.Types;

namespace BeeSharp.Tests.Types
{
    public partial struct AbsPathRoot
    {
        private const char Separator = '\\';
        private const string SeparatorAsString = "\\";

        private readonly CIString value;

        public AbsPathRoot(string r) => this.value = CIString.Of(r);
    }
}

#endif
