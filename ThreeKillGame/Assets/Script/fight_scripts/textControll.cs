using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textControll : MonoBehaviour
{
    //[SerializeField]
    private float multiple = 2f;

    private void OnEnable()
    {
        Invoke("HideWidget", FightControll.speedTime * multiple);
    }

    /// <summary>
    /// 隐藏自身
    /// </summary>
    private void HideWidget()
    {
        gameObject.SetActive(false);
    }
}