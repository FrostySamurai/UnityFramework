using Samurai.UnityFramework.Defs;
using UnityEngine;

namespace Samurai.UnityFramework
{
    public class AppStartUp : MonoBehaviour
    {
        [SerializeField] private AppConfig _config;
        [SerializeField] private SceneHandler _sceneHandler;

        private async void Start()
        {
            App.Init();
            App.Set(_sceneHandler);
            
            var defProvider = new ScriptableObjectDefinitionProvider(_config.ConfigsPath, _config.DefinitionsPath);
            Definitions.Init(defProvider);

            if (_config.SkipIntro)
            {
                await _sceneHandler.Load(_config.MainMenuScene);
                return;
            }
            
            await _sceneHandler.Load(_config.IntroScene);

            if (SceneHandler.TryGetReference<IntroScreen>(_config.IntroScene, out var intro))
            {
                await intro.Show();
            }

            await _sceneHandler.Load(_config.MainMenuScene);
            await _sceneHandler.Unload(_config.IntroScene);
        }
    }
}