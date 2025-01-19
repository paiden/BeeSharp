using System;
using System.Threading.Tasks;

namespace BeeSharp.Flux
{
    public interface IAction
    {
    }

    public interface IAsyncAction
    {
    }

    public static class IActionExtensions
    {
        public static Task<TAppSTate> CannotReduce<TAppSTate>(this IAsyncAction action, string reducerName)
            => throw new InvalidOperationException($"Reducer '{reducerName}' cannot handle action '{action.GetType().Name}'.");

        public static TAppState CannotReduce<TAppState>(this IAction action, string reducerName)
            => throw new InvalidOperationException($"Reducer '{reducerName}' cannot handle action '{action.GetType().Name}'.");
    }
}
