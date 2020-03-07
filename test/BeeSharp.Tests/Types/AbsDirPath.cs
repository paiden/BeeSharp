using System;

namespace BeeSharp.Tests.Types
{
    public struct AbsDirPath : IEquatable<AbsDirPath>, IComparable<AbsDirPath>, IPreventDefaultConstruction
    {
        public AbsDirPath()
        {

        }
    }
}
