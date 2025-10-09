using System.Collections.Generic;
using System.Linq;
using Samurai.UnityFramework;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TMP_Text;

namespace Samurai.Example.UI.Loading
{
    public class Loader : MonoBehaviour
    {
        private const string LogTag = nameof(Loader);
        
        [SerializeField] private Slider _progressBar;
        [SerializeField] private Text _loadingText;

        private bool _waitingForInput;
        private bool _continuePressed;

        private void Awake()
        {
            Scenes.SetReference(this);
        }

        private void Update()
        {
            if (_waitingForInput && Input.anyKeyDown)
            {
                _continuePressed = true;
            }
        }

        public async Awaitable Load()
        {
            // do loading, this is just a demonstration
            
            Log.Debug($"Loading session.", LogTag);

            var steps = new List<LoadingStep>()
            {
                new FakeLoadingStep(1f).SetWeight(2f).SetText("Doing some unnecessary stuff.."),
                new FakeLoadingStep(.3f).SetWeight(1f).SetText("Bet you didn't even have time to read this!"),
                new FakeLoadingStep(.6f).SetWeight(2f).SetText("Oh you will be here for a while.."),
                new FakeLoadingStep(1f).SetWeight(1f).SetText("And for no reason!"),
                new FakeLoadingStep(2f).SetWeight(0.5f).SetText("Hmm maybe I just shouldn't prolong the loading?"),
            };

            float totalWeight = steps.Sum(x => x.Weight);
            _progressBar.maxValue = totalWeight;
            _progressBar.value = 0f;

            foreach (var step in steps)
            {
                _loadingText.SetText(step.Text ?? "Loading..");
                await step.Process();
                _progressBar.value += step.Weight;
            }
            
            Log.Debug($"Waiting for player input.", LogTag);

            await WaitForPlayerInput();
            
            Log.Debug($"Loading finished.", LogTag);
        }

        private async Awaitable WaitForPlayerInput()
        {
            _waitingForInput = true;
            _loadingText.SetText("Press any key to continue..");
            while (!_continuePressed)
            {
                await Awaitable.NextFrameAsync();
            }
        }
    }
}