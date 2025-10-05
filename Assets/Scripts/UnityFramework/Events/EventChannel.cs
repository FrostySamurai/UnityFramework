using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace Samurai.UnityFramework.Events
{
    public delegate void EventCallback<in T>(T evt) where T : IEvent;
    
    internal abstract class EventChannel
    {
        internal abstract Type DataType { get; }

        internal abstract void Unregister(object source);
    }

    internal class EventChannel<T> : EventChannel where T : IEvent
    {
        internal override Type DataType => typeof(T);

        private readonly List<EventCallback<T>> _callbacks = new();
        private readonly Dictionary<object, EventCallback<T>> _callbacksBySource = new();

        internal void Register(EventCallback<T> callback, object source)
        {
            if (_callbacksBySource.TryGetValue(source, out var existing))
            {
                Remove(existing, source);
            }
            
            _callbacks.Add(callback);
            _callbacksBySource[source] = callback;
        }

        internal override void Unregister(object source)
        {
            if (_callbacksBySource.TryGetValue(source, out var callback))
            {
                Remove(callback, source);
            }
        }

        internal void Raise(T @event)
        {
            using var obj = ListPool<EventCallback<T>>.Get(out var callbacks);
            callbacks.AddRange(_callbacks);
            foreach (var entry in callbacks)
            {
                entry?.Invoke(@event);
            }
        }

        private void Remove(EventCallback<T> callback, object source)
        {
            _callbacksBySource.Remove(source);
            int index = _callbacks.FindIndex(x => x == callback);
            if (index >= 0)
            {
                _callbacks.RemoveAt(index);
            }
        }
    }
}