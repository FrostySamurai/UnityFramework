using System.Collections.Generic;

namespace Samurai.UnityFramework.Loading
{
    public interface ILoadingStepProvider
    {
        IEnumerable<LoadingStep> Steps { get; }
    }
}