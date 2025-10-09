using Samurai.UnityFramework.Defs;
using UnityEngine;

namespace Samurai.UnityFramework
{
    public class AppStartUp : MonoBehaviour
    {
        [SerializeField] private AppConfig _config;

        private async void Start()
        {
            App.Init();
            
            var defProvider = new ScriptableObjectDefinitionProvider(_config.ConfigsPath, _config.DefinitionsPath);
            Definitions.Init(defProvider);

            if (_config.SkipIntro)
            {
                await Scenes.Load(_config.MainMenuScene);
                return;
            }
            
            await Scenes.Load(_config.IntroScene);

            if (Scenes.TryGetReference<IntroScreen>(_config.IntroScene, out var intro))
            {
                await intro.Show();
            }

            await Scenes.Load(_config.MainMenuScene);
            await Scenes.Unload(_config.IntroScene);
        }
    }
}