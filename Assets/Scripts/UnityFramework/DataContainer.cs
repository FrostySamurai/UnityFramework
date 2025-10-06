using System;
using System.Collections.Generic;
using Mirzipan.Extensions.Collections;
using UnityEngine.Pool;

namespace Samurai.UnityFramework
{
    [Serializable]
    public class DataContainer
    {
        public readonly string Id;
        
        // TODO: this doesn't save values properly
        private readonly Dictionary<Type, object> _data = new();

        public DataContainer(string id)
        {
            Id = id;
            if (Id.IsNullOrEmpty())
            {
                // fallback
                Id = Guid.NewGuid().ToString();
            }
        }

        public void Save(string folderPath)
        {
            using var obj = ListPool<ISavable>.Get(out var savables);
            foreach (object entry in _data.Values)
            {
                if (entry is ISavable savable)
                {
                    savables.Add(savable);
                }
            }
            
            FileHandler.Save(savables, folderPath, Id);
        }

        public void Load(string folderPath)
        {
            var savables = FileHandler.Load<List<ISavable>>(folderPath, Id);
            if (savables is null)
            {
                return;
            }
            
            savables.ForEach(Set);
            Log.Debug("Loaded from save file!", Profile.LogTag);
        }

        public void Set<T>(T obj)
        {
            if (obj is null)
            {
                return;
            }

            _data[typeof(T)] = obj;
        }

        public void Remove<T>()
        {
            _data.Remove(typeof(T));
        }

        public T Get<T>()
        {
            return _data.TryGetValue(typeof(T), out object obj) ? (T)obj : default;
        }

        public bool TryGet<T>(out T data)
        {
            bool found = _data.TryGetValue(typeof(T), out object obj);
            data = found ? (T)obj : default;
            return found;
        }
    }
}