using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Samurai.Example.UI.Loading;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TMP_Text;

namespace Samurai.UnityFramework.Loading
{
    public class Loader : MonoBehaviour
    {
        private const string LogTag = nameof(Loader);
        
        [SerializeField] private Slider _progressBar;
        [SerializeField] private Text _loadingText;
        
        [Header("Logic")] 
        [SerializeField] private bool _waitForPlayerInput;

        private bool _waitingForInput;
        private bool _continuePressed;
        
        private readonly List<LoadingStep> _steps = new();

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

        public void Add(IEnumerable<ILoadingStepProvider> providers)
        {
            if (providers is null)
            {
                return;
            }
            
            foreach (var provider in providers)
            {
                Add(provider);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(ILoadingStepProvider provider)
        {
            if (provider is null || provider.Steps is null)
            {
                return;
            }
            
            _steps.AddRange(provider.Steps);
        }

        public void Add(IEnumerable<LoadingStep> steps)
        {
            if (steps is null)
            {
                return;
            }
            
            _steps.AddRange(steps);
        }

        public void Add(LoadingStep step)
        {
            if (step is null)
            {
                return;
            }
            
            _steps.Add(step);
        }

        public async Awaitable Load()
        {
            Log.Debug($"Loading session.", LogTag);

            Add(GetComponents<ILoadingStepProvider>());
            _steps.Sort((lhs, rhs) => rhs.Priority.CompareTo(lhs.Priority));

            float totalWeight = _steps.Sum(x => x.Weight);
            _progressBar.maxValue = totalWeight;
            _progressBar.value = 0f;

            foreach (var step in _steps)
            {
                _loadingText.SetText(step.Text ?? "Loading..");
                await step.Process();
                _progressBar.value += step.Weight;
            }
            
            if (_waitForPlayerInput)
            {
                Log.Debug($"Waiting for player input.", LogTag);
                await WaitForPlayerInput();
            }

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