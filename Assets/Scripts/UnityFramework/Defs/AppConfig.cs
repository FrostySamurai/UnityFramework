using UnityEngine;

namespace Samurai.UnityFramework.Defs
{
    [CreateAssetMenu(fileName = "AppConfig", menuName = "AppConfig")]
    public class AppConfig : Config
    {
        [Header("Debug")]
        public bool SkipIntro;
        
        [Header("Definitions")]
        public string ConfigsPath;
        public string DefinitionsPath;

        [Header("Scenes")]
        public string IntroScene;
        public string MainMenuScene;
        public string LoadingScene;
        public string SessionScene;
    }
}