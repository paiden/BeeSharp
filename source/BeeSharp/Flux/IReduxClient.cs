namespace BeeSharp.Flux
{
    public interface IReduxClient<TState>
    {

        void MapState(TState state);
    }
}
