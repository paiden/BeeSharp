﻿using System;
using BeeSharp.Types;
using FluentAssertions;
using Xunit;

namespace BeeSharp.Tests.Types
{
    public sealed class AbsFilePathTests : StructSemTypeTests<AbsFilePath>
    {
        protected override AbsFilePath CreateX() => AbsFilePath.New(@"C:\test");

        protected override AbsFilePath CreateY() => AbsFilePath.New(@"C:\test.txt");

        protected override bool InvokeEqualsOp(AbsFilePath x, AbsFilePath y) => x == y;

        protected override bool InvokeNotEqualsOp(AbsFilePath x, AbsFilePath y) => x != y;

        [Theory]
        [InlineData(@"")]
        [InlineData(@"C:\")]
        [InlineData(@"C:\test\")]
        [InlineData(@".\test")]
        [InlineData(@".\test.txt")]
        public void GivenInvalidPath_NewThrowsArgExc(string input)
        {
            // Arrange

            // Act
            Action a = () => AbsFilePath.New(input);

            // Assert
            a.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(@"C:\test")]
        [InlineData(@"C:\test.txt")]
        public void GivenValidPath_NewCrateAbsPath(string input)
        {
            // Arrange

            // Act
            var p = AbsFilePath.New(input);

            // Assert
            ((string)p).Should().Be(input);
        }


        [Theory]
        [InlineData(@"C:\test.txt ", @"C:\test.txt")]
        [InlineData(@" C:\test.txt", @"C:\test.txt")]
        [InlineData(@" C:\ test.txt", @"C:\ test.txt")]
        public void GivenPathWithCorrectableUserErrors_OfCratesCorrectedAbsPath(string input, string expected)
        {
            // Arrange

            // Act
            var p = AbsFilePath.Of(input).Unwrap();

            // Assert
            ((string)p).Should().Be(expected);
        }

        [Theory]
        [InlineData(@" C:\test\")]
        public void GivenPathWithUncorrectableUserErrors_OfCratesError(string input)
        {
            // Arrange

            // Act
            var e = AbsFilePath.Of(input).UnwrapErr();

            // Assert
            e.Should().NotBeNull();
        }
    }
}