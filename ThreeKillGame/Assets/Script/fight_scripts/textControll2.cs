using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textControll2 : MonoBehaviour
{

    private void Start()
    {
        Invoke("DestortThisText", FightControll.speedTime * 1.6f);  //销毁
    }

    /// <summary>
    /// 延迟销毁
    /// </summary>
    private void DestortThisText()
    {
        Destroy(transform.gameObject);
    }
}