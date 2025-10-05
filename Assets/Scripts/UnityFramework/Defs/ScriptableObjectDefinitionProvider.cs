using System.Collections.Generic;
using UnityEngine;

namespace Samurai.UnityFramework.Defs
{
    public class ScriptableObjectDefinitionProvider : IDefinitionProvider
    {
        private readonly string _configsPath;
        private readonly string _definitionsPath;
        
        public ScriptableObjectDefinitionProvider(string configsPath, string definitionsPath)
        {
            _configsPath = configsPath;
            _definitionsPath = definitionsPath;
        }
        
        public void GetConfigs(List<IConfig> configs)
        {
            configs.AddRange(Resources.LoadAll<Config>(_configsPath));
        }

        public void GetDefinitions(List<IDefinition> definitions)
        {
            definitions.AddRange(Resources.LoadAll<Definition>(_definitionsPath));
        }
    }
}