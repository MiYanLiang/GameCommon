using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroCardDrag : MonoBehaviour {
    
    private Transform backGround;    //背景存放id数据
    private Transform beginParentTransform; //记录拖拽卡片的父级对象
    /// <summary>
    /// UI界面的顶层Canvas
    /// GameObject.Find("Canvas").transform;)
    /// </summary>
    private Transform canvas_Transform;

	// Use this for initialization
	void Start () {
        canvas_Transform = GameObject.Find("Canvas").transform;
        backGround = canvas_Transform.GetChild(0);
    }
    private void Update()
    {
        //print("pppppppppppppppppppp" + transform.GetComponent<HeroDataControll>().HeroData[0]);
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
        if (go.tag == "Grid")  //如果拖动卡牌下是：没有卡牌的格子时
        {
            //将原先的位置武将卡牌号存为0
            HeroIdChangeAndSave.pos_heroId[int.Parse(beginParentTransform.name)] = 0;
            SetPosAndParent(transform, go.transform);
            transform.GetComponent<Image>().raycastTarget = true;
            //将新的位置的武将卡牌号存为当前武将的id
            HeroIdChangeAndSave.pos_heroId[int.Parse(go.transform.name)] = int.Parse(transform.GetComponent<HeroDataControll>().HeroData[0]);
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
                go.transform.DOMoveY(beginParentTransform.position.y, 0.3f).OnComplete(() =>
                {
                    go.transform.SetParent(beginParentTransform);
                    transform.GetComponent<Image>().raycastTarget = true;
                }).SetEase(Ease.InOutQuint);
            }
            else
            {
                go.transform.DOMoveX(beginParentTransform.position.x, 0.2f).OnComplete(() =>
                {
                    go.transform.DOMoveY(beginParentTransform.position.y, 0.3f).OnComplete(() =>
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
        //transform.GetChild(0).gameObject.SetActive(true);
        GameObject.Find("ArrayText").GetComponent<UIDynamicDisplay>().ChangeNumOfPeople();
        GameObject.Find("ReadyText").GetComponent<UIDynamicDisplay>().ChangeNumOfPeople();

        string strid = "";
        for (int i = 0; i < HeroIdChangeAndSave.pos_heroId.Length; i++)
        {
            strid = strid + "  " + HeroIdChangeAndSave.pos_heroId[i];
        }

        backGround.GetComponent<HeroIdChangeAndSave>().SaveNowHeroId(); //刷新保存当前拥有的武将id

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
        backGround.GetComponent<HeroIdChangeAndSave>().RestoreCardUnSelect(transform);
    }

}
