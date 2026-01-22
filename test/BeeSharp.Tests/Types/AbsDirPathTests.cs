using System;
using System.Collections.Generic;
using BeeSharp.Types;
using FluentAssertions;
using Xunit;

namespace BeeSharp.Tests.Types
{
    public sealed class AbsDirPathTests : StructSemTypeTests<AbsDirPath, string>
    {
        protected override IEnumerable<string> InvalidNewValues
        {
            get
            {
                yield return @"";
                yield return @"C:";
            }
        }

        protected override AbsDirPath NewX() => AbsDirPath.New(@"C:\X\");

        protected override AbsDirPath NewY() => AbsDirPath.New(@"C:\Y\");

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
            var p = AbsDirPath.Of(input).UnwrapOrThrow();

            // Assert
            p.Should().Be(AbsDirPath.New(expected));
        }

        [Theory]
        [InlineData(@"C:\test:")]
        public void GivenPathWithUncorrectableUserInputErrors_ReturnsErrorRes(string input)
        {
            // Act
            var e = AbsDirPath.Of(input).UnwrapErrOrThrow();

            // Assert
            e.Should().NotBeNull();
        }

        [Fact]
        public void GivenDirPath_WhenCombinedWithFilename_IsAAbsFilePath()
        {
            // Arrange
            var d = AbsDirPath.Of(@"C:\test").UnwrapOrThrow();
            var f = FileName.New("test.txt");

            // Act
            var fp = d / f;

            // Assert
            fp.Should().Be(AbsFilePath.New(@"C:\test\test.txt"));
        }

        [Theory]
        [InlineData(@"C:\test\", @"..\", @"C:\")]
        [InlineData(@"C:\test\", @".\x\", @"C:\test\x\")]
        [InlineData(@"C:\test\abc\", @"..\x\", @"C:\test\x\")]
        public void GivenAbsPath_WhenCombinedWithRelDirPath_IsNewAbsDirPath(string abs, string rel, string expected)
        {
            // Arrange
            var ap = AbsDirPath.New(abs);
            var rdp = RelDirPath.New(rel);

            // Act
            var r = ap / rdp;

            // Assert
            r.Should().Be(AbsDirPath.New(expected));
        }

        protected override AbsDirPath New(string b) => AbsDirPath.New(b);
    }
}
