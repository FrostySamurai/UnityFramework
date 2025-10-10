using Mirzipan.Extensions.Unity.UI;
using Samurai.Example.UI.Loading;
using Samurai.UnityFramework;
using Samurai.UnityFramework.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Samurai.Example.UI.MainMenu
{
    [Window(MainMenuWindows.GameSelection)]
    public class GameSelectionWindow : Window
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _backButton;
        
        protected override void OnShow()
        {
            // TODO: passing providers through start session is not very nice, figure out a better way of accessing the loader before loading starts
            _newGameButton.SetOnClickListener(() => App.StartSession("Session", new ExampleTestLoadingStepProvider()));
            _backButton.SetOnClickListener(() =>
            {
                Profile.Unload(true);
                ShowWindow<ProfilesWindow>();
            });
        }
    }
}