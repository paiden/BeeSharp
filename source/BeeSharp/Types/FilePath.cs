using System;
using System.Collections.Generic;
using System.Text;

namespace BeeSharp.Types
{
    public struct FilePath : IConstrainedType<FilePath>
    {
        private readonly Opt<AbsFilePath> absFilePath;
        private readonly Opt<RelFilePath> relFilePath;

        public int CompareTo(FilePath other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(FilePath other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}
