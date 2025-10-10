using UnityEngine;

namespace Samurai.UnityFramework.Loading
{
    public abstract class LoadingStep
    {
        public int Priority { get; private set; }
        public float Weight { get; private set; } = 1f;
        public string Text { get; private set; }

        public LoadingStep WithPriority(int priority)
        {
            Priority = priority;
            return this;
        }
        
        public LoadingStep WithWeight(float weight)
        {
            Weight = weight;
            return this;
        }

        public LoadingStep WithText(string text)
        {
            Text = text;
            return this;
        }
        
        public abstract Awaitable Process();
    }
}