using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class posResFlash : MonoBehaviour
{
    public Transform posRes;    //临时记录位置
    public Transform Shangzhenwei;
    public Transform canvas;
    public Transform[] fights;  //战斗画面
    public Transform[] jiugongges;//九宫格位置

    //顶部势力按钮执行，优先显示界面
    public void ChangePosShow(int n)
    {
        fights[n].SetParent(posRes);
        fights[n].SetParent(canvas);
        jiugongges[n].SetParent(canvas);
        jiugongges[n].SetParent(Shangzhenwei);
    }
}
