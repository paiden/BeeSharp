using System;
using BeeSharp.Types;
using FluentAssertions;
using Xunit;

namespace BeeSharp.Tests.Types
{
    public sealed class RelFilePathTests
    {
        [Theory]
        [InlineData(@"")]
        [InlineData(@".")]
        [InlineData(@".\")]
        [InlineData(@"\x")]
        [InlineData(@"x")]
        [InlineData(@"test.txt")]
        [InlineData(@"test.txt\")]
        [InlineData(@"\\test.txt")]
        [InlineData(@"c:\test.txt")]
        [InlineData(@"./test.txt")]
        public void New_GivenInvalidInput_ThrowsArgExc(string input)
        {
            // Act
            Action a = () => RelFilePath.New(input);

            // Assert
            a.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(@".\test.txt")]
        [InlineData(@".\test\test.txt")]
        public void New_GivenValidInput_DoesNotThrow(string input)
        {
            // Act
            var p = RelFilePath.New(input);

            // Assert
            p.ToString().Should().Be(input);
        }

        [Theory]
        [InlineData(@"")]
        [InlineData(@".")]
        [InlineData(@".\")]
        [InlineData(@"x")]
        [InlineData(@"test.txt\")]
        [InlineData(@"\\test.txt")]
        [InlineData(@"c:\test.txt")]
        public void Of_GivenInvalidInput_ThrowsArgExc(string input)
        {
            // Act
            Action a = () => RelFilePath.Of(input);

            // Assert
            a.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(@"\x", @".\x")]
        [InlineData(@"x", @".\x")]
        [InlineData(@"test.txt", @".\test.txt")]
        [InlineData(@"./test.txt", @".\test.txt")]
        public void New_GivenCorrectableInput_CreatsCorrectedRelPath(string input, string expected)
        {
            // Act
            var rp = RelFilePath.Of(input);

            // Assert
            ((string)rp).Should().Be(expected);
        }

        [Theory]
        [InlineData(@".\test.txt")]
        [InlineData(@".\test\test.txt")]
        public void Of_GivenValidInput_DoesNotThrow(string input)
        {
            // Act
            var p = RelFilePath.Of(input);

            // Assert
            p.ToString().Should().Be(input);
        }
    }
}
