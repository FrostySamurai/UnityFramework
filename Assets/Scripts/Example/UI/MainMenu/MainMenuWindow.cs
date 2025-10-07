using Mirzipan.Extensions.Unity.UI;
using Samurai.UnityFramework;
using Samurai.UnityFramework.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Samurai.Example.UI.MainMenu
{
    [Window(MainMenuWindows.MainMenu)]
    public class MainMenuWindow : Window
    {
        [SerializeField] private Button _profilesButton;
        [SerializeField] private Button _exitButton;
        
        protected override void OnShow()
        {
            _profilesButton.SetOnClickListener(ShowWindow<ProfilesWindow>);
            _exitButton.SetOnClickListener(App.Quit);
        }
    }
}