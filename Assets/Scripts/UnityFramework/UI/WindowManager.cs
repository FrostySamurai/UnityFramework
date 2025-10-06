using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Samurai.UnityFramework.UI
{
    public class WindowManager : MonoBehaviour
    {
        [SerializeField] private Window _defaultWindow;
        [SerializeField] private List<Window> _windows;

        protected Window Current;
        
        private void Awake()
        {
            _windows.ForEach(x => x.Inject(this));
            if (_defaultWindow is not null)
            {
                Show(_defaultWindow.Id);
            }
        }

        private void OnValidate()
        {
            _windows = GetComponentsInChildren<Window>(true).ToList();
        }

        public async void Show(string id, bool instant = false)
        {
            var target = _windows.Find(w => w.Id == id);
            if (target is null)
            {
                return;
            }

            if (Current is not null)
            {
                await Current.Hide(instant);
            }

            Current = target;
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