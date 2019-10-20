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
    public Transform[] playerCanvas;    //玩家武将专属界面

    private void Start()
    {
        ChangePosShow(0);//恢复战斗和备战位显示玩家自身
    }


    //顶部势力按钮执行，优先显示界面
    public void ChangePosShow(int n)
    {
        fights[n].SetParent(posRes);
        fights[n].SetParent(canvas);
        jiugongges[n].SetParent(canvas);
        jiugongges[n].SetParent(Shangzhenwei);
        if (n == 0) //关闭或显示备战位等
        {
            for (int i = 0; i < playerCanvas.Length; i++)
            {
                playerCanvas[i].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < playerCanvas.Length; i++)
            {
                playerCanvas[i].gameObject.SetActive(false);
            }
        }
    }
}
