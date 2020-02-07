using System;
using BeeSharp.Types;
using FluentAssertions;
using Xunit;

namespace BeeSharp.Tests.Types
{
    public class OptTests
    {
        private const int DefSome = 1;
        private const int OpValue = DefSome + 1;

        private static readonly Error DefErr = Error.Unspecified("e");

        [Fact]
        public void And_GivenSomeOpt_ShouldReturnAnd()
        {
            // Arrange
            var o = Some();

            // Act
            var a = o.And(OpValue);

            // Assert
            a.Unwrap().Should().Be(OpValue);
        }

        [Fact]
        public void And_GivenNoneOpt_ShouldReturNone()
        {
            // Arrange
            var o = None();

            // Act
            var a = o.And(OpValue);

            // Assert
            a.IsNone.Should().BeTrue();
        }

        [Fact]
        public void AndOpt_GivenSome_ShouldReturnAnd()
        {
            // Arrange
            var o = Some();

            // Act
            var a = o.And(Some(OpValue));

            // Assert
            a.Unwrap().Should().Be(OpValue);
        }

        [Fact]
        public void AndOpt_GivenNone_ShouldReturnNone()
        {
            // Arrange
            var o = None();

            // Act
            var a = o.And(Some(OpValue));

            // Assert
            a.IsNone.Should().BeTrue();
        }


        [Fact]
        public void AndThen_GivenSome_ShouldReturnAnd()
        {
            // Arrange
            var o = Some();

            // Act
            var a = o.AndThen(i => i + OpValue);

            // Assert
            a.Unwrap().Should().Be(DefSome + OpValue);
        }

        [Fact]
        public void AndThen_GivenNone_ShouldReturnNone()
        {
            // Arrange
            var o = None();

            // Act
            var a = o.AndThen(i => i + OpValue);

            // Assert
            a.IsNone.Should().BeTrue();
        }

        [Fact]
        public void AndThenOpt_GivenSome_ShouldReturnAnd()
        {
            // Arrange
            var o = Some();

            // Act
            var a = o.AndThen(i => Some(i + OpValue));

            // Assert
            a.Unwrap().Should().Be(DefSome + OpValue);
        }

        [Fact]
        public void AndThenOpt_GivenNone_ShouldReturnNone()
        {
            // Arrange
            var o = None();

            // Act
            var a = o.AndThen(i => Some(i + OpValue));

            // Assert
            a.IsNone.Should().BeTrue();
        }

        [Fact]
        public void Contains_GivenSome_ShouldReturnTrue()
        {
            // Arrange
            var o = Some();

            // Act
            var cont = o.Contains(DefSome);

            // Assert
            cont.Should().BeTrue();
        }

        [Fact]
        public void Contains_GivenNone_ShouldReturnFalse()
        {
            // Arrange
            var o = None();

            // Act
            var cont = o.Contains(DefSome);

            // Assert
            cont.Should().BeFalse();
        }

        [Fact]
        public void Expect_GivenSome_ShouldReturnSome()
        {
            // Arrange
            var o = Some();

            // Act
            var uw = o.Expect("X");

            // Assert
            uw.Should().Be(DefSome);
        }

        [Fact]
        public void Expect_GivenNone_ShouldThrow()
        {
            // Arrange
            var o = None();

            // Act
            Action a = () => o.Expect("X");

            // Assert
            a.Should().Throw<InvalidOperationException>().WithMessage("X");
        }

        [Fact]
        public void ExpectNone_GivenSome_ShouldThrow()

        {
            // Arrange
            const string Message = "X";
            var o = Some();

            // Act
            Action a = () => o.ExpectNone(Message);

            // Assert
            a.Should().Throw<InvalidOperationException>().WithMessage(Message);
        }

        [Fact]
        public void ExpectNone_GivenNone_ShouldNotThrow()

        {
            // Arrange
            const string Message = "X";
            var o = None();

            // Act
            Action a = () => o.ExpectNone(Message);

            // Assert
            a.Should().NotThrow();
        }


        [Fact]
        public void Filter_GivenSome_WhenFilterMatches_ShouldReturnSome()
        {
            // Arrange
            var o = Some();

            // Act
            var f = o.Filter(i => i == DefSome);

            // Assert
            f.Unwrap().Should().Be(DefSome);
        }

        [Fact]
        public void Filter_GivenSome_WhenFilterDoesntMatch_ShouldReturnNone()
        {
            // Arrange
            var o = Some();

            // Act
            var f = o.Filter(i => i != DefSome);

            // Assert
            f.Should().Be(None());
        }

