using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllMonthEvent : MonoBehaviour
{
    public static ControllMonthEvent instance;

    [SerializeField]
    Transform sanguoTVContantTran;
    [SerializeField]
    GameObject monthEventObj;

    private int monthIndex;
    [SerializeField]
    private float playTextSpeed;

    // Start is called before the first frame update
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        monthIndex = 1;
        playTextSpeed = 1f;
    }

    /// <summary>
    /// 添加每月的事件到大地图显示
    /// </summary>
    /// <param name="isNextYear"></param>
    /// <param name="contant"></param>
    public void AddShowMonthEvent(string contant)
    {
        GameObject monthObj = Instantiate(monthEventObj, sanguoTVContantTran);
        monthObj.transform.GetComponentsInChildren<Text>()[0].text = monthIndex + "月";
        ShowTextForMonthEvent(monthObj.transform.GetComponentsInChildren<Text>()[1], contant);

        monthIndex++;
        if (monthIndex>12)
        {
            UpdateMonthData();
        }
    }

    //更新月份
    public void UpdateMonthData()
    {
        monthIndex = 1;
    }

    private void ShowTextForMonthEvent(Text eventText, string data)
    {
        eventText.text = "";
        eventText.DOText(data, playTextSpeed).SetEase(Ease.Linear).SetAutoKill(false);
    }


}
