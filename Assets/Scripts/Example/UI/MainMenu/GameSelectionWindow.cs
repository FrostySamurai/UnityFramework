using Mirzipan.Extensions.Unity.UI;
using Samurai.Example.UI.Loading;
using Samurai.UnityFramework;
using Samurai.UnityFramework.Defs;
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
            _newGameButton.SetOnClickListener(LoadSession);
            _backButton.SetOnClickListener(() =>
            {
                Profile.Unload(true);
                ShowWindow<ProfilesWindow>();
            });
        }

        private async void LoadSession()
        {
            var config = Definitions.Config<AppConfig>();

            Session.Create("Session");
            
            await Scenes.Load(config.LoadingScene);
            await Scenes.Unload(config.MainMenuScene);

            await Awaitable.MainThreadAsync();
            if (Scenes.TryGetReference<Loader>(config.LoadingScene, out var loader))
            {
                await loader.Load();
            }
            
            var sessionLoad = Scenes.Load(config.SessionScene);
            var loadingUnload = Scenes.Unload(config.LoadingScene);

            await sessionLoad;
            await loadingUnload;
        }
    }
}