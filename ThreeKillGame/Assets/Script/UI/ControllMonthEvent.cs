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

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        //monthEventObj = Resources.Load("Prefab/Force/monthEvent", typeof(GameObject)) as GameObject;
        monthIndex = 1;
    }

    /// <summary>
    /// 添加每月的事件到大地图显示
    /// </summary>
    /// <param name="isNextYear"></param>
    /// <param name="contant"></param>
    public void AddShowMonthEvent(bool isNextYear, string contant)
    {
        UpdateMonthData(isNextYear);
        GameObject monthObj = Instantiate(monthEventObj, sanguoTVContantTran);
        monthObj.transform.GetComponentsInChildren<Text>()[0].text = monthIndex+"月";
        monthObj.transform.GetComponentsInChildren<Text>()[1].text = contant;
    }

    //更新月份
    private void UpdateMonthData(bool isNextYear)
    {
        if (isNextYear)
        {
            monthIndex = 1;
        }
        else
        {
            if (monthIndex<12)
            {
                monthIndex++;
            }
            //monthIndex += Random.Range(0, 4);
        }
    }
}
