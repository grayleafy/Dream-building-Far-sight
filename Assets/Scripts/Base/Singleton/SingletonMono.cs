﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    static bool isDestroyed = false;
    private static string rootName = "Managers";

    public static T GetInstance()
    {
        if (isDestroyed) return null;
        if (instance == null)
        {
            GameObject obj = GameObject.Find(typeof(T).ToString());
            if (obj == null)
            {
                obj = new GameObject();
                //设置对象的名字为脚本名
                obj.name = typeof(T).ToString();
                //让这个单例模式对象 过场景 不移除
                //因为 单例模式对象 往往 是存在整个程序生命周期中的
                DontDestroyOnLoad(obj);
                instance = obj.AddComponent<T>();
            }
            else
            {
                if (obj.transform.parent == null) DontDestroyOnLoad(obj);
                instance = obj.GetComponent<T>();
                if (instance == null)
                {
                    Debug.LogError("场景中已经存在单例类名称的gameObject,但是其他地方在其初始化前尝试获取单例");
                }
            }



            GameObject managers = GameObject.Find(rootName);
            if (managers == null)
            {
                managers = new GameObject(rootName);
            }
            DontDestroyOnLoad(managers);
            obj.transform.parent = managers.transform;
        }

        return instance;
    }

    public virtual void OnDestroy()
    {
        isDestroyed = true;
    }
}
