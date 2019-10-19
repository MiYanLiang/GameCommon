using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class posResFlash : MonoBehaviour
{
    public Transform posRes;    //临时记录位置
    public Transform canvas;
    public Transform[] fights;

    //顶部势力按钮执行，优先显示界面
    public void ChangePosShow(int n)
    {
        fights[n].SetParent(posRes);
        fights[n].SetParent(canvas);
    }
}
