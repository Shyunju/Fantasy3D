using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class PersistentAttribute : Attribute
{
    public readonly bool Persistent;
    public PersistentAttribute(bool persistent)
    {
        this.Persistent = persistent;
    }
}

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    protected static T _instance = null;
    private static bool _instantiated = false;
    private bool _persistent = false;
    [SerializeField]
    private bool _isDontDestroy = false;

    public static bool isInitialize { get => _instance != null; }

    static public T Instance
    {
        get
        {
            if (_instance == null)
                Create();
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    static public void Create()
    {
        if (_instance == null)
        {
            //T[] objects = FindObjectsOfType<T>();
            T[] objects = FindObjectsByType<T>(FindObjectsSortMode.InstanceID);
            if (objects.Length > 0)
            {
                _instance = objects[0];

                for (int i = 1; i < objects.Length; ++i)
                {
                    if (Application.isPlaying)
                        Destroy(objects[i].gameObject);
                    else
                        DestroyImmediate(objects[i].gameObject);
                }
            }
            else
            {
                GameObject go = new GameObject(string.Format("{0}", typeof(T).Name));
                _instance = go.AddComponent<T>();
            }

            if (!_instantiated)
            {
                PersistentAttribute attribute = Attribute.GetCustomAttribute(typeof(T), 
                    typeof(PersistentAttribute)) as PersistentAttribute;
                if (attribute != null && attribute.Persistent)
                {
                    _instance._persistent = attribute.Persistent;
                    DontDestroyOnLoad(_instance.gameObject);
                }

                //_instance.OnAwake();
            }

            _instantiated = true;
        }
    }

    private void Awake()
    {
        Create();
        if (_isDontDestroy)
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(this);
            }
        }
        OnAwake();
    }

    virtual protected void OnDestroy()
    {
        if (!this._persistent)
        {
            _instantiated = false;
            _instance = null;
        }
    }

    virtual protected void OnAwake() { }
}
