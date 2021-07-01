using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BeeSharp.Flux
{
    public class FluxStore<TState> : IFluxStore<TState>
    {
        private readonly AsyncLock mutex = new();

        private readonly List<WeakReference<IReduxClient<TState>>> clients = new();
        private readonly Func<IAction, TState, TState> reducer;
        private readonly Func<IAsyncAction, TState, Task<TState>> asyncReducer;

        private TState state;

        public FluxStore(
            Func<IAction, TState, TState> reducer,
            Func<IAsyncAction, TState, Task<TState>> asyncReducer,
            TState initialState)
        {
            this.reducer = reducer;
            this.asyncReducer = asyncReducer;
            this.state = initialState;
        }

        public void Dispatch(IAction action)
        {
            TState newState = default!;
            using (this.mutex.Lock())
            {
                newState = this.reducer(action, this.state);
                this.state = newState;
            }

            this.OnStateChanged(newState);
        }

        public async Task DispatchAsync(IAsyncAction action)
        {
            TState newState = default!;
            using (this.mutex.Lock())
            {
                newState = await this.asyncReducer(action, this.state);
                this.state = newState;
            }

            this.OnStateChanged(newState);
        }

        public void Connect(IReduxClient<TState> client)
        {
            this.clients.Add(new WeakReference<IReduxClient<TState>>(client));
            client.MapState(this.state);
        }

        public TRet Get<TRet>(Func<TState, TRet> binding)
        {
            return binding(this.state);
        }

        private void OnStateChanged(TState newState)
        {
            foreach (var clientRef in this.clients)
            {
                if (clientRef.TryGetTarget(out var client))
                {
                    client.MapState(newState);
                }
            }
        }

        public static Func<TState, TState> CombinedReducer(params Func<IAction, TState, TState>[] subReducers)
        {
            throw new NotImplementedException();
        }

        public Action<TAction> BindAction<TAction>(IAction _ = default!) where TAction : IAction
            => (action) => this.Dispatch(action);

        public Func<TAction, Task> BindAction<TAction>(IAsyncAction _ = default!) where TAction : IAsyncAction
            => (action) => this.DispatchAsync(action);

        private class AsyncLock : IDisposable
        {
            private readonly SemaphoreSlim semaphoreSlim = new(1, 1);

            public AsyncLock Lock()
            {
                this.semaphoreSlim.Wait();
                return this;
            }

            public async Task<AsyncLock> LockAsync()
            {
                await this.semaphoreSlim.WaitAsync();
                return this;
            }

            public void Dispose()
            {
                this.semaphoreSlim.Release();
            }
        }
    }
}
