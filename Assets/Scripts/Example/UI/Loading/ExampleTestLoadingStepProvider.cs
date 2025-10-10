using System.Collections.Generic;
using Samurai.UnityFramework.Loading;

namespace Samurai.Example.UI.Loading
{
    public sealed class ExampleTestLoadingStepProvider : ILoadingStepProvider
    {
        private readonly List<LoadingStep> _steps = new()
        {
            new FakeLoadingStep(1f).WithWeight(2f).WithText("Doing some unnecessary stuff.."),
            new FakeLoadingStep(.3f).WithWeight(1f).WithText("Bet you didn't even have time to read this!"),
            new FakeLoadingStep(.6f).WithWeight(2f).WithText("Oh you will be here for a while.."),
            new FakeLoadingStep(1f).WithWeight(1f).WithText("And for no reason!"),
            new FakeLoadingStep(2f).WithWeight(0.5f).WithText("Hmm maybe I just shouldn't prolong the loading?"),
        };

        public IEnumerable<LoadingStep> Steps => _steps;
    }
}