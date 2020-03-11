namespace BeeSharp.Types
{
    public partial struct AbsFilePath
    {
        private const char SepChar = '\\';
        private const char AltSepChar = '/';

        private readonly CIString value;

        public AbsFilePath(string p) => this.value = CIString.UncheckedNew(p);
    }
}
