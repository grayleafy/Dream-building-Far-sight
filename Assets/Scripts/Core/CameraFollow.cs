using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    public float persDistance = 6;
    public float orthoDistance = 100;
    public float currentDistance = 6;


    float horizontal = 0;
    float vertical = 0;
    Vector3 lastMousePos;
    Vector3 mouseDelta;

    // Start is called before the first frame update
    void Start()
    {
        lastMousePos = Input.mousePosition;
        mouseDelta = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            mouseDelta = Vector3.zero;
            lastMousePos = Input.mousePosition;
        }
        else
        {
            mouseDelta = Input.mousePosition - lastMousePos;
            lastMousePos = Input.mousePosition;
        }
    }

    private void LateUpdate()
    {
        if (GetComponent<ViewChange>()._changing == false && GetComponent<ViewChange>().isOrthographic == false)
        {
            ChangeAngle();
        }
        else
        {
            vertical = Camera.main.transform.rotation.eulerAngles.x;
            horizontal = Camera.main.transform.rotation.eulerAngles.y;
        }

        if (target != null)
        {
            if (GetComponent<ViewChange>().isOrthographic == false && GetComponent<ViewChange>()._changing == false || GetComponent<ViewChange>().isOrthographic == true && GetComponent<ViewChange>()._changing == true)
            {
                currentDistance = Mathf.Lerp(currentDistance, persDistance, Time.deltaTime);
            }
            else
            {
                currentDistance = Mathf.Lerp(currentDistance, orthoDistance, Time.deltaTime);
            }

            Vector3 pos = target.transform.position - currentDistance * Camera.main.transform.forward;
            Camera.main.transform.position = pos;
        }
    }

    void ChangeAngle()
    {
        horizontal += mouseDelta.x * 0.2f;
        horizontal = (horizontal + 360) % 360;
        vertical -= mouseDelta.y * 0.2f;
        vertical = Mathf.Clamp(vertical, -20, 60);
        Quaternion rotation = Quaternion.Euler(new Vector3(0, horizontal, 0)) * Quaternion.Euler(new Vector3(vertical, 0, 0));
        Camera.main.transform.rotation = rotation;
    }
}
