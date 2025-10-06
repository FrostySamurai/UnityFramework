using DG.Tweening;
using UnityEngine;

namespace Samurai.Example.UI.Animations
{
    public class FadeOutAnimation : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private bool _playOnAwake;
        [SerializeField] private bool _fadeOut;
        [SerializeField] private float _duration;
        [SerializeField] private Ease _ease;
        [SerializeField] private bool _destroyAfter = true;
        [SerializeField] private bool _useUnscaledTime = true;
        
        private void Awake()
        {
            if (!_playOnAwake)
            {
                return;
            }
            
            Play();
        }

        public async void Play()
        {
            _canvasGroup.alpha = 1f;
            var tween = _canvasGroup.DOFade(0f, _duration).SetEase(_ease).SetUpdate(_useUnscaledTime);
            await tween.AsyncWaitForCompletion();
            if (_destroyAfter)
            {
                await Awaitable.MainThreadAsync();
                Destroy(gameObject);
            }
        }
    }
}