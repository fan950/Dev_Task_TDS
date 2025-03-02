using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour
{
    protected bool bDestroy;
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject findObj = GameObject.Find(typeof(T).Name);

                if (findObj)
                {
                    instance = findObj.GetComponent<T>();
                }
                else
                {
                    GameObject obj = Instantiate(Resources.Load("Manager/" + typeof(T).Name) as GameObject);
                    obj.name = typeof(T).Name;
                    instance = obj.GetComponent<T>();
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (null == instance)
            instance = GetComponent<T>();
        if (!bDestroy)
            DontDestroyOnLoad(gameObject);
    }
    public virtual void Init() { }
}