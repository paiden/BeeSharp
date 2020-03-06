using System;

namespace BeeSharp.Validation
{
    public static class ObjectValidationExtensions
    {
        public static T CheckNotNull<T>(this T obj, string varName)
            => obj ?? throw new ArgumentNullException(varName);
    }
}
