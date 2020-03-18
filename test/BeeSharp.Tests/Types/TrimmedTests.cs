using System;
using System.Collections.Generic;
using BeeSharp.Types;
using FluentAssertions;
using Xunit;

namespace BeeSharp.Tests.Types
{
    public sealed class TrimmedTests : StructSemTypeTests<Trimmed, string>
    {
        protected override IEnumerable<string> InvalidNewValues { get { yield break; } }

        protected override Trimmed NewX() => Trimmed.New("x");

        protected override Trimmed NewY() => Trimmed.New("y");

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

        protected override Trimmed New(string b)
        {
            throw new NotImplementedException();
        }
    }
}
