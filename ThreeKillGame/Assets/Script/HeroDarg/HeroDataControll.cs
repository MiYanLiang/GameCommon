using OfficeOpenXml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroDataControll : MonoBehaviour {

    //[HideInInspector]
    public List<string> HeroData = new List<string>(); //存储当前武将的数据
    List<string> heroIdDate = new List<string>();
    List<string> HeroDataTest = new List<string>();
    //public List<string> HeroData { get => heroData; set => heroData = value; }
    //index	    roleName	所属势力    soldierKind	  rarity  recruitingMoney  attack    defense	soldierNum	    闪避率	
    //0         1           2           3             4       5                6         7          8               9       
    //暴击率	暴击伤害	重击率	    重击伤害	  破甲    装备id           典故id	 武器技id	强化兵种技id	羁绊技id
    //10        11          12          13            14      15               16        17         18              19


    private int grade_hero;  //记录卡牌品阶
    public int Grade_hero { get => grade_hero; set => grade_hero = value; }

    private int price_hero; //记录此卡牌出售的价格
    public int Price_hero { get => price_hero; set => price_hero = value; }

    List<List<string>> fetterInformation = new List<List<string>>();

    int heroidtest;
    string heroName;
    int heroKindType;
    string heroKindName;
    string attack;
    string defense;
    string soldierNum;

    // Use this for initialization
    void Start () {
        
        print(HeroData[0] + ":" + HeroData[1]);
        heroidtest = int.Parse(HeroData[0]);
        heroName = HeroData[1];
        heroKindType = int.Parse(HeroData[3]);
        GetHeroTypeName();
        attack = HeroData[6];
        defense = HeroData[7];
        soldierNum = HeroData[8];
        //HeroDataTest = HeroData;
        //for (int i = 0; i < HeroDataTest.Count; i++)
        //{
        //    print(HeroDataTest[i]);
        //}
    }

    /// <summary>
    /// 显示英雄名等数据信息
    /// </summary>
    public void ShowThisHeroData()
    {
        //if (HeroDataTest != null)
        //{
        //    //Debug.Log("//传递成功--"+heroData[1]);
        //    //显示英雄名等信息
        //    gameObject.transform.GetChild(0).GetComponent<Text>().text = HeroDataTest[0] + ":" + HeroDataTest[1];
        //    gameObject.transform.GetChild(1).GetComponent<Text>().text = HeroDataTest[6];
        //    gameObject.transform.GetChild(2).GetComponent<Text>().text = HeroDataTest[7];
        //}
        if (HeroData != null)
        {
            //Debug.Log("//传递成功--"+heroData[1]);
            //显示英雄名等信息
            gameObject.transform.GetChild(0).GetComponent<Text>().text = HeroData[0] + ":" + HeroData[1];
            gameObject.transform.GetChild(1).GetComponent<Text>().text = HeroData[6];
            gameObject.transform.GetChild(2).GetComponent<Text>().text = HeroData[7];
        }
    }

    //获取点击当前卡牌武将的id
    public void GetThisCardId()
    {
        
        GameObject.Find("TopInformationBar").GetComponentInChildren<Text>().text = "";
        int heroId = heroidtest;
        heroIdDate.Add(heroId.ToString());
        print("点击的"+heroId);
        fetterInformation=GameObject.Find("FettrrControl").GetComponent<FetterContronl>().init_One(heroIdDate);
        //显示点击英雄的名字  及  阶数（阶数尚未得到）
        GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[1].text = heroName+ "\u1500"+"1"+"阶";
        //显示兵种及英雄相关属性  （缺少拥有个数）
        GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[2].text = heroKindName + "\u2000" + "拥有" + "1" + "\u2000" + "攻击" + attack + "\u2000" + "防御" + defense + "\u2000" + "士兵" + soldierNum;
        //显示羁绊内容
        if (fetterInformation.Count > 0)
        {
            ////传递给显示详细信息
            for (int j = 0; j < fetterInformation.Count; j++)
            {
                for (int i = 0; i < fetterInformation[j].Count; i++)
                {
                    //print(fetterInformation[j][i]);
                    GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += "\u3000" + "[" + fetterInformation[j][1] + "]" + fetterInformation[j][9] + "同时上阵时," + "攻击+" + fetterInformation[j][3] + "%" +","+ "防御+" + fetterInformation[j][4] + "%"+"," + "士兵+" + fetterInformation[j][5] + "%" + "\t";
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

    //获取兵种名字
    void GetHeroTypeName()
    {
        string name = "";
        if (heroKindType == 1)
        {
            name = "盾兵";
            heroKindName = name;
        }
        else if (heroKindType == 2)
        {
            name = "象兵";
            heroKindName = name;
        }
        else if (heroKindType == 3)
        {
            name = "戟兵";
            heroKindName = name;
        }
        else if (heroKindType == 4)
        {
            name = "禁卫";
            heroKindName = name;
        }
        else if (heroKindType == 5)
        {
            name = "枪兵";
            heroKindName = name;
        }
        else if (heroKindType == 6)
        {
            name = "骑兵";
            heroKindName = name;
        }
        else if (heroKindType == 7)
        {
            name = "军师";
            heroKindName = name;
        }
        else if (heroKindType == 8)
        {
            name = "工兵";
            heroKindName = name;
        }
        else if (heroKindType == 9)
        {
            name = "方士";
            heroKindName = name;
        }
        else if (heroKindType == 10)
        {
            name = "神兽";
            heroKindName = name;
        }
    }
}
