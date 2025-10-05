using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Pool;

namespace Samurai.UnityFramework.Defs
{
    public class Definitions
    {
        internal const string LogTag = "Definitions";

        #region Static

        private static Definitions _instance;

        internal static void Init(IDefinitionProvider provider)
        {
            using var obj = ListPool<IDefinitionProvider>.Get(out var providers);
            providers.Add(provider);
            Init(providers);
        }
        
        internal static void Init(IEnumerable<IDefinitionProvider> providers)
        {
            if (_instance != null)
            {
                Log.Debug("An instance of definitions is already created. Skipping creation..", LogTag);
                return;
            }

            Log.Debug("Initializing.", LogTag);
            _instance = new Definitions(providers);
            Log.Debug("Initialized.", LogTag);
        }

        internal static void Dispose()
        {
            _instance = null;
            Log.Debug("Disposed.", LogTag);
        }

        public static T Config<T>() where T : class, IConfig
        {
            return _instance._configs.TryGetValue(typeof(T), out var config) ? (T)config : null;
        }

        public static bool TryGet<T>(string id, out T definition) where T : class, IDefinition
        {
            definition = Get<T>(id);
            return definition is not null;
        }
    
        public static T Get<T>(string id) where T : class, IDefinition
        {
            if (!_instance._definitions.TryGetValue(typeof(T), out var definitions))
            {
                return null;
            }

            return definitions.TryGetValue(id, out var definition) ? (T)definition : null;
        }

        public static IEnumerable<T> Get<T>(Predicate<T> predicate = null) where T : IDefinition
        {
            if (!_instance._definitions.TryGetValue(typeof(T), out var definitions))
            {
                return Enumerable.Empty<T>();
            }

            var typed = definitions.Values.Cast<T>();
            return predicate != null ? typed.Where(x => predicate(x)) : typed;
        }

        public static void Get<T>(List<T> result, Predicate<T> predicate = null) where T : IDefinition
        {
            if (!_instance._definitions.TryGetValue(typeof(T), out var definitions))
            {
                return;
            }

            var typed = definitions.Values.Cast<T>();
            if (predicate == null)
            {
                result.AddRange(typed);
                return;
            }

            result.AddRange(typed.Where(x => predicate(x)));
        }

        #endregion Static
    
        private readonly Dictionary<Type, Dictionary<string, IDefinition>> _definitions = new();
        private readonly Dictionary<Type, IConfig> _configs = new();

        private Definitions(IEnumerable<IDefinitionProvider> providers)
        {
            if (providers is null) 
                return;

            using var objD = ListPool<IDefinition>.Get(out var definitions);
            using var objC = ListPool<IConfig>.Get(out var configs);
            foreach (var provider in providers)
            {
                if (provider is null) 
                    continue;
                
                configs.Clear();
                definitions.Clear();
                
                provider.GetConfigs(configs);
                foreach (var entry in configs)
                {
                    var type = entry.GetType();
                    if (!_configs.TryAdd(type, entry))
                    {
                        Log.Error($"Duplicate config '{type.Name}'! Skipping..");
                    }
                }
                
                provider.GetDefinitions(definitions);
                foreach (var entry in definitions)
                {
                    var type = entry.GetType();
                    if (!_definitions.TryGetValue(type, out var typeDefinitions))
                    {
                        typeDefinitions = new();
                        _definitions[type] = typeDefinitions;
                    }

                    if (!typeDefinitions.TryAdd(entry.Id, entry))
                    {
                        Log.Error($"Diplicate definition '{type.Name}' with Id '{entry.Id}'! Skipping..");
                    }
                }
            }
        }
    }
}