using UnityEngine;

namespace Samurai.Example.UI.Loading
{
    public abstract class LoadingStep
    {
        public float Weight { get; private set; } = 1f;
        public string Text { get; private set; }
        
        public LoadingStep SetWeight(float weight)
        {
            Weight = weight;
            return this;
        }

        public LoadingStep SetText(string text)
        {
            Text = text;
            return this;
        }
        
        public abstract Awaitable Process();
    }
}