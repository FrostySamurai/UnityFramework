using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Samurai.UnityFramework
{
    public class SceneHandler : MonoBehaviour
    {
        private const string LogTag = "Scenes";
        
        public async Task Load(string sceneName)
        {
            Log.Debug($"Loading '{sceneName}'.", LogTag);
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            Log.Debug($"Loaded '{sceneName}'.", LogTag);
        }

        public async Task Unload(string sceneName)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            if (!scene.isLoaded)
            {
                return;
            }

            Log.Debug($"Unloading '{sceneName}'.", LogTag);
            await SceneManager.UnloadSceneAsync(scene);
            Log.Debug($"Unloaded '{sceneName}'.", LogTag);
        }
    }
}