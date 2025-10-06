namespace Samurai.UnityFramework
{
    public static class Profile
    {
        private const string LogTag = "Profile";
        
        // TODO: this should allow for loading multiple profiles (in case of multiplayer game)
        private static DataContainer _current;

        public static DataContainer Current => _current;
        public static bool Exists => _current is not null; // TODO: maybe a better name

        public static DataContainer Load(string id)
        {
            Log.Debug($"Setting profile to '{id}'.", LogTag);
            
            // if (load)
            // {
                // TODO: load
                // Log.Debug($"Profile loaded from save file!", LogTag);
            // }

            _current = new DataContainer(id);
            
            return _current;
        }

        public static void Unload()
        {
            if (!Exists)
            {
                return;
            }
            
            // TODO: saving?
            _current = null;
            Log.Debug($"Disposed.", LogTag);
        }

        public static void Save()
        {
            if (Exists)
            {
                //TODO: do saving
            }
        }
    }
}
