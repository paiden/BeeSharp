using System;
using FluentAssertions;
using Xunit;

namespace BeeSharp.Tests.Types
{
    public abstract class StructSemTypeTests<T, TBase> : SemTypeTests<T, TBase>
        where T : struct, IEquatable<T>, IComparable<T>
    {
        [Fact]
        public void EqualOp_ReturnsCorrectResult()
        {
            // Arrange
            var x = this.NewX();
            var y = this.NewY();

            x.Should().NotBeSameAs(y);

            // Act
            var xxc = this.InvokeEqualsOp(x, x);
            var xyc = this.InvokeEqualsOp(x, y);

            // Assert
            xxc.Should().BeTrue();
            xyc.Should().BeFalse();
        }

        [Fact]
        public void NotEqualOp_ReturnsCorrectResult()
        {
            // Arrange
            var x = this.NewX();
            var y = this.NewY();

            x.Should().NotBeSameAs(y);

            // Act
            var xxc = this.InvokeNotEqualsOp(x, x);
            var xyc = this.InvokeNotEqualsOp(x, y);

            // Assert
            xxc.Should().BeFalse();
            xyc.Should().BeTrue();
        }

        protected abstract bool InvokeEqualsOp(T x, T y);
        protected abstract bool InvokeNotEqualsOp(T x, T y);
    }
}
