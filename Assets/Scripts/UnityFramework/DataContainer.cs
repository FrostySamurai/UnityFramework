using System;
using System.Collections.Generic;

namespace Samurai.UnityFramework
{
    [Serializable]
    public class DataContainer
    {
        public readonly string Id;
        
        private readonly Dictionary<Type, object> _data = new();

        public DataContainer(string id)
        {
            Id = id;
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