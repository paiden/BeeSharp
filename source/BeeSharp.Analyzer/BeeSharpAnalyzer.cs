using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace BeeSharp.Analyzer
{
    public abstract class BeeSharpAnalyzer : DiagnosticAnalyzer
    {
        protected static LocalizableString LocS(string name)
            => new LocalizableResourceString(name, Resources.ResourceManager, typeof(Resources));
    }
}
