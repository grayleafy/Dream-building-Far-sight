using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorTimer : SingletonBase<EditorTimer>
{
    public double lastTime;
    public double currentTime;
    public EditorTimer()
    {
#if UNITY_EDITOR
        lastTime = currentTime = EditorApplication.timeSinceStartup;
        EditorApplication.update += () =>
        {
            lastTime = currentTime;
            currentTime = EditorApplication.timeSinceStartup;
        };
#endif
    }

    public double EditorDeltaTime => currentTime - lastTime;


}
