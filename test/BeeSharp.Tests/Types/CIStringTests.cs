using System.Collections.Generic;
using System.Linq;
using BeeSharp.Types;
using FluentAssertions;
using Xunit;

namespace BeeSharp.Tests.Types
{
    public class CIStringTests : StructSemTypeTests<CIString, string>
    {
        protected override IEnumerable<string> InvalidNewValues => Enumerable.Empty<string>();

        [Fact]
        public void WhenComparing_CasingIsIgnored()
        {
            // Arrange
            var la = CIString.New("a");
            var ua = CIString.New("A");

            // Act
            var r = la.Equals(ua);

            // Assert
            r.Should().BeTrue();
        }

        protected override CIString New(string b) => CIString.New("x");
        protected override CIString NewX() => CIString.New("x");
        protected override CIString NewY() => CIString.New("y");
        protected override bool InvokeEqualsOp(CIString x, CIString y) => x == y;
        protected override bool InvokeNotEqualsOp(CIString x, CIString y) => x != y;
    }
}
