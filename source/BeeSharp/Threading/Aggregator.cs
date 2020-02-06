using System;
using System.Threading;
using System.Threading.Tasks;

namespace BeeSharp.Threading
{
    /// <summary>
    /// Aggregates all invocations of some <see cref="Action"/> that occur within some
    /// specified interval and will only execute the last one.
    /// </summary>
    /// <remarks>
    /// Don't use this class for critical business logic that relies on something being executed
    /// once. This is more of a circuit breaker. E.g. some event fires 300 times in 1 second, but
    /// I only care that it was raised at all and execute some expensive logic once if so. But I
    /// don't care much (besides performance) if the logic runs twice.
    /// </remarks>
    public sealed class Aggregator
    {
        private readonly TimeSpan aggregateInterval;

        private CancellationTokenSource cancel = new CancellationTokenSource();

        public Aggregator(TimeSpan aggregateInterval)
        {
            this.aggregateInterval = aggregateInterval;
        }

        public void Aggregate(Action toRun)
        {
            this.cancel.Cancel();
            this.cancel = new CancellationTokenSource();

            Task.Delay(this.aggregateInterval, cancel.Token)
                .ContinueWith(_ => toRun(), TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
