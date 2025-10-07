using System;
using System.Reflection;

namespace Samurai.UnityFramework
{
    public static class FrameworkExtensions
    {
        #region Type

        public static bool TryGetCustomAttribute<T>(this Type @this, out T attribute) where T : Attribute
        {
            attribute = @this.GetCustomAttribute<T>();
            return attribute != null;
        }

        #endregion Type
    }
}