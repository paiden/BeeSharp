using System.Collections.Generic;
using BeeSharp.Types;

namespace BeeSharp.Tests.Types
{
    public sealed class FileNameTests : StructSemTypeTests<FileName, string>
    {
        protected override IEnumerable<string> InvalidInitValues
        {
            get
            {
                yield return "";
            }
        }

        protected override FileName Create(string b) => FileName.New(b);

        protected override FileName CreateX() => FileName.New("x.txt");

        protected override FileName CreateY() => FileName.New("y.txt");

        protected override bool InvokeEqualsOp(FileName x, FileName y) => x == y;

        protected override bool InvokeNotEqualsOp(FileName x, FileName y) => x != y;
    }
}
