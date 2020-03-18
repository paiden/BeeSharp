using System.Collections.Generic;
using BeeSharp.Types;

namespace BeeSharp.Tests.Types
{
    public sealed class FileNameTests : StructSemTypeTests<FileName, string>
    {
        protected override IEnumerable<string> InvalidNewValues
        {
            get
            {
                yield return "";
            }
        }

        protected override FileName New(string b) => FileName.New(b);

        protected override FileName NewX() => FileName.New("x.txt");

        protected override FileName NewY() => FileName.New("y.txt");

        protected override bool InvokeEqualsOp(FileName x, FileName y) => x == y;

        protected override bool InvokeNotEqualsOp(FileName x, FileName y) => x != y;
    }
}
