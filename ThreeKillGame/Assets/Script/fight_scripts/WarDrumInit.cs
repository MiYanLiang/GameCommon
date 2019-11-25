using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarDrumInit : MonoBehaviour
{
    [SerializeField]
    Button[] drumBtns;  //战鼓按钮
    private int nowLevel = 1;

    private void OnEnable()
    {
        nowLevel = CreateAndUpdate.level;
        InitDrumBtn(nowLevel);
    }

    /// <summary>
    /// 初始化战鼓状态
    /// </summary>
    /// <param name="nowLevel"></param>
    private void InitDrumBtn(int nowLevel)
    {
        for (int i = 0; i < drumBtns.Length; i++)
        {
            if (nowLevel >= int.Parse(LoadJsonFile.WarDrumTableDates[i][2]))    //当前等级大于等于解锁等级
            {
                drumBtns[i].transform.GetChild(1).gameObject.SetActive(false);
                drumBtns[i].interactable = true;
            }
            else
            {
                drumBtns[i].transform.GetChild(1).gameObject.SetActive(true);
                drumBtns[i].transform.GetChild(1).GetComponent<Text>().text = LoadJsonFile.WarDrumTableDates[i][2] + "级解锁";
                drumBtns[i].interactable = false;
            }
        }
    }
}