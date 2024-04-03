using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeViewButton : BasePanel
{
    public GameObject bindObject;
    public Vector3 offset = new Vector3(0, 1, 0);

    public void Bind(GameObject obj)
    {
        bindObject = obj;
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Camera.main.GetComponent<ViewChange>().ChangeProjection = true;
        });
    }

    private void Update()
    {
        if (bindObject != null)
        {
            Vector3 pos = bindObject.transform.position + offset;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
            GetComponent<RectTransform>().position = screenPos;
        }
    }
}
