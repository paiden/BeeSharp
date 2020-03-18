namespace BeeSharp.Types
{
    public partial struct RelDirPath
    {
        public static RelDirPath Parent = new RelDirPath(@"..\");
        public static RelDirPath Current = new RelDirPath(@".\");

        private readonly CIString value;

        private RelDirPath(string s) => value = CIString.UncheckedNew(s);

        public static implicit operator string(RelDirPath rp) => rp.value;
    }
}
