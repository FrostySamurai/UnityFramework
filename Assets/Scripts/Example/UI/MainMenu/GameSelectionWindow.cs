using Mirzipan.Extensions.Unity.UI;
using Samurai.Example.UI.Loading;
using Samurai.UnityFramework;
using Samurai.UnityFramework.Defs;
using Samurai.UnityFramework.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            var sceneHandler = App.Get<SceneHandler>();

            Session.Create("Session");
            
            await sceneHandler.Load(config.LoadingScene);
            await sceneHandler.Unload(config.MainMenuScene);

            await Awaitable.MainThreadAsync();
            var scene = SceneManager.GetSceneByName(config.LoadingScene);
            foreach (var go in scene.GetRootGameObjects())
            {
                if (go.TryGetComponent<Loader>(out var loader))
                {
                    await loader.Load();
                    break;
                }
            }
            
            var sessionLoad = sceneHandler.Load(config.SessionScene);
            var loadingUnload = sceneHandler.Unload(config.LoadingScene);

            await sessionLoad;
            await loadingUnload;
        }
    }
}