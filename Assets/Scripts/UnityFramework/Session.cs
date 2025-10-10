using System.Collections.Generic;
using System.IO;
using Samurai.UnityFramework.Defs;
using Samurai.UnityFramework.Events;

namespace Samurai.UnityFramework
{
    public static class Session
    {
        private const string LogTag = nameof(Session);

        private static DataContainer _sessionData;

        public static EventAggregator Events => _sessionData?.Get<EventAggregator>();

        public static void Create(string id)
        {
            Log.Debug("Initializing.", LogTag);
            
            _sessionData = new DataContainer(id);
            _sessionData.Set(new EventAggregator(App.Events));
            _sessionData.Load(GetSaveFolderPath());
            
            Log.Debug("Initialized.", LogTag);
        }

        public static void Dispose()
        {
            if (!Exists())
            {
                return;
            }

            Events.Dispose();
            _sessionData = null;
            Log.Debug($"Disposed.", LogTag);
        }
        
        public static T Get<T>()
        {
            if (!Exists())
            {
                return default;
            }

            return _sessionData.TryGet<T>(out var data) ? data : App.Get<T>();
        }

        public static bool TryGet<T>(out T data)
        {
            if (!Exists())
            {
                data = default;
                return false;
            }

            bool found = _sessionData.TryGet(out data);
            found &= App.TryGet(out data);
            return found;
        }
        
        public static void Save()
        {
            if (!Exists())
            {
                return;
            }
            
            Log.Debug($"Saving session '{_sessionData.Id}'.", LogTag);
            _sessionData.Save(GetSaveFolderPath());
        }

        public static bool Exists()
        {
            return _sessionData is not null;
        }

        #region Utilities

        public static void GetSaves(List<string> saves)
        {
            FileHandler.GetAllFileNames(saves, GetSaveFolderPath(), Definitions.Config<AppConfig>().SaveExtension);
        }

        #endregion Utilities

        private static string GetSaveFolderPath()
        {
            var appConfig = Definitions.Config<AppConfig>();
            if (!Profile.Exists)
            {
                return appConfig.SaveFolderPath;
            }

            return Path.Combine(appConfig.SaveFolderPath, Profile.Current.Id);
        }
    }
}