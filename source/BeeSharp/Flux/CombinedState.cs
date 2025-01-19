using System;
using System.Collections.Generic;
using System.Text;

namespace BeeSharp.Flux
{
    public abstract class CombinedStateBase<T> : IReduxClient<T>
        where T : class
    {
        protected T? stateA;

        void IReduxClient<T>.MapState(T state)
        {
            this.stateA = state;
            this.Combine();
        }

        protected abstract void Combine();
    }

    public sealed class CombinedState<InputStateA, InputStateB>: CombinedStateBase<InputStateA>, IReduxClient<InputStateB>
        where InputStateA: class 
        where InputStateB: class
    {
        private readonly Action<InputStateA, InputStateB> combineFunction;
        private InputStateB? stateB;

        public CombinedState(Action<InputStateA, InputStateB> combineFunction)
        {
            this.combineFunction = combineFunction;
        }

        void IReduxClient<InputStateB>.MapState(InputStateB state)
        {
            this.stateB = state;
            this.Combine();
        }

        protected override void Combine()
        {
            var sa = this.stateA;
            var sb = this.stateB;

            if(sa != null && sb != null)
            {
                this.combineFunction(sa, sb);
            }
        }
    }
}
