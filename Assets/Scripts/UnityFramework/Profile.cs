using System.Collections.Generic;
using Samurai.UnityFramework.Defs;

namespace Samurai.UnityFramework
{
    public static class Profile
    {
        private const string LogTag = "Profile";
        
        // TODO: this should allow for loading multiple profiles (in case of multiplayer game)
        private static DataContainer _current;

        public static DataContainer Current => _current;
        public static bool Exists => _current is not null; // TODO: maybe a better name

        #region Profile Handling

        public static DataContainer Load(string id)
        {
            Log.Debug($"Setting profile to '{id}'.", LogTag);

            var config = Definitions.Config<AppConfig>();
            var profile = FileHandler.Load<DataContainer>(config.SaveFolderPath, id);
            if (profile is not null)
            {
                Log.Debug($"Profile loaded from save file!", LogTag);
            }

            _current = profile ?? new DataContainer(id);
            return _current;
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
            FileHandler.Save(_current, config.SaveFolderPath, _current.Id);
        }

        #endregion Profile Handling

        #region Utilities

        public static void GetExistingProfiles(List<string> profileIds)
        {
            var config = Definitions.Config<AppConfig>();
            FileHandler.GetAllFileNames(profileIds, config.SaveFolderPath, "sav");
        }

        #endregion Utilities
    }
}
