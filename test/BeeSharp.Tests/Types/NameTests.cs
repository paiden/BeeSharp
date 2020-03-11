using System;
using BeeSharp.Types;
using FluentAssertions;
using Xunit;

namespace BeeSharp.Tests.Types
{
    public sealed class NameTests : StructSemTypeTests<Name>
    {
        protected override Name CreateX() => Name.New("X");

        protected override Name CreateY() => Name.New("Y");

        protected override bool InvokeEqualsOp(Name x, Name y) => x == y;

        protected override bool InvokeNotEqualsOp(Name x, Name y) => x != y;

        [Fact]
        public void GivenNonTrimmedString_NewThrows()
        {
            // Act
            Action a = () => Name.New(" a ");

            // Assert
            a.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GivenNonTrimmedString_OfTrims()
        {
            // Act
            var n = Name.Of(" a ");

            // Assert
            ((string)n).Should().Be("a");
        }
    }
}
