using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace BeeSharp.Tests.Types
{
    public abstract class SemTypeTests<T, TBase>
        where T : IEquatable<T>, IComparable<T>
    {
        [Fact]
        public void GivenInvalidInitValues_NewThrowsArgExc()
        {
            foreach (var iv in this.InvalidNewValues) // Parameterized test not possible as member cannot be static
            {
                // Act
                Action a = () => this.New(iv);

                // Assert
                a.Should().Throw<ArgumentException>($"'{iv}' is not a valid 'new' input for data type '{typeof(T)}'.");
            }
        }


        [Fact]
        public void GivenDiffRefButSameVal_EquatableEqualsIsTrue()
        {
            // Arrange
            var x = this.NewX();
            var y = this.NewX();
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
            var x = this.NewX();
            var y = this.NewX();
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
            var x = this.NewX();

            // Act
            var r = x.Equals(null);

            // Assert
            r.Should().BeFalse();
        }

        [Fact]
        public void GivenDiffValues_EqualsIsFalse()
        {
            // Arrange
            var x = this.NewX();
            var y = this.NewY();

            // Act
            var r = x.Equals(y);

            // Assert
            r.Should().BeFalse();
        }

        [Fact]
        public void WhenComparingXToY_ResultIsNegOne()
        {
            // Arrange
            var x = this.NewX();
            var y = this.NewY();

            // Act
            var r = x.CompareTo(y);

            // Assert
            r.Should().Be(-1);
        }


        [Fact]
        public void WhenComparingXToX_ResultIsZero()
        {
            // Arrange
            var x = this.NewX();
            var y = this.NewX();

            // Act
            var r = x.CompareTo(y);

            // Assert
            r.Should().Be(0);
        }


        [Fact]
        public void WhenComparingYToX_ResultIsPosOne()
        {
            // Arrange
            var x = this.NewY();
            var y = this.NewX();

            // Act
            var r = x.CompareTo(y);

            // Assert
            r.Should().Be(1);
        }

        [Fact]
        public void ImplementsGetHashCode()
        {
            // Arrange
            var x1 = this.NewX();
            var x2 = this.NewX();
            var y = this.NewY();

            // Act
            var x1h = x1.GetHashCode();
            var x2h = x2.GetHashCode();
            var yh = y.GetHashCode();

            // Assert
            x1h.Should().Be(x2h);
            x1h.Should().NotBe(yh);
        }

        protected abstract T NewX();
        protected abstract T NewY();
        protected abstract T New(TBase b);
        protected abstract IEnumerable<TBase> InvalidNewValues { get; }

        protected static IEnumerable<TA> Enum<TA>(params TA[] args)
            => args;
    }
}
