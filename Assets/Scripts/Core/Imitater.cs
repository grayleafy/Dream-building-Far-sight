using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imitater : MonoBehaviour
{
    public Transform sourceTransform;
    public Vector3 offset;

    private void LateUpdate()
    {

        Follow(transform, sourceTransform);

    }

    void Follow(Transform self, Transform source)
    {
        self.position = source.position + offset;
        self.rotation = source.rotation;
        for (int i = 0; i < source.childCount; i++)
        {
            Follow(self.GetChild(i), source.GetChild(i));
        }
    }
}
