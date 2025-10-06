using DG.Tweening;
using UnityEngine;

namespace Samurai.UnityFramework.UI.Transitions
{
    public sealed class FadeWindowTransition : WindowTransition
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        
        [SerializeField] private float _fadeInDuration;
        [SerializeField] private Ease _fadeInEase;
        
        [SerializeField] private float _fadeOutDuration;
        [SerializeField] private Ease _fadeOutEase;
        [SerializeField] private bool _useUnscaledTime = true;
        
        
        public override async Awaitable Show(bool instant)
        {
            if (instant)
            {
                _canvasGroup.alpha = 1f;
                return;
            }

            _canvasGroup.alpha = 0f;
            var tween = _canvasGroup.DOFade(1f, _fadeInDuration).SetEase(_fadeInEase).SetUpdate(_useUnscaledTime);
            await tween.AsyncWaitForCompletion();
        }

        public override async Awaitable Hide(bool instant)
        {
            if (instant)
            {
                _canvasGroup.alpha = 0f;
                return;
            }

            var tween = _canvasGroup.DOFade(0f, _fadeOutDuration).SetEase(_fadeOutEase).SetUpdate(_useUnscaledTime);
            await tween.AsyncWaitForCompletion();
        }
    }
}