using System.Collections.Generic;
using Samurai.UnityFramework.Loading;
using UnityEngine;

namespace Samurai.Example.UI.Loading
{
    public class ExampleComponentLoadingStepProvider : MonoBehaviour, ILoadingStepProvider
    {
        private List<LoadingStep> _steps = new()
        {
            new FakeLoadingStep(2f).WithText("Priority example! From mono behaviour..").WithWeight(.8f).WithPriority(10)
        };

        public IEnumerable<LoadingStep> Steps => _steps;
    }
}