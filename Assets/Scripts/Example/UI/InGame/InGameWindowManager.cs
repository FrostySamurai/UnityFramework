using Samurai.UnityFramework;
using Samurai.UnityFramework.UI;
using UnityEngine;

namespace Samurai.Example.UI.InGame
{
    public class InGameWindowManager : WindowManager
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Current is PauseWindow)
                {
                    HideCurrent();
                    App.Resume();
                }
                else
                {
                    App.Pause();
                    Show<PauseWindow>();
                }
            }
        }
    }
}