using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class Gateway : MonoBehaviour
{
    public Gateway another;
    public bool isForward;
    public Imitater imitater;
    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (Camera.main.GetComponent<ViewChange>().isOrthographic)
        {
            if (other.tag == "Player" && player == null)
            {
                if (IsInSelf(other.transform.position))
                {
                    imitater = CloneCharacter(other.gameObject);
                    player = other.gameObject;
                }
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (player != null)
        {
            Destroy(imitater.gameObject);
            imitater = null;
            player = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.tag == "Player")
        //{
        //    if (IsInSelf(other.transform.position) == false)
        //    {
        //        if (imitater != null)
        //        {
        //            Destroy(imitater.gameObject);
        //            imitater = null;
        //        }
        //        Transmission(other.gameObject);
        //    }
        //}
    }

    private void Update()
    {
        if (player != null && IsInSelf(player.transform.position) == false)
        {
            //if (imitater != null)
            //{
            //    Destroy(imitater.gameObject);
            //    imitater = null;
            //}
            Transmission(player);
        }
    }

    /// <summary>
    /// 是否在自己的门这一侧
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    bool IsInSelf(Vector3 pos)
    {
        return Vector3.Dot(transform.forward, pos - transform.position) > 0 == isForward;
    }


    Imitater CloneCharacter(GameObject go)
    {
        GameObject imitaterObj = GameObject.Instantiate(go);
        imitaterObj.tag = "Untagged";
        foreach (var script in imitaterObj.GetComponentsInChildren<MonoBehaviour>())
        {
            Destroy(script);
        }
        foreach (var script in imitaterObj.GetComponentsInChildren<Rigidbody>())
        {
            Destroy(script);
        }

        imitater = imitaterObj.AddComponent<Imitater>();
        imitater.sourceTransform = go.transform;
        imitater.offset = another.transform.position - transform.position;

        return imitater;
    }

    /// <summary>
    /// 传送到另一边
    /// </summary>
    /// <param name="go"></param>
    void Transmission(GameObject go)
    {
        if (go.GetComponent<Rigidbody>() != null)
        {
            go.transform.position = go.transform.position + another.transform.position - transform.position;
            go.GetComponent<Rigidbody>().MovePosition(go.transform.position);
        }
        else
        {
            go.transform.position = go.transform.position + another.transform.position - transform.position;
        }

        imitater.offset = -another.transform.position + transform.position;
        another.imitater = imitater;
        another.player = player;

        imitater = null;
        player = null;
    }
}
