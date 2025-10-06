using UnityEngine;

namespace Samurai.Example.UI.Loading
{
    public sealed class FakeLoadingStep : LoadingStep
    {
        private readonly float _waitDuration;
        
        public FakeLoadingStep(float waitDuration)
        {
            _waitDuration = waitDuration;
        }
        
        public override async Awaitable Process()
        {
            await Awaitable.WaitForSecondsAsync(_waitDuration);
        }
    }
}