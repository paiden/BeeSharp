using System.Collections.Generic;
using BeeSharp.Types;
using FluentAssertions;
using Xunit;

namespace BeeSharp.Tests.Types
{
    public sealed class RelDirPathTests : StructSemTypeTests<RelDirPath, string>
    {
        protected override IEnumerable<string> InvalidNewValues
            => Enum("", ".", "\\", @".\a", @"C:\test\");

        protected override RelDirPath New(string b) => RelDirPath.New(b);

        protected override RelDirPath NewX() => RelDirPath.New(@".\x\");

        protected override RelDirPath NewY() => RelDirPath.New(@".\y\");

        protected override bool InvokeEqualsOp(RelDirPath x, RelDirPath y) => x == y;

        protected override bool InvokeNotEqualsOp(RelDirPath x, RelDirPath y) => x != y;

        [Theory]
        [InlineData(@"..\abc\..\", @"..\")]
        [InlineData(@"..\abc\.", @"..\abc\")]
        [InlineData(@"..\abc\..\cde\.", @"..\cde\")]
        public void Of_NormalizesPath(string input, string expected)
        {
            // Act
            var p = RelDirPath.Of(input);

            // Assert
            ((string)p.Unwrap()).Should().Be(expected);
        }
    }
}
