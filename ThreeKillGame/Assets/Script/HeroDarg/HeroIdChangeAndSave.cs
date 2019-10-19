using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroIdChangeAndSave : MonoBehaviour
{
    public static int[] pos_heroId = new int[16];     //记录九宫格和备战区的英雄id

    public Transform JiuGongGe; //九宫格
    public Transform BeiZhanWei;//备战位

    public List<string> fightIdList = new List<string>();   //上阵英雄id
    public List<string> allIdList = new List<string>();     //全部英雄id

    private void Awake()
    {
        for (int i = 0; i < pos_heroId.Length; i++)
        {
            pos_heroId[i] = 0;
        }
    }
    /// <summary>
    /// 刷新保存当前拥有的武将id
    /// </summary>
    public void SaveNowHeroId()
    {
        fightIdList.Clear();
        allIdList.Clear();
        for (int i = 0; i < 9; i++)
        {
            if (JiuGongGe.GetChild(i).childCount>0)
            {
                fightIdList.Add(JiuGongGe.GetChild(i).GetChild(0).GetComponent<HeroDataControll>().HeroData[0]);
                allIdList.Add(JiuGongGe.GetChild(i).GetChild(0).GetComponent<HeroDataControll>().HeroData[0]);
            }
        }
        for (int i = 0; i < 7; i++)
        {
            if (BeiZhanWei.GetChild(i).childCount > 0)
            {
                allIdList.Add(BeiZhanWei.GetChild(i).GetChild(0).GetComponent<HeroDataControll>().HeroData[0]);
            }
        }
    }

}
