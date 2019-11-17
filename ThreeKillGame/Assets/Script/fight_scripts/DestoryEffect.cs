using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryEffect : MonoBehaviour
{
    public float destoryTime = 0.5f;

    private void Start()
    {
        Invoke("DistoryGameObject", destoryTime);
    }

    private void DistoryGameObject()
    {
        Destroy(gameObject);
    }
}
