using Mirzipan.Extensions.Unity;
using UnityEngine;

namespace Samurai.UnityFramework.UI
{
    public abstract class Window : MonoBehaviour
    {
        [SerializeField] private string _id;

        public string Id => _id;

        protected WindowManager Manager;

        internal void Inject(WindowManager manager)
        {
            Manager = manager;
        }
        
        public async Awaitable Show(bool instant = false)
        {
            OnShow();
            this.SetActive(true);
            if (TryGetComponent<WindowTransition>(out var transition))
            {
                await transition.Show(instant);
            }
        }

        public async Awaitable Hide(bool instant = false)
        {
            if (TryGetComponent<WindowTransition>(out var transition))
            {
                await transition.Hide(instant);
            }
            this.SetActive(false);
            OnHide();
        }

        protected abstract void OnShow();
        protected virtual void OnHide() {}

        protected void ShowWindow(string id)
        {
            Manager.Show(id);
        }
    }
}