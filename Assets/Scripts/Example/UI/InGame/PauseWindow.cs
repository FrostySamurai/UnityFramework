using Mirzipan.Extensions.Unity.UI;
using Samurai.Example.UI.MainMenu;
using Samurai.UnityFramework;
using Samurai.UnityFramework.Defs;
using Samurai.UnityFramework.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Samurai.Example.UI.InGame
{
    [Window(InGameWindows.Pause)]
    public class PauseWindow : Window
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _mainMenuButton;
        
        protected override void OnShow()
        {
            _continueButton.SetOnClickListener(() =>
            {
                Manager.HideCurrent();
                App.Resume();
            });
            
            _mainMenuButton.SetOnClickListener(GoToMainMenu);
        }

        private async void GoToMainMenu()
        {
            // TODO: save session
            
            App.Resume();

            var config = Definitions.Config<AppConfig>();
            var sceneHandler = Session.Get<SceneHandler>();

            await sceneHandler.Load(config.MainMenuScene);
            await sceneHandler.Unload(config.SessionScene);

            if (SceneHandler.TryGetReference<WindowManager>(config.MainMenuScene, out var windowManager))
            {
                windowManager.Show<GameSelectionWindow>(force: true);
            }
            
            Session.Dispose();
        }
    }
}