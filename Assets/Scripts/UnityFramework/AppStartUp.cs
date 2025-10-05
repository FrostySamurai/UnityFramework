using Samurai.UnityFramework.Defs;
using UnityEngine;
using UnityEngine.SceneManagement;

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

            var scene = SceneManager.GetSceneByName(_config.IntroScene);
            foreach (var entry in scene.GetRootGameObjects())
            {
                if (entry.TryGetComponent<IntroScreen>(out var intro))
                {
                    await intro.Show();
                    break;
                }
            }

            await _sceneHandler.Load(_config.MainMenuScene);
            await _sceneHandler.Unload(_config.IntroScene);
        }
    }
}