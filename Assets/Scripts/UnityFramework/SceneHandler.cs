using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Samurai.UnityFramework
{
    public class SceneHandler : MonoBehaviour
    {
        private const string LogTag = "Scenes";

        private static readonly Dictionary<string, DataContainer> _sceneContexts = new();

        public static void SetReference<T>(T component) where T : Component
        {
            if (_sceneContexts.TryGetValue(component.gameObject.scene.name, out var context))
            {
                context.Set(component);
            }
        }

        public static T GetReference<T>(string sceneName) where T : Component
        {
            return _sceneContexts.TryGetValue(sceneName, out var context) ? context.Get<T>() : null;
        }

        public static bool TryGetReference<T>(string sceneName, out T reference) where T : Component
        {
            reference = null;
            if (_sceneContexts.TryGetValue(sceneName, out var context))
            {
                reference = context.Get<T>();
            }
            return reference != null;
        }
        
        public async Awaitable Load(string sceneName)
        {
            Log.Debug($"Loading '{sceneName}'.", LogTag);
            
            _sceneContexts[sceneName] = new DataContainer(sceneName);
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            
            Log.Debug($"Loaded '{sceneName}'.", LogTag);
        }

        public async Awaitable Unload(string sceneName)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            if (!scene.isLoaded)
            {
                return;
            }

            Log.Debug($"Unloading '{sceneName}'.", LogTag);
            
            _sceneContexts.Remove(sceneName);
            await SceneManager.UnloadSceneAsync(scene);
            
            Log.Debug($"Unloaded '{sceneName}'.", LogTag);
        }
    }
}