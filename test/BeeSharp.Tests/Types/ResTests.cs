using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using BeeSharp.Types;
using Xunit;

[assembly: ExcludeFromCodeCoverage]

namespace BeeSharp.Tests.Types
{
    public class ResTests
    {
        private static readonly Error DefError = Error.Unspecified("Initial Error");

        private static readonly Error OpError = Error.Unspecified("Operation Error");

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
            var rv = r.Unwrap();

            // Assert
            rv.Should().Be(Expected);
        }

        [Fact]
        public void GivenErrorResult_WhenUnwrappingResult_ThenThrowsInvalidOp()
        {
            // Arrange
            var r = Err();

            // Act
            Action a = () => r.Unwrap();

            // Assert
            a.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void GivenSucessResult_WhenInvokingResAnd_ThenInvokesThatOperation()
        {
            // Arrange
            const int InitVal = 3;
            const int AndVal = 28;
            var r = Ok(InitVal);

            // Act
            var r2 = r.And(Res.Ok(AndVal));

            // Assert
            r2.Unwrap().Should().Be(AndVal);
        }

        [Fact]
        public void GivenErrorResult_WhenInvokingResAnd_ThenReturnsTheErrorResult()
        {
            // Arrange
            const int AndVal = 28;
            var r = Err();

            // Act
            var r2 = r.And(Res.Ok(AndVal));

            // Assert
            r2.UnwrapErr().Should().Be(DefError);
        }

        [Fact]
        public void GivenSucessResult_WhenInvokingAndThen_ThenInvokesThatOperation()
        {
            // Arrange
            const int InitVal = 3;
            const int AddInThen = 28;
            var r = Ok(InitVal);

            // Act
            var r2 = r.And(i => Res.Ok(i + AddInThen));

            // Assert
            r2.Unwrap().Should().Be(InitVal + AddInThen);
        }

        [Fact]
        public void GivenErrorResult_WhenInvokingAndThen_ThenReturnsTheErrorResult()
        {
            // Arrange
            const int AddInThen = 28;
            var r = Err();

            // Act
            var r2 = r.And(i => Res.Ok(i + AddInThen));

            // Assert
            r2.IsErr.Should().BeTrue();
            r2.UnwrapErr().Should().Be(DefError);
        }

        [Fact]
        public void GivenSucessResult_WhenInvokingResOr_ThenReturnsOriginalRes()
        {
            // Arrange
            const int Initial = 3;
            const int OrVal = 4;
            var r = Ok(Initial);

            // Act
            var r2 = r.Or(Res.Ok(OrVal));

            // Assert
            r2.Unwrap().Should().Be(Initial);
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
            r2.Unwrap().Should().Be(Initial);
        }

        [Fact]
        public void GivenErrorResult_WhenInvokingResOr_ThenReturnsOrRes()
        {
            // Arrange
            const int OrVal = 4;
            var r = Err();

            // Act
            var r2 = r.Or(Res.Ok(OrVal));

            // Assert
            r2.Unwrap().Should().Be(OrVal);
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
            r2.Unwrap().Should().Be(OrVal);
        }

        [Fact]
        public void GivenSuccessResult_WhenInvokingOrElse_ThenReturnsOrginalResult()
        {
            // Arrange
            const int InitVal = 3;
            var r = Ok(InitVal);

            // Act
            var r2 = r.Or(e => Error.InvalidOp("Modified error message"));

            // Assert
            r2.Unwrap().Should().Be(InitVal);
        }


        [Fact]
        public void GivenErrorResult_WhenInvokingOrElse_ThenInvokesOrElseOperation()
        {
            // Arrange
            var r = Err();
            const string Modified = "Modified error message";

            // Act
            var r2 = r.Or(e => Error.InvalidOp(Modified));

            // Assert
            r2.UnwrapErr().Message.Should().Be(Modified);
        }

        [Fact]
        public void GivenSuccessResult_WhenMappedToOpt_ReturnNewResult()
        {
            // Arrange
            const int val = 2;
            var r = Ok(val);

            // Act
            var mapped = r.Map(i => Res.Ok($"{i}{i}"));

            // Assert
            mapped.Unwrap().Should().Be("22");
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
            mapped.Unwrap().Should().Be("22");
        }

        [Fact]
        public void GivenErrorResult_WhenMapped_ReturnsOriginalError()
        {
            // Arrange
            var r = Err();

            // Act
            var mapped = r.Map(i => Res.Ok($"{i}{i}"));

            // Assert
            mapped.UnwrapErr().Should().Be(DefError);
        }

        [Fact]
        public void GivenResultThatContainsSameValue_ThenContainsReturnsTrue()
        {
            // Arrange
            const int Val = 3;
            var r = Ok(Val);

            // Act
            var cont = r.Contains(Val);

            // Assert
            cont.Should().BeTrue();
        }

        [Fact]
        public void GivenErrorResult_ThenContainsReturnsFalse()
        {
            // Arrange
            const int Val = 3;
            var r = Err();

            // Act
            var cont = r.Contains(Val);

            // Assert
            cont.Should().BeFalse();
        }

        [Fact]
        public void GivenErrorResult_ThenContainsErrReturnsTrue()
        {
            // Arrange
            var r = Err();

            // Act
            var cont = r.ContainsErr(DefError);

            // Assert
            cont.Should().BeTrue();
        }

        [Fact]
        public void GivenSuccessResult_ThenContainsErrReturnsFalse()
        {
            // Arrange
            var r = Ok();

            // Act
            var cont = r.ContainsErr(DefError);

            // Assert
            cont.Should().BeFalse();
        }

        [Fact]
        public void OrWrapError_GivenOk_ReturnsOk()
        {
            // Arrange
            var r = Ok();

            // Act
            var w = r.OrWrapError(OpError);

            // Assert
            w.Should().Be(Ok());
        }

        [Fact]
        public void OrWrapError_GivenErr_ReturnsWrappedErr()
        {
            // Arrange
            var r = Err();

            // Act
            var w = r.OrWrapError(OpError);

            // Assert
            w.IsErr.Should().BeTrue();
            var e = w.UnwrapErr();
            e.Inner.Should().Be(DefError);
            e.Message.Should().Be(OpError.Message);
        }

        private Res<int> Ok(int value = 1)
            => Res.Ok(value);

        private Res<int> Err()
            => Res<int>.Err(DefError);
    }
}
