using System;
using BeeSharp.Types;
using FluentAssertions;
using Xunit;

namespace BeeSharp.Tests.Types
{
    public sealed class TrimmedTests : StructSemTypeTests<Trimmed>
    {
        protected override Trimmed CreateX() => Trimmed.New("x");

        protected override Trimmed CreateY() => Trimmed.New("y");

        protected override bool InvokeEqualsOp(Trimmed x, Trimmed y) => x == y;

        protected override bool InvokeNotEqualsOp(Trimmed x, Trimmed y) => x != y;

        [Fact]
        public void GivenNonTrimmedString_NewThrows()
        {
            // Act
            Action a = () => Trimmed.New(" ");

            // Assert
            a.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GivenNonTrimmedString_OfTrims()
        {
            // Arrange

            // Act
            var t = Trimmed.Of(" a ");

            // Assert
            ((string)t).Should().Be("a");
        }
    }
}
