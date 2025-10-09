using System.Collections;
using UnityEngine;

namespace Samurai.UnityFramework.UI.Transitions
{
    public abstract class UiTransition : MonoBehaviour
    {
        public abstract IEnumerator Show(bool instant);
        public abstract IEnumerator Hide(bool instant);
    }
}