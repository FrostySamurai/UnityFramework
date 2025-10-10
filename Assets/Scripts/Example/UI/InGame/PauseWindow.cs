using Mirzipan.Extensions.Unity.UI;
using Samurai.UnityFramework;
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

        private void GoToMainMenu()
        {
            App.Resume();
            App.EndSession();
        }
    }
}