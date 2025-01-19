using System;

namespace BeeSharp.Types
{
    public struct RelFilePath : IConstrainedType<RelFilePath>
    {
        private readonly string value;

        private RelFilePath(string p) => this.value = p;

        public static implicit operator string(RelFilePath rfp) => rfp.value;

        public static RelFilePath New(string p) => new RelFilePath(p);

        public static RelFilePath Of(string p) => new RelFilePath(p);

        public static RelFilePath UncheckedNew(string p) => new RelFilePath(p);

        public int CompareTo(RelFilePath other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(RelFilePath other)
        {
            throw new NotImplementedException();
        }

        public override string ToString() => this.value;
    }
}
