using System.Collections;
using Mirzipan.Extensions.Unity;
using Samurai.UnityFramework.UI.Transitions;
using UnityEngine;

namespace Samurai.UnityFramework.UI
{
    public abstract class Window : MonoBehaviour
    {
        protected WindowManager Manager;

        internal void Inject(WindowManager manager)
        {
            Manager = manager;
        }

        public IEnumerator Show(bool instant = false)
        {
            OnShow();
            this.SetActive(true);
            if (TryGetComponent<UiTransition>(out var transition))
            {
                yield return transition.Show(instant);
            }
        }

        public IEnumerator Hide(bool instant = false)
        {
            if (TryGetComponent<UiTransition>(out var transition))
            {
                yield return transition.Hide(instant);
            }
            this.SetActive(false);
            OnHide();
        }

        public void ForceHide()
        {
            this.SetActive(false);
            OnHide();
        }

        protected abstract void OnShow();
        protected virtual void OnHide() {}

        protected void ShowWindow<T>() where T : Window
        {
            Manager.Show<T>();
        }
        
        protected void ShowWindow(string id)
        {
            Manager.Show(id);
        }
    }
}