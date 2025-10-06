using System;
using Samurai.UnityFramework;

namespace Samurai.Example
{
    [Serializable]
    public class TestModel : ISavable
    {
        public string Text;

        public TestModel(string text)
        {
            Text = text;
        }
    }
}