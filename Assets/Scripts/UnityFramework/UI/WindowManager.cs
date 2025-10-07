using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Samurai.UnityFramework.UI
{
    public class WindowManager : MonoBehaviour
    {
        public const string LogTag = "Windows";
        
        [SerializeField] private Window _defaultWindow;
        [SerializeField] private List<Window> _windows;

        private readonly Dictionary<string, Window> _windowsById = new();
        private readonly Dictionary<Type, Window> _windowsByType = new();
        
        protected Window Current;
        
        private async void Awake()
        {
            foreach (var window in _windows)
            {
                window.Inject(this);
                
                var type = window.GetType();
                _windowsByType[type] = window;

                if (!type.TryGetCustomAttribute<WindowAttribute>(out var windowAttribute))
                {
                    Log.Warning($"Window {type.Name} is missing window attribute! Manager will not find the window by id.", LogTag);
                    continue;
                }
                
                _windowsById[windowAttribute.Id] = window;
            }
            
            if (_defaultWindow is not null)
            {
                await Show(_defaultWindow, false);
            }
        }

        private void OnValidate()
        {
            _windows = GetComponentsInChildren<Window>(true).ToList();
        }

        public async void Show<T>(bool instant = false) where T : Window
        {
            if (_windowsByType.TryGetValue(typeof(T), out var window))
            {
                await Show(window, instant);
            }
        }

        public async void Show(string id, bool instant = false)
        {
            if (_windowsById.TryGetValue(id, out var window))
            {
                await Show(window, instant);
            }
        }

        private async Awaitable Show(Window window, bool instant)
        {
            if (window is null)
            {
                return;
            }

            if (Current is not null)
            {
                await Current.Hide(instant);
            }

            Current = window;
            await Current.Show(instant);
        }

        public async void HideCurrent()
        {
            if (Current is null)
            {
                return;
            }
            
            await Current.Hide();
            Current = null;
        }
    }
}