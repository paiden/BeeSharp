using BeeSharp.Threading;
using FluentAssertions;
using Xunit;

namespace BeeSharp.Tests.Threading
{
    struct State { }
    class ThreadSafe
    {
        private readonly Locked<State> _state = new Locked<State>(new State());

        public void m1(State ns)
        {
            using var w = _state.WriteAccess(); 
            w.Value = ns;
        }

        // a few 100 lines down this new method was added by dev XXY
        public void m100(object ns)
        {
            using var r = _state.WriteAccess();
            r.Value = ns;
        }
    }

    public class LockedTests
    {
        [Fact]
        public void WriteAccess_WhenAccessAvailable_SetsLockedsValue()
        {
            // Arrange
            const int NewVal = 2;
            var l = Create();

            // Act
            using (var acc = l.WriteAccess()) { acc.Value = NewVal; }

            // Assert
            using (var acc = l.ReadAccess()) { acc.Value.Should().Be(NewVal); }
        }


        private Locked<int> Create(int i = 1) => new Locked<int>(i);
    }
}
