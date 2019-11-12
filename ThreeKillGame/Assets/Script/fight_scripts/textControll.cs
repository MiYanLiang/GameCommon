using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textControll : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("HideWidget", FightControll.speedTime * 1.5f);
    }

    /// <summary>
    /// 隐藏自身
    /// </summary>
    private void HideWidget()
    {
        gameObject.SetActive(false);
    }
}
