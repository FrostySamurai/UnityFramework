using System;
using Samurai.UnityFramework;

namespace Samurai.Example
{
    [Serializable, Savable]
    public class TestModel
    {
        public string Text;

        public TestModel(string text)
        {
            Text = text;
        }
    }
}