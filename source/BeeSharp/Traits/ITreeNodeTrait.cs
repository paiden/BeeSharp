using System;
using System.Collections.Generic;
using System.Linq;

namespace BeeSharp.Traits
{
    public interface ITreeNodeTrait<out T>
        where T : ITreeNodeTrait<T>
    {
        IEnumerable<T> GetChildren();

        IEnumerable<IEnumerable<T>> EnumerateBreadthFirst()
        {
            var q = new Queue<(int, T)>(new[] { (depth: 0, node: (T)this) });
            var result = new List<(int, T)>();

            while (q.Count > 0)
            {
                var current = q.Dequeue();
                result.Add((current.Item1, (T)current.Item2));

                foreach (var c in current.Item2.GetChildren())
                {
                    q.Enqueue((current.Item1 + 1, c));
                }
            }

            return result.GroupBy(i => i.Item1, i => i.Item2);
        }

        IEnumerable<T> EnumerateDepthFirstPre()
        {
            yield return (T)this;

            foreach (var c in this.GetChildren())
            {
                foreach (var sn in c.EnumerateDepthFirstPre())
                {
                    yield return sn;
                }
            }
        }

        IEnumerable<T> EnumerateDepthFirstPost()
        {
            foreach (var c in this.GetChildren())
            {
                foreach (var sn in c.EnumerateDepthFirstPost())
                {
                    yield return sn;
                }
            }

            yield return (T)this;
        }

        TNew Map<TNew>(Func<T, TNew> map)
            where TNew : ITreeNodeBuildTrait<TNew>
        {
            var newNode = map((T)this);
            var childNodes = this.GetChildren()
                .Select(c => c.Map(map));
            newNode.SetChildren(childNodes);
            return newNode;
        }
    }

    public static class TreeNodeTraitExtensions
    {
        public static ITreeNodeTrait<T> TreeTrait<T>(this T supportsTrait)
            where T : ITreeNodeTrait<T>
            => supportsTrait;

        public static ITreeNodeBuildTrait<T> Builder<T>(this T supportsTrait)
            where T : ITreeNodeBuildTrait<T>
            => supportsTrait;
    }
}
