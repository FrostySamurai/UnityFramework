using Samurai.UnityFramework.Events;
using Samurai.UnityFramework.Pooling;
using UnityEditor;
using UnityEngine;

namespace Samurai.UnityFramework
{
    public static class App
    {
        private const string LogTag = "Application";
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

        #region Gameplay

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

        #endregion Gameplay

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