﻿using System;
using System.Collections.Generic;
using BeeSharp.Types;
using FluentAssertions;
using Xunit;

namespace BeeSharp.Tests.Types
{
    public sealed class NameTests : StructSemTypeTests<Name, string>
    {
        protected override IEnumerable<string> InvalidNewValues
        {
            get
            {
                yield return "";
            }
        }

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

        protected override Name NewX() => Name.New("X");

        protected override Name NewY() => Name.New("Y");

        protected override bool InvokeEqualsOp(Name x, Name y) => x == y;

        protected override bool InvokeNotEqualsOp(Name x, Name y) => x != y;

        protected override Name New(string b) => Name.New(b);
    }
}
