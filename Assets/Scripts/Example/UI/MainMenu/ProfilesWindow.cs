using Mirzipan.Extensions.Unity.UI;
using Samurai.UnityFramework;
using Samurai.UnityFramework.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Samurai.Example.UI.MainMenu
{
    public class ProfilesWindow : Window
    {
        [SerializeField] private Button _profilesButton;
        [SerializeField] private Button _backButton;
        
        protected override void OnShow()
        {
            _profilesButton.SetOnClickListener(() =>
            {
                Profile.Load("profile1");
                ShowWindow("GameSelection");
            });
            _backButton.SetOnClickListener(() => ShowWindow("MainMenu"));
        }
    }
}