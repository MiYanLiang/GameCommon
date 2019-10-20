using OfficeOpenXml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroDataControll : MonoBehaviour {

    //[HideInInspector]
    public List<string> HeroData = new List<string>(); //存储当前武将的数据
    List<string> heroIdDate = new List<string>();
    //public List<string> HeroData { get => heroData; set => heroData = value; }
    //index	    roleName	所属势力    soldierKind	  rarity  recruitingMoney  attack    defense	soldierNum	    闪避率	
    //0         1           2           3             4       5                6         7          8               9       
    //暴击率	暴击伤害	重击率	    重击伤害	  破甲    装备id           典故id	 武器技id	强化兵种技id	羁绊技id
    //10        11          12          13            14      15               16        17         18              19


    private int grade_hero;  //记录卡牌品阶
    public int Grade_hero { get => grade_hero; set => grade_hero = value; }
    List<List<string>> fetterInformation = new List<List<string>>();


    // Use this for initialization
    void Start () {
        //print(HeroData[0] + ":" + HeroData[1]);
    }

    /// <summary>
    /// 显示英雄名等数据信息
    /// </summary>
    public void ShowThisHeroData()
    {
        if (HeroData != null)
        {
            //Debug.Log("//传递成功--"+heroData[1]);
            //显示英雄名等信息
            gameObject.transform.GetChild(0).GetComponent<Text>().text = HeroData[0]+":"+HeroData[1];
            gameObject.transform.GetChild(1).GetComponent<Text>().text = HeroData[6];
            gameObject.transform.GetChild(2).GetComponent<Text>().text = HeroData[7];
        }
    }

    //获取点击当前卡牌武将的id
    public void GetThisCardId()
    {
        GameObject.Find("TopInformationBar").GetComponentInChildren<Text>().text = "";
        int heroId = int.Parse(HeroData[0]);
        heroIdDate.Add(heroId.ToString());
        fetterInformation=GameObject.Find("FettrrControl").GetComponent<FetterContronl>().init_One(heroIdDate);
        if (fetterInformation.Count > 0)
        {
            ////传递给显示详细信息
            for (int j = 0; j < fetterInformation.Count; j++)
            {
                for (int i = 0; i < fetterInformation[j].Count; i++)
                {
                    print(fetterInformation[j][i]);
                    GameObject.Find("TopInformationBar").GetComponentInChildren<Text>().text += "\u3000" + "[" + fetterInformation[j][1] + "]" + fetterInformation[j][9] + "同时上阵," + "攻击+" + fetterInformation[j][3] + "%" + "防御+" + fetterInformation[j][4] + "%" + "士兵+" + fetterInformation[j][5] + "%" + "\t";
                    break;
                }
            }
        }
        else
        {
            GameObject.Find("TopInformationBar").GetComponentInChildren<Text>().text = "\u3000" + "此英雄无羁绊";
        }
        heroIdDate.Clear();
    }
}
