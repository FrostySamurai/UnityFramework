using System.Collections.Generic;

namespace Samurai.UnityFramework.Defs
{
    public interface IDefinitionProvider
    {
        void GetConfigs(List<IConfig> configs);
        void GetDefinitions(List<IDefinition> definitions);
    }
}