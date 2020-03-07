#if BEES_UNIX

Not supported yet. Just planning out on how to support multi platform scenarios

using System;
using System.Collections.Generic;
using System.Text;

namespace BeeSharp.Tests.Types
{
    public partial struct class AbsPathRoot
    {
        private const string DirSep = @"\";

        private readonly CIString value;

    }
}

#endif