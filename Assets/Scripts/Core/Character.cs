using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    public float moveSpeed = 4;

    private void Reset()
    {
        GetComponent<Rigidbody>().useGravity = false;
        gameObject.tag = "Player";
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        FixedUpdateMotion();
        Stick();
    }

    void FixedUpdateMotion()
    {
        float up = (Input.GetKey(KeyCode.W) ? 1 : -1) + (Input.GetKey(KeyCode.S) ? -1 : 1);
        float right = (Input.GetKey(KeyCode.D) ? 1 : -1) + (Input.GetKey(KeyCode.A) ? -1 : 1);
        Vector2 input = new Vector2(right, up).normalized;
        Vector3 dir = InputToDrection(input);
        GetComponent<Rigidbody>().velocity = dir * moveSpeed;
    }

    /// <summary>
    /// 相机下的移动方向
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Vector3 InputToDrection(Vector2 input)
    {
        Vector3 up = Camera.main.transform.up;
        Vector3 right = Camera.main.transform.right;
        Vector3 dir = input.x * right + input.y * up;
        return Vector3.ProjectOnPlane(dir, Vector3.up).normalized;
    }


    void Stick()
    {
        Vector3 origin = transform.position + Vector3.up * 1.5f;
        int mask = (1 << LayerMask.NameToLayer("Character"));
        if (Physics.SphereCast(origin, 0.5f, Vector3.down, out RaycastHit hitInfo, 2, ~mask))
        {
            float dy = hitInfo.point.y - transform.position.y;
            GetComponent<Rigidbody>().AddForce(new Vector3(0, dy / Time.fixedDeltaTime - GetComponent<Rigidbody>().velocity.y, 0), ForceMode.VelocityChange);
            //transform.position = hitInfo.point;
        }
    }
}
