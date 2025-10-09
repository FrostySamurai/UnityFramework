using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Samurai.UnityFramework.UI
{
    public class WindowManager : MonoBehaviour
    {
        internal const string LogTag = "Windows";
        
        [SerializeField] private Window _defaultWindow;
        [SerializeField] private List<Window> _windows;

        private readonly Dictionary<string, Window> _windowsById = new();
        private readonly Dictionary<Type, Window> _windowsByType = new();

        private Coroutine _currentTransition;
        
        protected Window Current;
        
        private void Awake()
        {
            Scenes.SetReference(this);
            
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
                Show(_defaultWindow, false, false);
            }
        }

        private void OnValidate()
        {
            _windows = GetComponentsInChildren<Window>(true).ToList();
        }

        public void Show<T>(bool instant = false, bool force = false)
        {
            if (_windowsByType.TryGetValue(typeof(T), out var window))
            {
                Show(window, instant, force);
            }
        }

        public void Show(string id, bool instant = false, bool force = false)
        {
            if (_windowsById.TryGetValue(id, out var window))
            {
                Show(window, instant, force);
            }
        }
        
        // TODO ensure two coroutines can't run at the same time
        // TODO 1: don't allow multiple at once
        // TODO 2: cancel the running transition and start the new one
        // TODO 3: queue up new transitions and wait for current to finish
        
        // TODO: doing the force stuff like this is maybe not the nicest of solutions at all
        private void Show(Window window, bool instant, bool force)
        {
            if (force)
            {
                if (_currentTransition is not null)
                {
                    StopCoroutine(_currentTransition);
                }
                
                if (Current) 
                    Current.ForceHide();
            }
            
            _currentTransition = StartCoroutine(ShowCoroutine(window, instant));
        }

        public void HideCurrent(bool instant = false)
        {
            _currentTransition = StartCoroutine(HideCurrentCoroutine(instant));
        }

        private IEnumerator ShowCoroutine(Window window, bool instant)
        {
            if (window is null)
            {
                yield break;
            }

            if (Current is not null)
            {
                yield return Current.Hide(instant);
            }

            Current = window;
            yield return Current.Show(instant);

            _currentTransition = null;
        }

        private IEnumerator HideCurrentCoroutine(bool instant)
        {
            if (Current is not null)
            {
                yield return Current.Hide(instant);
                Current = null;
            }

            _currentTransition = null;
        }
    }
}