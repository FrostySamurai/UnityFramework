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
            Log.Debug("Loading.");
            // TODO: try loading

            _current = new DataContainer(id);
            
            Log.Debug($"Profile '{id}' loaded.", LogTag);
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
