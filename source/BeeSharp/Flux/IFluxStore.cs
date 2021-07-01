using System;
using System.Threading.Tasks;

namespace BeeSharp.Flux
{
    public interface IFluxStore<TState>
    {
        void Connect(IReduxClient<TState> client);

        Action<TAction> BindAction<TAction>(IAction _ = default!) where TAction : IAction;
        Func<TAction, Task> BindAction<TAction>(IAsyncAction _ = default!) where TAction : IAsyncAction;
    }
}
