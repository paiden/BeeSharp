namespace BeeSharp.Types
{
    /// <summary>
    /// Captures user input that can either be absolute or relative directory path
    /// and provides methods to convert either input to an absolute directory path when needed.
    /// </summary>
    public partial struct DirPath : IConstrainedType<DirPath>
    {
        private readonly Opt<RelDirPath> rel;
        private readonly Opt<AbsDirPath> abs;

        public DirPath(RelDirPath p)
        {
            abs = Opt.None<AbsDirPath>();
            rel = Opt.Some(p);
        }

        public DirPath(AbsDirPath p)
        {
            abs = Opt.Some(p);
            rel = Opt.None<RelDirPath>();
        }

        public static bool operator ==(DirPath x, DirPath y) => x.Equals(y);

        public static bool operator !=(DirPath x, DirPath y) => !x.Equals(y);

        public static R<DirPath> Of(string p)
        {
            var rp = RelDirPath.Of(p);
            if (rp.IsOk) { return R<DirPath>.Ok(new DirPath(rp.UnwrapOrThrow())); }

            var ap = AbsDirPath.Of(p);
            if (ap.IsOk) { return R<DirPath>.Ok(new DirPath(ap.UnwrapOrThrow())); }

            return R<DirPath>.Err(Err.InvalidOp($"'{p}' is not a valid absolute or relative directory path."));
        }

        public int CompareTo(DirPath other) => this.StringRep.CompareTo(other.StringRep);

        public bool Equals(DirPath other) => this.StringRep.Equals(other.StringRep);

        public override bool Equals(object obj) => obj is DirPath dp && this.Equals(dp);

        public override int GetHashCode() => this.StringRep.GetHashCode();

        public AbsDirPath ToAbsDirPath(AbsDirPath basePath)
            => this.rel.IsSome ? basePath / this.rel.Unwrap() : this.abs.Unwrap();

        public AbsDirPath ToAbsDirPath() => this.ToAbsDirPath(AbsDirPath.Current);

        private string StringRep => this.rel.IsSome
            ? (string)this.rel.Unwrap()
            : this.abs.Unwrap();
    }
}
