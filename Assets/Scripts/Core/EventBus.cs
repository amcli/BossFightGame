using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public interface IGameEvent { }

    public sealed class EventBus
    {
        readonly Dictionary<Type, Delegate> handlers = new Dictionary<Type, Delegate>();

        public void Subscribe<T>(Action<T> handler) where T : IGameEvent
        {
            if (handler == null) return;
            var type = typeof(T);
            handlers[type] = handlers.TryGetValue(type, out var existing)
                ? Delegate.Combine(existing, handler)
                : handler;
        }

        public void Unsubscribe<T>(Action<T> handler) where T : IGameEvent
        {
            if (handler == null) return;
            var type = typeof(T);
            if (!handlers.TryGetValue(type, out var existing)) return;
            var updated = Delegate.Remove(existing, handler);
            if (updated == null) handlers.Remove(type);
            else handlers[type] = updated;
        }

        public void Publish<T>(T evt) where T : IGameEvent
        {
            if (!handlers.TryGetValue(typeof(T), out var existing)) return;

            // Snapshot the delegate before invoking so handlers can safely
            // unsubscribe themselves (or others) during dispatch.
            var snapshot = (Action<T>)existing;
            try
            {
                snapshot.Invoke(evt);
            }
            catch (Exception e)
            {
                Debug.LogError($"[EventBus] Handler for {typeof(T).Name} threw: {e}");
            }
        }

        public void Clear() => handlers.Clear();
    }
}
