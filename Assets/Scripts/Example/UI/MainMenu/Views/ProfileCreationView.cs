using System;
using DG.Tweening;
using Mirzipan.Extensions.Collections;
using Mirzipan.Extensions.Unity;
using Mirzipan.Extensions.Unity.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Samurai.Example.UI.MainMenu.Views
{
    public class ProfileCreationView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _input;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private Button _createButton;
        
        public void Init(Action<string> onCreateConfirmed)
        {
            _cancelButton.SetOnClickListener(Hide);
            _createButton.SetOnClickListener(() => CreateProfile(onCreateConfirmed));
            
            this.SetActive(true);
            GetComponent<CanvasGroup>().DOFade(1f, 0.3f).SetEase(Ease.InCubic);
        }

        public void Hide()
        {
            this.SetActive(false);
        }

        private void CreateProfile(Action<string> onCreateConfirmed)
        {
            if (_input.text.IsNullOrEmpty())
            {
                return;
            }

            Hide();
            onCreateConfirmed?.Invoke(_input.text);
        }
    }
}