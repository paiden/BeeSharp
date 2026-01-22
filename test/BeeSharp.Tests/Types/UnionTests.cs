using System;
using System.Globalization;
using System.Threading;

using BeeSharp.Types;

using FluentAssertions;

using Xunit;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace BeeSharp.Tests.Types
{
    internal class Person
    {
        public U<Unborn, Living, Dead> state = new Unborn();

        public struct Unborn
        {
        }
        public struct Living
        {
            string Name;
            DateTime DateOfBirth;
        }
        public struct Dead
        {
            string Name;
            DateTime DateOfBirth;
        }
    }

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

        public static TheoryData<U<int, double, string>, U<int, double, string>, bool> EqualsTestData { get; } =
            new TheoryData<U<int, double, string>, U<int, double, string>, bool>()
            {
                { CreateInt(), null, false },
                { CreateInt(), CreateInt(2), false },
                { CreateInt(), CreateInt(), true },
                { CreateInt(), CreateDouble(), false },
                { CreateString(), CreateString(), true },
            };

        [Theory]
        [MemberData(nameof(EqualsTestData))]
        public void Eqals_ReturnsCorrectResult(U<int, double, string> x, U<int, double, string> y, bool expected)
        {
            // Act
            bool er = x.Equals(y);

            // Assert
            er.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(EqualsTestData))]
        public void EqalsObj_ReturnsCorrectResult(U<int, double, string> x, object y, bool expected)
        {
            // Act
            bool er = x.Equals(y);

            // Assert
            er.Should().Be(expected);
        }


        [Fact]
        public void Foo()
        {
            var x = new U<int, string>("hello");

            var s = x.ToString();
        }

        private static string Map(U<int, double, string> u)
            => u.Map(
                   i => i.ToString("Mapped int: 0", CultureInfo.InvariantCulture),
                   d => d.ToString("Mapped double: 0.0", CultureInfo.InvariantCulture),
                   s => $"Mapped string: {s}");

        private static void Do(U<int, double, string> u, out ManualResetEventSlim ie, out ManualResetEventSlim de, out ManualResetEventSlim se)
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


        private static U<int, double, string> CreateInt(int i = IDef)
            => new U<int, double, string>(i);

        private static U<int, double, string> CreateDouble(double d = DDef)
            => new U<int, double, string>(d);

        private static U<int, double, string> CreateString(string s = SDef)
            => new U<int, double, string>(s);
    }
}
