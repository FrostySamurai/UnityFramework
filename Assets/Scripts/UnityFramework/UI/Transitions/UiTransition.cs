using UnityEngine;

namespace Samurai.UnityFramework.UI
{
    public abstract class UiTransition : MonoBehaviour
    {
        public abstract Awaitable Show(bool instant);
        public abstract Awaitable Hide(bool instant);
    }
}