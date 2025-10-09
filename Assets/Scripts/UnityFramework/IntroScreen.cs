using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Samurai.UnityFramework
{
    public class IntroScreen : MonoBehaviour
    {
        [SerializeField] private List<CanvasGroup> _canvasGroups = new();

        private void Awake()
        {
            SceneHandler.SetReference(this);
        }

        public async Awaitable Show()
        {
            foreach (var entry in _canvasGroups)
            {
                var tween = entry.DOFade(1f, 2f)
                    .SetEase(Ease.InCubic);
                await tween.AsyncWaitForCompletion();

                tween = entry.DOFade(0f, 2f).SetEase(Ease.OutCubic);
                await tween.AsyncWaitForCompletion();
            }
        }
    }
}