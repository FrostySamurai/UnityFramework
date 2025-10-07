using System;
using System.Reflection;

namespace Samurai.UnityFramework
{
    public static class FrameworkExtensions
    {
        #region Type

        public static bool TryGetCustomAttribute<T>(this object entry, out T attribute) where T : Attribute
        {
            attribute = entry.GetType().GetCustomAttribute<T>();
            return attribute is not null;
        }

        public static bool TryGetCustomAttribute<T>(this Type @this, out T attribute) where T : Attribute
        {
            attribute = @this.GetCustomAttribute<T>();
            return attribute is not null;
        }

        public static bool HasAttribute<T>(this object @this) where T : Attribute
        {
            return Attribute.IsDefined(@this.GetType(), typeof(T));
        }

        public static bool HasAttribute<T>(this Type @this) where T : Attribute
        {
            return @this.IsDefined(typeof(T));
        }

        #endregion Type
    }
}