using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RuntimeTimer : SingletonMono<RuntimeTimer>
{
    /// <summary>
    /// 等待一段时间后执行
    /// </summary>
    /// <param name="action"></param>
    /// <param name="waitTime"></param>
    public void WaitForInvoke(float waitTime, UnityAction action)
    {
        StartCoroutine(WaitForInvokeReally(action, waitTime));
    }

    IEnumerator WaitForInvokeReally(UnityAction action, float waitTime)
    {
        while (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            yield return null;
        }
        action();
    }
}
