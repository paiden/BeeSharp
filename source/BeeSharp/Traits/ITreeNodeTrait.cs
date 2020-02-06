
using System;
using System.Collections.Generic;

namespace BeeSharp.Traits
{
    public interface ITreeNodeTrait<out T>
    {
        IEnumerable<T> GetChildren();

        IEnumerable<T> TraverseDepthFirst()
        {
            throw new NotImplementedException();
        }
    }

    public static class ITreeNodeTraitExtensions
    {
        public static ITreeNodeTrait<T> Tree<T>(this T supportsTrait)
            where T : ITreeNodeTrait<T>
            => supportsTrait as ITreeNodeTrait<T>;
    }
}
