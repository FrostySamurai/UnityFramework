using Mirzipan.Extensions.Unity.UI;
using Samurai.Example.UI.MainMenu.Views;
using Samurai.UnityFramework;
using Samurai.UnityFramework.UI;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Text = TMPro.TMP_Text;

namespace Samurai.Example.UI.MainMenu
{
    [Window(MainMenuWindows.Profiles)]
    public class ProfilesWindow : Window
    {
        [SerializeField] private ProfileCreationView _profileCreation;
        [SerializeField] private Button _profilesButton;
        [SerializeField] private Button _backButton;

        [Header("Profiles")]
        [SerializeField] private Button _profilePrefab;
        [SerializeField] private RectTransform _profilesParent;
        
        protected override void OnShow()
        {
            App.Pool.ReturnChildren(_profilePrefab, _profilesParent);

            using var obj = ListPool<string>.Get(out var profileIds);
            Profile.GetExistingProfiles(profileIds);
            foreach (string profileId in profileIds)
            {
                var instance = App.Pool.Retrieve(_profilePrefab, _profilesParent);
                instance.GetComponentInChildren<Text>().SetText(profileId);

                string id = profileId;
                instance.SetOnClickListener(() => LoadProfile(id));
            }
            
            _profileCreation.Hide();
            _profilesButton.SetOnClickListener(() =>
            {
                _profileCreation.Init(LoadProfile);
            });
            
            _backButton.SetOnClickListener(ShowWindow<MainMenuWindow>);
        }

        private void LoadProfile(string id)
        {
            Profile.Load(id);
            ShowWindow<GameSelectionWindow>();
        }
    }
}