        [Fact]
        public void Filter_GivenNone_WhenFilterMatches_ShouldReturnNone()
        {
            // Arrange
            var o = None();

            // Act
            var f = o.Filter(i => true);

            // Assert
            f.Should().Be(None());
        }

        [Fact]
        public void OrOpt_GivenSome_ShouldReturnSome()
        {
            // Arrange
            var o = Some();

            // Act
            var f = o.Or(Some(OpValue));

            // Assert
            f.Should().Be(o);
        }

        [Fact]
        public void OrOpt_GivenNone_ShouldReturnOr()
        {
            // Arrange
            var o = None();

            // Act
            var f = o.Or(Some(OpValue));

            // Assert
            f.Should().Be(Some(OpValue));
        }

        [Fact]
        public void Or_GivenSome_ShouldReturnSome()
        {
            // Arrange
            var o = Some();

            // Act
            var f = o.Or(OpValue);

            // Assert
            f.Should().Be(o);
        }

        [Fact]
        public void Or_GivenNone_ShouldReturnSome()
        {
            // Arrange
            var o = None();

            // Act
            var f = o.Or(OpValue);

            // Assert
            f.Should().Be(Some(OpValue));
        }

        [Fact]
        public void Flatten_GivenSomeSome_ShouldReturnSome()
        {
            // Arrange
            var oo = Opt.Some(Some());

            // Act
            var o = oo.Flatten();

            // Assert
            o.Should().Be(Some());
        }

        [Fact]
        public void Flatten_GivenSomeNone_ShouldReturnNone()
        {
            // Arrange
            var oo = Opt.Some(None());

            // Act
            var o = oo.Flatten();

            // Assert
            o.Should().Be(None());
        }

        [Fact]
        public void MapOpt_GivenSome_ShouldReturnSomeMapped()
        {
            // Arrange
            var o = Some();

            // Act
            var r = o.Map(i => Opt.Some($"{i}{OpValue}"));

            // Assert
            r.Unwrap().Should().Be($"{DefSome}{OpValue}");
        }

        [Fact]
        public void MapOpt_GivenNone_ShouldReturnNone()
        {
            // Arrange
            var o = None();

            // Act
            var r = o.Map(i => Opt.Some($"{i}{OpValue}"));

            // Assert
            r.Should().Be(Opt.None<string>());
        }

        [Fact]
        public void Map_GivenSome_ShouldReturnSomeMapped()
        {
            // Arrange
            var o = Some();

            // Act
            var r = o.Map(i => $"{i}{OpValue}");

            // Assert
            r.Unwrap().Should().Be($"{DefSome}{OpValue}");
        }

        [Fact]
        public void Map_GivenNone_ShouldReturnNone()
        {
            // Arrange
            var o = None();

            // Act
            var r = o.Map(i => $"{i}{OpValue}");

            // Assert
            r.Should().Be(Opt.None<string>());
        }

        [Fact]
        public void MapOr_GivenSome_ShouldReturnReturnSomeMapped()
        {
            // Arrange
            const string Def = "def";
            var o = Some();

            // Act
            var r = o.MapOr(i => $"{i}{OpValue}", def: Def);

            // Assert
            r.Should().Be($"{DefSome}{OpValue}");
        }

        [Fact]
        public void MapOr_GivenNone_ShouldReturnSomeDefault()
        {
            // Arrange
            const string Def = "def";
            var o = None();

            // Act
            var r = o.MapOr(i => $"{i}{OpValue}", def: Def);

            // Assert
            r.Should().Be(Def);
        }

        [Fact]
        public void MapOrElse_GivenSome_ShouldReturnSomeMapped()
        {
            // Arrange
            const string Def = "def";
            var o = Some();

            // Act
            var r = o.MapOrElse(i => $"{i}{OpValue}", def: () => Def);

            // Assert
            r.Should().Be($"{DefSome}{OpValue}");
        }

        [Fact]
        public void MapOrElse_GivenNone_ShouldReturnSomeDefault()
        {
            // Arrange
            const string Def = "def";
            var o = None();

            // Act
            var r = o.MapOrElse(i => $"{i}{OpValue}", def: () => Def);

            // Assert
            r.Should().Be(Def);
        }

        [Fact]
        public void OkOr_GivenSome_ShouldReturnOkSome()
        {
            // Arrange
            var o = Some();

            // Act
            var r = o.OkOr(DefErr);

            // Assert
            r.Should().Be(Res.Ok(DefSome));
        }

        [Fact]
        public void OkOr_GivenNone_ShouldReturnErr()
        {
            // Arrange
            var o = None();

            // Act
            var r = o.OkOr(DefErr);

            // Assert
            r.Should().Be(Res<int>.Err(DefErr));
        }

