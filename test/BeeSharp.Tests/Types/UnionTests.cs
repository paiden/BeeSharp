using System.Globalization;
using System.Threading;
using BeeSharp.Types;
using FluentAssertions;
using Xunit;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace BeeSharp.Tests.Types
{
    public class UnionTests
    {
        private const int IDef = 1;
        private const double DDef = 2.0;
        private const string SDef = "T";

        [Fact]
        public void Map_GivenIntUnion_MapsTheInt()
        {
            // Arrange
            var x = CreateInt();

            // Act
            var r = Map(x);

            // Assert
            r.Should().Be("Mapped int: 1");
        }

        [Fact]
        public void Map_GivenDoubleUnion_MapsTheDouble()
        {
            // Arrange
            var x = CreateDouble();

            // Act
            var r = Map(x);

            // Assert
            r.Should().Be("Mapped double: 2.0");
        }

        [Fact]
        public void Map_GivenStringUnion_MapsTheString()
        {
            // Arrange
            var x = CreateString();

            // Act
            var r = Map(x);

            // Assert
            r.Should().Be("Mapped string: T");
        }

        [Fact]
        public void Do_GivenIntUnion_RunsIntAction()
        {
            // Arrange
            var u = CreateInt();

            // Act
            Do(u, out var ie, out var de, out var se);

            // Assert
            ie.IsSet.Should().BeTrue();
            de.IsSet.Should().BeFalse();
            se.IsSet.Should().BeFalse();
        }


        [Fact]
        public void Do_GivenDoubleUnion_RunsDoubleAction()
        {
            // Arrange
            var u = CreateDouble();

            // Act
            Do(u, out var ie, out var de, out var se);

            // Assert
            ie.IsSet.Should().BeFalse();
            de.IsSet.Should().BeTrue();
            se.IsSet.Should().BeFalse();
        }

        [Fact]
        public void Do_GivenStringUnion_RunsIntAction()
        {
            // Arrange
            var u = CreateString();

            // Act
            Do(u, out var ie, out var de, out var se);

            // Assert
            ie.IsSet.Should().BeFalse();
            de.IsSet.Should().BeFalse();
            se.IsSet.Should().BeTrue();
        }

        public static TheoryData<Union<int, double, string>, Union<int, double, string>, bool> EqualsTestData { get; } =
            new TheoryData<Union<int, double, string>, Union<int, double, string>, bool>()
            {
                { CreateInt(), null, false },
                { CreateInt(), CreateInt(2), false },
                { CreateInt(), CreateInt(), true },
                { CreateInt(), CreateDouble(), false },
                { CreateString(), CreateString(), true },
            };

        [Theory]
        [MemberData(nameof(EqualsTestData))]
        public void Eqals_ReturnsCorrectResult(Union<int, double, string> x, Union<int, double, string> y, bool expected)
        {
            // Act
            bool er = x.Equals(y);

            // Assert
            er.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(EqualsTestData))]
        public void EqalsObj_ReturnsCorrectResult(Union<int, double, string> x, object y, bool expected)
        {
            // Act
            bool er = x.Equals(y);

            // Assert
            er.Should().Be(expected);
        }

        private static string Map(Union<int, double, string> u)
            => u.Map(
                   i => i.ToString("Mapped int: 0", CultureInfo.InvariantCulture),
                   d => d.ToString("Mapped double: 0.0", CultureInfo.InvariantCulture),
                   s => $"Mapped string: {s}");

        private static void Do(Union<int, double, string> u, out ManualResetEventSlim ie, out ManualResetEventSlim de, out ManualResetEventSlim se)
        {
            var iel = new ManualResetEventSlim();
            var del = new ManualResetEventSlim();
            var sel = new ManualResetEventSlim();

            ie = iel;
            de = del;
            se = sel;

            u.Do(
                i => iel.Set(),
                d => del.Set(),
                s => sel.Set());
        }


        private static Union<int, double, string> CreateInt(int i = IDef)
            => new Union<int, double, string>(i);

        private static Union<int, double, string> CreateDouble(double d = DDef)
            => new Union<int, double, string>(d);

        private static Union<int, double, string> CreateString(string s = SDef)
            => new Union<int, double, string>(s);
    }
}
