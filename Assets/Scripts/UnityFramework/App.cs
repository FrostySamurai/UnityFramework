using Samurai.Example.UI.Loading;
using Samurai.Example.UI.MainMenu;
using Samurai.UnityFramework.Defs;
using Samurai.UnityFramework.Events;
using Samurai.UnityFramework.Loading;
using Samurai.UnityFramework.Pooling;
using Samurai.UnityFramework.UI;
using UnityEditor;
using UnityEngine;

namespace Samurai.UnityFramework
{
    public static class App
    {
        private const string LogTag = nameof(App);
        private static DataContainer _data = new(nameof(App));

        public static bool IsPaused { get; private set; }
        public static EventAggregator Events => _data.Get<EventAggregator>();
        public static ComponentPool Pool => _data.Get<ComponentPool>();

        #region Lifecycle

        public static void Init()
        {
            Log.Debug("Initializing.", LogTag);
            
            _data.Set(new EventAggregator());
            _data.Set(new ComponentPool());
            
            Log.Debug("Initialized", LogTag);
        }

        public static void Dispose()
        {
            Events.Dispose();
            _data = null;
            
            Log.Debug("Disposed.", LogTag);
        }

        public static void Quit()
        {
            Dispose();
            
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        #endregion Lifecycle

        #region Session

        public static async void StartSession(string id, params  ILoadingStepProvider[] loadingStepProviders)
        {
            var config = Definitions.Config<AppConfig>();

            Session.Create(id);
            
            await Scenes.Load(config.LoadingScene);
            await Scenes.Unload(config.MainMenuScene);

            await Awaitable.MainThreadAsync();
            if (Scenes.TryGetReference<Loader>(config.LoadingScene, out var loader))
            {
                loader.Add(loadingStepProviders);
                await loader.Load();
            }
            
            var sessionLoad = Scenes.Load(config.SessionScene);
            var loadingUnload = Scenes.Unload(config.LoadingScene);

            await sessionLoad;
            await loadingUnload;
        }

        public static async void EndSession()
        {
            var config = Definitions.Config<AppConfig>();

            await Scenes.Load(config.MainMenuScene);
            await Scenes.Unload(config.SessionScene);

            if (Scenes.TryGetReference<WindowManager>(config.MainMenuScene, out var windowManager))
            {
                windowManager.Show<GameSelectionWindow>(force: true);
            }
            
            Session.Dispose();
        }

        public static void TogglePause()
        {
            if (IsPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }

        public static void Pause()
        {
            IsPaused = true;
            Time.timeScale = 0f;
        }

        public static void Resume()
        {
            IsPaused = false;
            Time.timeScale = 1f;
        }

        #endregion Session

        #region Data

        public static T Get<T>()
        {
            return _data.Get<T>();
        }

        public static bool TryGet<T>(out T data)
        {
            return _data.TryGet(out data);
        }

        public static void Set<T>(T data)
        {
            _data.Set(data);
        }

        public static void Remove<T>()
        {
            _data.Remove<T>();
        }

        #endregion Data
    }
}