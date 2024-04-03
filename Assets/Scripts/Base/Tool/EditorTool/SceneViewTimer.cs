using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SceneViewTimer : SingletonBase<SceneViewTimer>
{
    public double lastTime;
    public double currentTime;
    public SceneViewTimer()
    {
#if UNITY_EDITOR
        lastTime = currentTime = EditorApplication.timeSinceStartup;
        SceneView.duringSceneGui += (secenView) =>
        {
            lastTime = currentTime;
            currentTime = EditorApplication.timeSinceStartup;
        };
#endif
    }

    public double EditorDeltaTime => currentTime - lastTime;
}
