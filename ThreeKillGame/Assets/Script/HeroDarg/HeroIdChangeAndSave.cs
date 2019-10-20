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

    List<int> allIdList_int = new List<int>();     //全部英雄id
    List<int> fightIdList_int = new List<int>();   //上阵英雄id

    List<string> heroTypeName = new List<string>();
    List<string> skillInformation = new List<string>();//激活的技能详细信息
    List<List<string>> fetterInformation = new List<List<string>>(); //激活羁绊的详细信息

    List<string> arrayGo = new List<string>() { "19", "89", "104", "67", "68" };  //上阵英雄数组,用以测试
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
        heroTypeName.Clear();
        skillInformation.Clear();
        fightIdList.Clear();
        allIdList.Clear();
        allIdList_int.Clear();
        fightIdList_int.Clear();
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
        for (int i = 0; i < fightIdList.Count; i++)
        {
            fightIdList_int.Add(int.Parse(fightIdList[i]));
        }
        for (int i = 0; i < allIdList.Count; i++)
        {
            allIdList_int.Add(int.Parse(allIdList[i]));
        }
        //GameObject.Find("FettrrControl").GetComponent<FetterContronl>().init_Go(fightIdList);//羁绊信息
        GameObject.Find("FettrrControl").GetComponent<FetterContronl>().init_Go(arrayGo);//羁绊信息
        fetterInformation= GameObject.Find("FettrrControl").GetComponent<FetterContronl>().fetterInformation1;
        for (int j = 0; j < fetterInformation.Count; j++)
        {
            for (int i = 0; i < fetterInformation[j].Count; i++)
            {
                //print("ss");
                print(fetterInformation[j][i]);
            }
        }

        heroTypeName =GameObject.Find("SoldiersControl").GetComponent<SoldiersControl>().init(allIdList_int);//初始兵种信息
        skillInformation=GameObject.Find("SoldiersControl").GetComponent<SoldiersControl>().init_up(fightIdList_int);//激活技能名称
        for (int i = 0; i < heroTypeName.Count; i++)
        {
            print(heroTypeName[i]);
        }
        for (int i = 0; i < skillInformation.Count; i++)
        {
            print(skillInformation[i]);
        }

        fetterInformation.Clear();
    }
    private void OnServerInitialized()
    {
       
    }




}
