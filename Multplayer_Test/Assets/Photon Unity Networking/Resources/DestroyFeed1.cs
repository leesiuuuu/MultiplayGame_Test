using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFeed1 : MonoBehaviour
{
    public float DestroyTime = 4f;

    private void OnEnable()
    {
        Destroy(gameObject, DestroyTime);
    }
}
