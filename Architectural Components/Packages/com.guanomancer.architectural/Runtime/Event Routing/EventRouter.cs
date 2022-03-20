using System;
using System.Collections.Generic;
using UnityEngine;

namespace Guanomancer.EventRouting
{
    public abstract class EventRouter
    {
        public abstract void AddSubscriber(IEventSubscriber subscriber);
        public abstract void RemoveSubscriber(IEventSubscriber subscriber);

        public readonly List<IEventSubscriber> Subscribers = new List<IEventSubscriber>();



        protected static readonly Dictionary<Type, EventRouter> Routers = new Dictionary<Type, EventRouter>();
        protected static readonly Dictionary<Type, List<IEventSubscriber>> AwaitingSubscribers = new Dictionary<Type, List<IEventSubscriber>>();

        public static void Subscribe<T>(IEventSubscriber subscriber) where T : struct, IEventContext
            => EventRouter<T>.Subscribe(subscriber);

        public static void Subscribe(Type eventType, IEventSubscriber subscriber)
        {
            if (!Routers.ContainsKey(eventType))
            {
                if (!AwaitingSubscribers.ContainsKey(eventType))
                    AwaitingSubscribers.Add(eventType, new List<IEventSubscriber>());
                AwaitingSubscribers[eventType].Add(subscriber);
                return;
            }

            Routers[eventType].AddSubscriber(subscriber);
        }

        public static bool Subscribe(string eventTypeName, IEventSubscriber subscriber)
        {
            var eventType = FindEventTypeByName(eventTypeName);
            
            if (eventType == null)
                return false;

            Subscribe(eventType, subscriber);
            return true;
        }

        public static void Unsubscribe<T>(IEventSubscriber subscriber) where T : struct, IEventContext
            => EventRouter<T>.Unsubscribe(subscriber);

        public static void Unsubscribe(Type eventType, IEventSubscriber subscriber)
        {
            if (!Routers.ContainsKey(eventType))
            {
                if (AwaitingSubscribers.ContainsKey(eventType) && AwaitingSubscribers[eventType].Contains(subscriber))
                    AwaitingSubscribers[eventType].Remove(subscriber);
                return;
            }

            Routers[eventType].RemoveSubscriber(subscriber);
        }

        public static bool Unsubscribe(string eventTypeName, IEventSubscriber subscriber)
        {
            var eventType = FindEventTypeByName(eventTypeName);

            if (eventType == null)
                return false;

            Unsubscribe(eventType, subscriber);
            return true;
        }

        public static void UnsubscribeAll(IEventSubscriber subscriber)
        {
            foreach(var router in Routers.Values)
            {
                if (router.Subscribers.Contains(subscriber))
                    router.RemoveSubscriber(subscriber);
            }
        }

        public static void Dispatch<T>(T eventContext) where T : struct, IEventContext
            => EventRouter<T>.Dispatch(eventContext);

        private static Dictionary<string, Type> _eventTypes;

        public static Type FindEventTypeByName(string eventTypeName)
        {
            if (_eventTypes == null)
            {
                _eventTypes = new Dictionary<string, Type>();
                var appAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var appAssembly in appAssemblies)
                {
                    var assemblyTypes = appAssembly.GetTypes();
                    foreach (var assemblyType in assemblyTypes)
                    {
                        if(assemblyType.GetInterface(nameof(IEventContext)) != null)
                        {
                            if (!_eventTypes.ContainsKey(assemblyType.Name))
                                _eventTypes.Add(assemblyType.Name, assemblyType);
                            else
                                Debug.LogWarning($"You have multiple members with the same name ({assemblyType.Name}) inheriting from {nameof(IEventContext)}. Only one will be enabled in the {nameof(EventRouter)}.");
                        }
                    }
                }
            }

            if (_eventTypes.ContainsKey(eventTypeName))
                return _eventTypes[eventTypeName];
            else
                return null;
        }
    }

    public class EventRouter<T> : EventRouter where T : struct, IEventContext
    {
        private static EventRouter<T> _instance;
        static EventRouter()
            => _instance = new EventRouter<T>();

        private List<IEventSubscriber> _addList = new List<IEventSubscriber>();
        private List<IEventSubscriber> _removeList = new List<IEventSubscriber>();
        private int _dispatchDepth;

        private EventRouter()
        {
            var type = typeof(T);
            Routers.Add(type, this);
            if(AwaitingSubscribers.ContainsKey(type))
            {
                foreach (var subscriber in AwaitingSubscribers[type])
                    AddSubscriber(subscriber);
                AwaitingSubscribers.Remove(type);
            }
        }

        public static void Subscribe(IEventSubscriber subscriber) => _instance.AddSubscriber(subscriber);

        public override void AddSubscriber(IEventSubscriber subscriber)
        {
            if (_dispatchDepth != 0)
            {
                _addList.Add(subscriber);
                return;
            }

            Subscribers.Add(subscriber);
        }

        public static void Unsubscribe(IEventSubscriber subscriber) => _instance.RemoveSubscriber(subscriber);

        public override void RemoveSubscriber(IEventSubscriber subscriber)
        {
            if (_dispatchDepth != 0)
            {
                _removeList.Add(subscriber);
                return;
            }

            Subscribers.Remove(subscriber);
        }

        public static void Dispatch(T eventContext) => _instance.DispatchEvent(eventContext);

        public void DispatchEvent(T eventContext)
        {
            _dispatchDepth++;
            foreach (var subscriber in Subscribers)
                subscriber.OnEvent(eventContext);
            _dispatchDepth--;

            if (_dispatchDepth == 0)
                ProcessSubscriptionChanges();
        }

        private void ProcessSubscriptionChanges()
        {
            foreach (var subscriber in _removeList)
                Subscribers.Remove(subscriber);
            _removeList.Clear();

            foreach (var subscriber in _addList)
                Subscribers.Remove(subscriber);
            _addList.Clear();
        }
    }
}
