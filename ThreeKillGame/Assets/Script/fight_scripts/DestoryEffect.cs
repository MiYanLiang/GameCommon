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