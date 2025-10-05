using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Samurai.UnityFramework.Pooling
{
    public class ComponentPool
    {
        private readonly Transform _poolParent;
        private readonly Dictionary<int, Queue<Component>> _pools = new();

        #region Lifecycle

        public ComponentPool()
        {
            var pool = new GameObject("[ObjectPool]");
            pool.SetActive(false);
            _poolParent = pool.transform;
        }

        #endregion Lifecycle

        #region Public Access

        public T Retrieve<T>(T prefab, Transform parent) where T : Component
        {
            if (!prefab)
            {
                return null;
            }

            var pool = GetPool(prefab);
            T instance = null;
            while (pool.Count > 0 && !instance)
            {
                var poolItem = pool.Dequeue();
                instance = poolItem as T;
                if (!instance && poolItem)
                {
                    Object.Destroy(poolItem.gameObject);
                }
            }
            
            if (!instance)
            {
                instance = Object.Instantiate(prefab);
                var poolInstance = instance.gameObject.AddComponent<ObjectPoolInstance>();
                poolInstance.PrefabInstanceId = prefab.GetInstanceID();
            }

            var transform = instance.transform;
            transform.SetParent(parent);
            transform.localScale = Vector3.one;

            if (instance is IPoolable poolable)
            {
                poolable.OnRetrievedFromPool();
            }
            
            return instance;
        }

        public void Return<T>(T instance, T prefab) where T : MonoBehaviour
        {
            if (!instance || !prefab)
            {
                return;
            }

            var pool = GetPool(prefab);
            instance.transform.SetParent(_poolParent);
            pool.Enqueue(instance);

            if (instance is IPoolable poolable)
            {
                poolable.OnReturnedToPool();
            }
        }

        public void Return<T>(IEnumerable<T> instances, T prefab) where T : Component
        {
            if (!prefab)
            {
                return;
            }

            var pool = GetPool(prefab);
            foreach (var instance in instances)
            {
                if (!instance)
                {
                    continue;
                }
                
                instance.transform.SetParent(_poolParent);
                pool.Enqueue(instance);

                if (instance is IPoolable poolable)
                {
                    poolable.OnReturnedToPool();
                }
            }
        }

        public void ReturnChildren<T>(T prefab, Transform parent) where T : Component
        {
            if (!prefab || !parent)
            {
                return;
            }
            
            Return(GetChildren(prefab, parent), prefab);
        }

        #endregion Public Access

        #region Private Helpers

        private Queue<Component> GetPool(Component prefab)
        {
            var prefabInstanceId = prefab.gameObject.GetInstanceID();
            if (_pools.TryGetValue(prefabInstanceId, out var pool))
            {
                return pool;
            }

            pool = new Queue<Component>();
            _pools[prefabInstanceId] = pool;
            return pool;
        }
        
        private IEnumerable<T> GetChildren<T>(T prefab, Transform parent) where T : Component
        {
            var instances = parent.GetComponentsInChildren<T>(true);
            for (var i = 0; i < instances.Length; i++)
            {
                var instance = instances[i];
                if (instance.transform == parent)
                {
                    instances[i] = null;
                    continue;
                }

                if (!instance.TryGetComponent<ObjectPoolInstance>(out var poolInstance))
                {
                    Object.Destroy(instance.gameObject);
                    instances[i] = null;
                    continue;
                }

                if (prefab.GetInstanceID() != poolInstance.PrefabInstanceId)
                {
                    instances[i] = null;
                }
            }
            return instances.Where(x => x != null);
        }

        #endregion Private Helpers
    }
}