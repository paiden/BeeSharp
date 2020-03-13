namespace BeeSharp.Types
{
    public partial struct FileName
    {
        private readonly CIString value;

        private FileName(string value) => this.value = CIString.UncheckedNew(value);

        public static implicit operator string(FileName fn) => fn.value;
    }
}
