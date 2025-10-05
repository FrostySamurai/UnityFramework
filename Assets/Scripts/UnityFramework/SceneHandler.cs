using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Samurai.UnityFramework
{
    public class SceneHandler : MonoBehaviour
    {
        public async Task Load(string sceneName)
        {
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        public async Task Unload(string sceneName)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            if (!scene.isLoaded)
            {
                return;
            }

            await SceneManager.UnloadSceneAsync(scene);
        }
    }
}