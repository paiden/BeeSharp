using System;
using System.Diagnostics.CodeAnalysis;
using BeeSharp.Types;
using FluentAssertions;
using Xunit;

[assembly: ExcludeFromCodeCoverage]

namespace BeeSharp.Tests.Types
{
    public class RTests
    {
        private static readonly Err DefErr = BeeSharp.Types.Err.Unspecified("Initial Error");

        private static readonly Err OpErr = BeeSharp.Types.Err.Unspecified("Operation Error");

        [Fact]
        public void GivenSucessResult_IsErrorIsFalse()
        {
            // Arrange
            var r = Ok();

            // Assert
            r.IsErr.Should().BeFalse();
        }

        [Fact]
        public void GivenSucessResult_IsOkIsTrue()
        {
            // Arrange
            var r = Ok();

            // Assert
            r.IsOk.Should().BeTrue();
        }

        [Fact]
        public void GivenSucessResult_WhenUnwrappingResult_ThenReturnsThatResultValue()
        {
            // Arrange
            const int Expected = 4;
            var r = Ok(Expected);

            // Act
            var rv = r.UnwrapOrThrow();

            // Assert
            rv.Should().Be(Expected);
        }

        [Fact]
        public void GivenSucessResult_WhenInvokingResOr_ThenReturnsOriginalRes()
        {
            // Arrange
            const int Initial = 3;
            const int OrVal = 4;
            var r = Ok(Initial);

            // Act
            var r2 = r.Or(R<int>.Ok(OrVal));

            // Assert
            r2.UnwrapOrThrow().Should().Be(Initial);
        }

        [Fact]
        public void GivenSucessResult_WhenInvokingOr_ThenReturnsOriginalRes()
        {
            // Arrange
            const int Initial = 3;
            const int OrVal = 4;
            var r = Ok(Initial);

            // Act
            var r2 = r.Or(OrVal);

            // Assert
            r2.Should().Be(Initial);
        }

        [Fact]
        public void GivenErrorResult_WhenInvokingResOr_ThenReturnsOrRes()
        {
            // Arrange
            const int OrVal = 4;
            var r = Err();

            // Act
            var r2 = r.Or(R<int>.Ok(OrVal));

            // Assert
            r2.UnwrapOrThrow().Should().Be(OrVal);
        }

        [Fact]
        public void GivenErrorResult_WhenInvokingOr_ThenReturnsOrRes()
        {
            // Arrange
            const int OrVal = 4;
            var r = Err();

            // Act
            var r2 = r.Or(OrVal);

            // Assert
            r2.Should().Be(OrVal);
        }

        [Fact]
        public void GivenSuccessResult_WhenMappedToNonOpt_ReturnNewResult()
        {
            // Arrange
            const int val = 2;
            var r = Ok(val);

            // Act
            var mapped = r.Map(i => $"{i}{i}");

            // Assert
            mapped.UnwrapOrThrow().Should().Be("22");
        }

        [Fact]
        public void OrWrapError_GivenErr_ReturnsWrappedErr()
        {
            // Arrange
            var r = Err();

            // Act
            var w = r.OrWrapError(OpErr);

            // Assert
            w.IsErr.Should().BeTrue();
            var e = w.UnwrapErrOrThrow();
            e.Inner.Should().Be(DefErr);
            e.Message.Should().Be(OpErr.Message);
        }

        private R<int> Ok(int value = 1)
            => R<int>.Ok(value);

        private R<int> Err()
            => R<int>.Err(DefErr);
    }
}
