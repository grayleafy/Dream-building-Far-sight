using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ObjectInteract : MonoBehaviour
{
    public ChangeViewButton button;
    public bool havButton = false;


    private void Reset()
    {
        GetComponent<SphereCollider>().isTrigger = true;
    }

    /// <summary>
    /// 状态改变时刷新
    /// </summary>
    public void Refresh()
    {
        HideButton();
    }

    void OpenButton()
    {
        if (button != null)
        {
            UIMgr.GetInstance().HidePanel(button);
        }
        havButton = true;
        UIMgr.GetInstance().ShowPanel<ChangeViewButton>("ChangeViewButton", (panel) =>
        {
            if (button != null)
            {
                UIMgr.GetInstance().HidePanel(button);
            }
            button = panel;
            button.Bind(gameObject);
        });
    }

    void HideButton()
    {
        havButton = false;
        if (button != null)
        {
            UIMgr.GetInstance().HidePanel(button);
            button = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            OpenButton();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            HideButton();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (havButton == false && other.tag == "Player")
        {
            OpenButton();
        }
    }
}