        [Fact]
        public void OkOrElse_GivenSome_ShouldReturnOkSome()
        {
            // Arrange
            var o = Some();

            // Act
            var r = o.OkOrElse(() => DefErr);

            // Assert
            r.Should().Be(Res.Ok(DefSome));
        }

        [Fact]
        public void OkOrElse_GivenNone_ShouldReturnErr()
        {
            // Arrange
            var o = None();

            // Act
            var r = o.OkOrElse(() => DefErr);

            // Assert
            r.Should().Be(Res<int>.Err(DefErr));
        }

        [Fact]
        public void OrElseOpt_GivenSome_ShouldReturnSome()
        {
            // Arrange
            var o = Some();

            // Act
            var r = o.OrElse(() => Some(OpValue));

            // Assert
            r.Should().Be(Some());
        }

        [Fact]
        public void OrElseOpt_GivenNone_ShouldReturnSomeOp()
        {
            // Arrange
            var o = None();

            // Act
            var r = o.OrElse(() => Some(OpValue));

            // Assert
            r.Should().Be(Some(OpValue));
        }


        [Fact]
        public void OrElse_GivenSome_ShouldReturnSome()
        {
            // Arrange
            var o = Some();

            // Act
            var r = o.OrElse(() => OpValue);

            // Assert
            r.Should().Be(Some());
        }

        [Fact]
        public void OrElse_GivenNone_ShouldReturnSomeOp()
        {
            // Arrange
            var o = None();

            // Act
            var r = o.OrElse(() => OpValue);

            // Assert
            r.Should().Be(Some(OpValue));
        }

        public static TheoryData<Opt<int>, Opt<int>, Opt<int>> XorTestData { get; }
            = new TheoryData<Opt<int>, Opt<int>, Opt<int>>()
            {
                { None(), None(), None() },
                { None(), Some(), Some() },
                { Some(), None(), Some() },
                { Some(), Some(), None() },
            };

        [Theory]
        [MemberData(nameof(XorTestData))]
        public void Xor_ShouldReturnCorrectResults(Opt<int> x, Opt<int> y, Opt<int> expected)
        {
            // Act
            var r = x.Xor(y);

            // Assert
            r.Should().Be(expected);
        }

        [Fact]
        public void UnwrapNone_GivenSome_ShouldThrow()
        {
            // Arrange
            var o = Some();

            // Act
            Action a = () => o.UnwrapNone();

            // Assert
            a.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void UnwrapNone_GivenNone_ShouldNotThrow()
        {
            // Arrange
            var o = None();

            // Act
            Action a = () => o.UnwrapNone();

            // Assert
            a.Should().NotThrow();
        }

        [Fact]
        public void UnwrapOr_GivenNone_ShouldReturnOr()
        {
            // Arrange
            var o = None();

            // Act
            var r = o.UnwrapOr(OpValue);

            // Assert
            r.Should().Be(OpValue);
        }

        [Fact]
        public void UnwrapOr_GivenSome_ShouldReturnSome()
        {
            // Arrange
            var o = Some();

            // Act
            var r = o.UnwrapOr(OpValue);

            // Assert
            r.Should().Be(DefSome);
        }

        [Fact]
        public void UnwrapOrLazy_GivenNone_ShouldReturnOr()
        {
            // Arrange
            var o = None();

            // Act
            var r = o.UnwrapOrElse(() => OpValue);

            // Assert
            r.Should().Be(OpValue);
        }

        [Fact]
        public void UnwrapOrLazy_GivenSome_ShouldReturnSome()
        {
            // Arrange
            var o = Some();

            // Act
            var r = o.UnwrapOrElse(() => OpValue);

            // Assert
            r.Should().Be(DefSome);
        }


        public static TheoryData<Opt<Res<int>>, Res<Opt<int>>> TransposeData { get; } =
            new TheoryData<Opt<Res<int>>, Res<Opt<int>>>()
            {
                { Opt<Res<int>>.None, Res<Opt<int>>.Ok(None()) },
                { Opt.Some(Res.Ok(1)), Res.Ok(Opt.Some(1)) },
                { Opt.Some(Res<int>.Err(DefErr)), Res<Opt<int>>.Err(DefErr) }
            };

        [Theory]
        [MemberData(nameof(TransposeData))]
        public void Transpose_ShouldReturnCorrectResults(Opt<Res<int>> x, Res<Opt<int>> expected)
        {
            // Arrange

            // Act
            var r = x.Transpose();

            // Assert
            r.Should().Be(expected);
        }

        private static Opt<int> None()
            => Opt.None<int>();

        private static Opt<int> Some(int val = DefSome)
            => Opt.Some(val);
    }
}
