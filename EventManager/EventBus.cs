using System;
using System.Collections.Generic;
using System.Linq;

public static class EventBus
{
    private static readonly Dictionary<Type, List<IGlobalSubscriber>> eventSubscribers = new Dictionary<Type, List<IGlobalSubscriber>>();

    public static void RaiseEvent<TSubscriber>(Action<TSubscriber> action) where TSubscriber : class, IGlobalSubscriber
    {
        if(!eventSubscribers.ContainsKey(typeof(TSubscriber))) return;
        List<IGlobalSubscriber> subscribers = eventSubscribers[typeof(TSubscriber)];
        foreach (IGlobalSubscriber subscriber in subscribers)
        {
            action?.Invoke(subscriber as TSubscriber);
        }
    }
    public static void Subscribe(IGlobalSubscriber subscriber)
    {
        List<Type> subscriberTypes = GetSubscriberTypes(subscriber);
        foreach (Type t in subscriberTypes)
        {
            if (!eventSubscribers.ContainsKey(t))
                eventSubscribers[t] = new List<IGlobalSubscriber>();
            eventSubscribers[t].Add(subscriber);
        }
    }

    private static List<Type> GetSubscriberTypes(IGlobalSubscriber globalSubscriber)
    {
        Type type = globalSubscriber.GetType();
        List<Type> subscriberTypes = type
            .GetInterfaces()
            .Where(it =>
                it.IsAssignableFrom(type) &&
                it != typeof(IGlobalSubscriber))
            .ToList();
        return subscriberTypes;
    }

    public static void Unsubscribe(IGlobalSubscriber subscriber)
    {
        List<Type> subscriberTypes = GetSubscriberTypes(subscriber);
        foreach (var type in subscriberTypes)
        {
            if (eventSubscribers.ContainsKey(type))
                eventSubscribers[type].Remove(subscriber);
        }
    }
}
        
   