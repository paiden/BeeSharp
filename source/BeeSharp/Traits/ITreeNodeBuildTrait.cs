using System.Collections.Generic;

namespace BeeSharp.Traits
{
    public interface ITreeNodeBuildTrait<T>
        where T : ITreeNodeBuildTrait<T>
    {
        void SetChildren(IEnumerable<T> children);
    }
}
