using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class posResFlash : MonoBehaviour
{
    public Transform posRes;    //临时记录位置
    public Transform Shangzhenwei;
    public Transform canvas;
    public Transform[] fights;  //战斗画面
    public Transform[] jiugongges;//九宫格位置
    public Transform[] playerCanvas;    //玩家武将专属界面
    public GameObject UpdateBtn;    //刷新招募按钮
    private bool isLockUpdateCard;  //记录是否对刷新招募上锁

    CreateAndUpdate createAndUpdate;

    private void Start()
    {
        isLockUpdateCard = false;
        ChangePosShow(0);//恢复战斗和备战位显示玩家自身
        createAndUpdate = UpdateBtn.GetComponent<CreateAndUpdate>();
    }

    //改变上锁状态
    public void ChangeLockState(bool boo)
    {
        isLockUpdateCard = boo;
    }

    //对是否上锁执行是否刷新招募
    public void UpdateLockOfRecruit()
    {
        if (isLockUpdateCard)
        {
            return;
        }
        else
        {
            createAndUpdate.cutMoney = 0; //每局结束的刷新招募不减金币
            createAndUpdate.UpdateCard();
            createAndUpdate.cutMoney = 0;
            createAndUpdate.updateMoneyText.text = createAndUpdate.cutMoney.ToString();
        }
    }

    //顶部势力按钮执行，优先显示界面
    public void ChangePosShow(int n)
    {
        //fights[n].SetParent(posRes);
        //fights[n].SetParent(canvas);
        //jiugongges[n].SetParent(canvas);
        //jiugongges[n].SetParent(Shangzhenwei);
        //if (n == 0) //关闭或显示备战位等
        //{
        //    for (int i = 0; i < playerCanvas.Length; i++)
        //    {
        //        playerCanvas[i].gameObject.SetActive(true);
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < playerCanvas.Length; i++)
        //    {
        //        playerCanvas[i].gameObject.SetActive(false);
        //    }
        //}
    }
}