﻿using System;
using BeeSharp.Types;
using FluentAssertions;
using Xunit;

namespace BeeSharp.Tests.Types
{
    public sealed class AbsDirPathTests : StructSemTypeTests<AbsDirPath>
    {
        protected override AbsDirPath CreateX() => AbsDirPath.New(@"C:\X\");

        protected override AbsDirPath CreateY() => AbsDirPath.New(@"C:\Y\");

        protected override bool InvokeEqualsOp(AbsDirPath x, AbsDirPath y) => x == y;

        protected override bool InvokeNotEqualsOp(AbsDirPath x, AbsDirPath y) => x != y;

        [Fact]
        public void GivenPathWithoutEndingSeparator_NewThrowsArg()
        {
            // Act
            Action a = () => AbsDirPath.New(@"C:\test");

            // Assert
            a.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(@"C:\test", @"C:\test\")]
        [InlineData(@"C:\test ", @"C:\test\")]
        [InlineData(@" C:\test", @"C:\test\")]
        [InlineData(@" C:\test ", @"C:\test\")]
        [InlineData(@"C:\test\ ", @"C:\test\")]
        [InlineData(@"C:\test/", @"C:\test\")]
        [InlineData(@"C:\ test", @"C:\ test\")]
        [InlineData(@"C :\test", @"C:\test\")]
        [InlineData(@" C: \test", @"C:\test\")]
        public void GivenPathWithUserInputErrors_OfCreatesCorrectedResult(string input, string expected)
        {
            // Act
            var p = AbsDirPath.Of(input).Unwrap();

            // Assert
            p.Should().Be(AbsDirPath.New(expected));
        }

        [Theory(Skip = "Probably not working because of a .net core fx bug: https://stackoverflow.com/questions/60634204/getfullpath-behavior-in-net-core-3-1-2-differs-from-net-4-6-1")]
        [InlineData(@"C:\test:")]
        public void GivenPathWithUncorrectableUserInputErrors_ReturnsErrorRes(string input)
        {
            // Act
            var e = AbsDirPath.Of(input).UnwrapErr();

            // Assert
            e.Should().NotBeNull();
        }
    }
}