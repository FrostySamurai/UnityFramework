using System;

namespace Samurai.UnityFramework.UI
{
    [AttributeUsage(AttributeTargets.Class)]
    public class WindowAttribute : Attribute
    {
        public string Id;

        public WindowAttribute(string id)
        {
            Id = id;
        }
    }
}