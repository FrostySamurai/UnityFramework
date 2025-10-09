using System;
using System.Reflection;
using Mirzipan.Extensions.Unity;
using UnityEngine;

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

        #region Component

        public static void SetActiveSafe(this Component @this, bool value)
        {
            if (@this is not null)
            {
                @this.SetActive(value);
            }
        }

        #endregion Component
    }
}