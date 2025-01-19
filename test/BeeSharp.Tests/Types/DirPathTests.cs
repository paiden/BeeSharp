using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BeeSharp.Extensions;
using BeeSharp.Types;
using FluentAssertions;
using Xunit;

namespace BeeSharp.Tests.Types
{
    public sealed class DirPathTests : StructSemTypeTests<DirPath, string>
    {
        private readonly string CurDir = Directory.GetCurrentDirectory();

        protected override IEnumerable<string> InvalidNewValues => Enumerable.Empty<string>();

        protected override bool InvokeEqualsOp(DirPath x, DirPath y) => x == y;

        protected override bool InvokeNotEqualsOp(DirPath x, DirPath y) => x != y;

        protected override DirPath New(string b) => throw new NotSupportedException();

        protected override DirPath NewX() => new DirPath(RelDirPath.New(@"..\..\test\"));

        protected override DirPath NewY() => new DirPath(AbsDirPath.New(@"c:\test\"));

        [Theory]
        [InlineData(@"C:\test\", @"C:\base\", @"C:\test\")]
        [InlineData(@".\test\", @"C:\base\", @"C:\base\test\")]
        public void ToAbsPath_UsesBasePathWhenIputIsRelativePathOtherwiseReturnsInputItself(string input, string basePath, string expected)
        {
            // Arrange
            var p = DirPath.Of(input).Unwrap();

            // Act
            var ap = p.ToAbsDirPath(AbsDirPath.New(basePath));

            // Assert
            ((string)ap).Should().Be(expected);
        }

        [Theory]
        [InlineData(@".\")]
        [InlineData(@".\x\")]

        public void ToAbsPath_WhenNoBasePathGiven_UsesCurrentDir(string input)
        {
            // Arrange
            string expected = Path.Combine(CurDir, input.Replace(@".\", string.Empty))
                .EnsureEndsWith('\\');
            var p = DirPath.Of(input).Unwrap();

            // Act
            var r = p.ToAbsDirPath();

            // Assert
            ((string)r).Should().Be(expected);
        }
    }
}
