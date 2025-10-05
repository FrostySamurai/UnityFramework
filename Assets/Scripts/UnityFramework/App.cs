using Samurai.UnityFramework.Events;
using UnityEditor;

namespace Samurai.UnityFramework
{
    public static class App
    {
        private const string LogTag = "Application";
        private static DataContainer _data = new(nameof(App));

        public static EventAggregator Events => _data.Get<EventAggregator>();

        public static void Init()
        {
            Log.Debug("Initializing.", LogTag);
            
            _data.Set(new EventAggregator());
            
            Log.Debug("Initialized", LogTag);
        }

        public static void Dispose()
        {
            Events.Dispose();
            _data = null;
            
            Log.Debug("Disposed.", LogTag);
        }

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

        public static void Quit()
        {
            Dispose();
            
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}