using System.Collections.Generic;
using Samurai.UnityFramework.Defs;

namespace Samurai.UnityFramework
{
    public static class Profile
    {
        internal const string LogTag = nameof(Profile);
        
        // TODO: this should allow for loading multiple profiles (in case of multiplayer game)
        private static DataContainer _current;

        public static DataContainer Current => _current;
        public static bool Exists => _current is not null;

        #region Profile Handling

        public static void Load(string id)
        {
            Log.Debug($"Setting profile to '{id}'.", LogTag);

            var config = Definitions.Config<AppConfig>();
            _current = new DataContainer(id);
            _current.Load(config.SaveFolderPath);
        }

        public static void Unload(bool save = false)
        {
            if (!Exists)
            {
                return;
            }

            if (save)
            {
                Save();
            }
            
            _current = null;
            Log.Debug($"Disposed.", LogTag);
        }

        public static void Save()
        {
            if (!Exists)
            {
                return;
            }
            
            var config = Definitions.Config<AppConfig>();
            _current.Save(config.SaveFolderPath);
        }

        #endregion Profile Handling

        #region Utilities

        public static void GetExistingProfiles(List<string> profileIds)
        {
            var config = Definitions.Config<AppConfig>();
            FileHandler.GetAllFileNames(profileIds, config.SaveFolderPath, Definitions.Config<AppConfig>().SaveExtension);
        }

        #endregion Utilities
    }
}
