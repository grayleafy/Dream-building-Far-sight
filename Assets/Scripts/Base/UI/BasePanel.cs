using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    public UILayer uILayer = UILayer.Mid;
    public bool destroyOnHide = true;
    public bool uniquePanel = true;

    public virtual void OnShow()
    {

    }

    public virtual void OnHide()
    {

    }

    /// <summary>
    /// 关闭自己
    /// </summary>
    public void HideSelf()
    {
        UIMgr.GetInstance()?.HidePanel(this);
    }
}
