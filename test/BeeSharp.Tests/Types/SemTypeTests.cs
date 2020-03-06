using System;
using FluentAssertions;
using Xunit;

namespace BeeSharp.Tests.Types
{
    public abstract class SemTypeTests<T>
        where T : IEquatable<T>, IComparable<T>
    {
        [Fact]
        public void GivenDiffRefButSameVal_EquatableEqualsIsTrue()
        {
            // Arrange
            var x = this.CreateX();
            var y = this.CreateX();
            x.Should().NotBeSameAs(y);

            // Act
            var r = x.Equals(y);

            // Assert
            r.Should().BeTrue();
        }

        [Fact]
        public void GivenDiffRefButSameVal_ObjectEqualsIsTrue()
        {
            // Arrange
            var x = this.CreateX();
            var y = this.CreateX();
            x.Should().NotBeSameAs(y);

            // Act
            var r = x.Equals((object)y);

            // Assert
            r.Should().BeTrue();
        }

        [Fact]
        public void EqualsWithNullIsFAlse()
        {
            // Arrange
            var x = this.CreateX();

            // Act
            var r = x.Equals(null);

            // Assert
            r.Should().BeFalse();
        }

        [Fact]
        public void GivenDiffValues_EqualsIsFalse()
        {
            // Arrange
            var x = this.CreateX();
            var y = this.CreateY();

            // Act
            var r = x.Equals(y);

            // Assert
            r.Should().BeFalse();
        }

        [Fact]
        public void WhenComparingXToY_ResultIsNegOne()
        {
            // Arrange
            var x = this.CreateX();
            var y = this.CreateY();

            // Act
            var r = x.CompareTo(y);

            // Assert
            r.Should().Be(-1);
        }


        [Fact]
        public void WhenComparingXToX_ResultIsZero()
        {
            // Arrange
            var x = this.CreateX();
            var y = this.CreateX();

            // Act
            var r = x.CompareTo(y);

            // Assert
            r.Should().Be(0);
        }


        [Fact]
        public void WhenComparingYToX_ResultIsPosOne()
        {
            // Arrange
            var x = this.CreateY();
            var y = this.CreateX();

            // Act
            var r = x.CompareTo(y);

            // Assert
            r.Should().Be(1);
        }

        [Fact]
        public void ImplementsGetHashCode()
        {
            // Arrange
            var x1 = this.CreateX();
            var x2 = this.CreateX();
            var y = this.CreateY();

            // Act
            var x1h = x1.GetHashCode();
            var x2h = x2.GetHashCode();
            var yh = y.GetHashCode();

            // Assert
            x1h.Should().Be(x2h);
            x1h.Should().NotBe(yh);
        }

        protected abstract T CreateX();
        protected abstract T CreateY();
    }
}
