using BeeSharp.Threading;
using FluentAssertions;
using Xunit;

namespace BeeSharp.Tests.Threading
{
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
