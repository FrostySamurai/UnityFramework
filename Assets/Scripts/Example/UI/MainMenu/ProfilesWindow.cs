using Mirzipan.Extensions.Unity.UI;
using Samurai.Example.UI.MainMenu.Views;
using Samurai.UnityFramework;
using Samurai.UnityFramework.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Samurai.Example.UI.MainMenu
{
    public class ProfilesWindow : Window
    {
        [SerializeField] private ProfileCreationView _profileCreation;
        [SerializeField] private Button _profilesButton;
        [SerializeField] private Button _backButton;
        
        protected override void OnShow()
        {
            _profileCreation.Hide();
            
            _profilesButton.SetOnClickListener(() =>
            {
                _profileCreation.Init(LoadProfile);
            });
            
            // TODO: load existing profiles
            
            _backButton.SetOnClickListener(() => ShowWindow("MainMenu"));
        }

        private void LoadProfile(string id)
        {
            Profile.Load(id);
            ShowWindow("GameSelection");
        }
    }
}