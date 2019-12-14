using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroCardDrag : MonoBehaviour
{

    private Transform backGround;    //背景存放id数据
    private Transform beginParentTransform; //记录拖拽卡片的父级对象
    /// <summary>
    /// UI界面的顶层Canvas
    /// GameObject.Find("Canvas").transform;)
    /// </summary>
    private Transform canvas_Transform;
    private Transform jiuGongge_Transform;
    private Transform preparation_Transform;

    private float moveSpeed = 0.2f;

    private CreateAndUpdate createUpdate;

    // Use this for initialization
    void Start()
    {
        canvas_Transform = GameObject.Find("Canvas").transform;
        jiuGongge_Transform = GameObject.Find("JiuGongge").transform;
        preparation_Transform = GameObject.Find("Preparation").transform;
        backGround = canvas_Transform.GetChild(0);
        createUpdate = GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>();
    }

    /// <summary>
    /// 开始拖拽
    /// </summary>
    public void Begin(BaseEventData data)
    {
        if (transform.parent == canvas_Transform)
            return;
        //记录原始父级控件
        beginParentTransform = transform.parent;
        //设置临时父级控件为Canvas
        transform.SetParent(canvas_Transform);
        //transform.GetChild(0).gameObject.SetActive(false);
    }

    /// <summary>
    /// 拖动中
    /// </summary>
    /// <param name="uiEvent">UI事件数据</param>
    public void OnDrag(BaseEventData uiEvent)
    {
        transform.position = Input.mousePosition;
        //判断是否可以穿透Image
        if (transform.GetComponent<Image>().raycastTarget)
            transform.GetComponent<Image>().raycastTarget = false;
    }

    /// <summary>
    /// 结束时
    /// </summary>
    public void End(BaseEventData data)
    {
        PointerEventData _eventData = data as PointerEventData; //获取拖拽释放事件
        if (_eventData == null)
            return;

        GameObject go = _eventData.pointerCurrentRaycast.gameObject;    //释放时鼠标透过拖动的Image后的物体
        if (go.tag == "GridJ" || go.tag == "GridP")  //如果拖动卡牌下是：没有卡牌的格子时
        {
            //判断上阵位是否已满
            int battleNums = 0;
            if (go.tag == "GridJ")
            {
                for (int i = 0; i < jiuGongge_Transform.childCount; i++)
                {
                    if (jiuGongge_Transform.GetChild(i).childCount > 0)
                        battleNums++;
                }
            }
            else
            {
                for (int i = 0; i < preparation_Transform.childCount; i++)
                {
                    if (preparation_Transform.GetChild(i).childCount > 0)
                        battleNums++;
                }
            }
            if ((go.tag == "GridP" && battleNums >= CreateAndUpdate.prepareNum) || (go.tag == "GridJ" && battleNums >= CreateAndUpdate.battleNum))
            {
                if (go.tag == "GridJ")
                {
                    createUpdate.GoldNotEnough(LoadJsonFile.TipsTableDates[4][2]);
                    //Debug.Log("上阵位已满");
                }
                else
                {
                    createUpdate.GoldNotEnough(LoadJsonFile.TipsTableDates[5][2]);
                    //Debug.Log("备战位已满");
                }
                SetPosAndParent(transform, beginParentTransform);
                transform.GetComponent<Image>().raycastTarget = true;
            }
            else
            {
                //将原先的位置武将卡牌号存为0
                HeroIdChangeAndSave.pos_heroId[int.Parse(beginParentTransform.name)] = 0;
                SetPosAndParent(transform, go.transform);
                transform.GetComponent<Image>().raycastTarget = true;
                //将新的位置的武将卡牌号存为当前武将的id
                HeroIdChangeAndSave.pos_heroId[int.Parse(go.transform.name)] = int.Parse(transform.GetComponent<HeroDataControll>().HeroData[0]);
            }
        }
        else if (go.tag == "Card") //如果是其他卡牌
        {
            HeroIdChangeAndSave.pos_heroId[int.Parse(beginParentTransform.name)] = int.Parse(go.transform.GetComponent<HeroDataControll>().HeroData[0]);
            HeroIdChangeAndSave.pos_heroId[int.Parse(go.transform.parent.name)] = int.Parse(transform.GetComponent<HeroDataControll>().HeroData[0]);

            SetPosAndParent(transform, go.transform.parent);   //将当前拖动卡牌设置到目标位置
            go.transform.SetParent(canvas_Transform);          //目标位置的原卡牌父级设置到Canvas

            //以下执行置换两个英雄卡牌的动画，完成位置互换
            if (Math.Abs(go.transform.position.x - beginParentTransform.position.x) <= 0)
            {
                go.transform.DOMoveY(beginParentTransform.position.y, moveSpeed).OnComplete(() =>
                {
                    go.transform.SetParent(beginParentTransform);
                    transform.GetComponent<Image>().raycastTarget = true;
                }).SetEase(Ease.InOutQuint);
            }
            else
            {
                go.transform.DOMoveX(beginParentTransform.position.x, moveSpeed).OnComplete(() =>
                {
                    go.transform.DOMoveY(beginParentTransform.position.y, moveSpeed).OnComplete(() =>
                    {
                        go.transform.SetParent(beginParentTransform);
                        transform.GetComponent<Image>().raycastTarget = true;
                    }).SetEase(Ease.InOutQuint);
                });
            }
        }
        else //其他任何情况，回归原始位置
        {
            SetPosAndParent(transform, beginParentTransform);
            transform.GetComponent<Image>().raycastTarget = true;
        }

        string strid = "";
        for (int i = 0; i < HeroIdChangeAndSave.pos_heroId.Length; i++)
        {
            strid = strid + "  " + HeroIdChangeAndSave.pos_heroId[i];
        }
        //延时刷新上阵位等信息
        Invoke("UpdateOthersNumText", moveSpeed * 2.5f);
    }

    /// <summary>
    /// 刷新上阵位备战位等信息
    /// </summary>
    private void UpdateOthersNumText()
    {
        GameObject.Find("ArrayText").GetComponent<UIDynamicDisplay>().ChangeNumOfPeople();
        GameObject.Find("ReadyText").GetComponent<UIDynamicDisplay>().ChangeNumOfPeople();
        backGround.GetComponent<HeroIdChangeAndSave>().SaveNowHeroId(); //刷新保存当前拥有的武将id
        backGround.GetComponent<HeroIdChangeAndSave>().GetNowPlayerPowerNums();
    }

    /// <summary>
    /// 设置父物体，拖动的UI位置归正
    /// </summary>
    /// <param name="t">对象Transform</param>
    /// <param name="parent">要设置到的父级</param>
    private void SetPosAndParent(Transform t, Transform parent)
    {
        t.SetParent(parent);
        t.position = parent.position;
    }

    //调用背景图代码恢复选择框
    public void RestoreUnSelect()
    {
        GameObject backGround = GameObject.Find("backGround");
        backGround.GetComponent<HeroIdChangeAndSave>().RestoreCardUnSelect(transform, int.Parse(transform.parent.name));
    }
}