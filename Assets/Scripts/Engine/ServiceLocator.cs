using System;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

    public static void Register<T>(T service)
    {
        Type type = typeof(T);
        if (_services.ContainsKey(type))
        {
            Debug.LogWarning($"Service of type {type.Name} is already registered.");
            return;
        }
        _services.Add(type, service);
    }

    public static T Get<T>()
    {
        Type type = typeof(T);
        if (!_services.TryGetValue(type, out object service))
        {
            throw new Exception($"Service of type {type.Name} is not registered.");
        }
        return (T)service;
    }

    public static void Unregister<T>()
    {
        Type type = typeof(T);
        if (_services.ContainsKey(type))
        {
            _services.Remove(type);
            Debug.Log($"Service of type {type.Name} Unregistered.");
        }
    }

    public static void Clear()
    {
        _services.Clear();
        Debug.Log("Service Locator cleared");
    }
}