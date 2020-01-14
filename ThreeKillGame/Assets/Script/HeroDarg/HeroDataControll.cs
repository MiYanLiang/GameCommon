using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroDataControll : MonoBehaviour
{

    public List<string> HeroData = new List<string>(); //存储当前武将的数据
    List<string> heroIdDate = new List<string>();
    List<string> HeroDataTest = new List<string>();
    //public List<string> HeroData { get => heroData; set => heroData = value; }
    //index	    roleName	所属势力    soldierKind	  rarity  recruitingMoney  attack    defense	soldierNum	    闪避率	
    //0         1           2           3             4       5                6         7          8               9       
    //暴击率	暴击伤害	重击率	    重击伤害	  破甲    装备id           典故id	 武器技id	强化兵种技id	羁绊技id  roleIntro	 IsHero	 roleIcon     addHpPercent
    //10        11          12          13            14      15               16        17         18              19          20          21      22      23

    private int grade_hero;  //记录卡牌品阶
    public int Grade_hero { get => grade_hero; set => grade_hero = value; }

    private int battleNums;  //记录此卡牌战斗次数
    public int BattleNums { get => battleNums; set => battleNums = value; }

    private int price_hero; //记录此卡牌出售的价格
    public int Price_hero { get => price_hero; set => price_hero = value; }

    List<List<string>> fetterInformation = new List<List<string>>();

    /// <summary>
    /// 显示英雄名等数据信息
    /// </summary>
    public void ShowThisHeroData()
    {
        if (HeroData != null)
        {
            gameObject.transform.GetChild(0).GetComponent<Text>().text = HeroData[1];
            gameObject.transform.GetChild(1).GetComponent<Text>().text = HeroData[6];
            //设置左上角兵种文字
            gameObject.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = LoadJsonFile.SoldierTypeDates[int.Parse(HeroData[3]) - 1][2];
            //设置武将背景
            gameObject.transform.GetComponent<Image>().sprite = Resources.Load("Image/RoleIcon/" + HeroData[22], typeof(Sprite)) as Sprite;
        }
    }

    //获取点击当前卡牌武将的id
    public void GetThisCardId()
    {
        GameObject topBar = GameObject.Find("TopInformationBar");
        fetterInformation.Clear();
        topBar.GetComponentInChildren<Text>().text = "";
        int heroId = int.Parse(HeroData[0]);
        heroIdDate.Add(heroId.ToString());
        print("点击的" + heroId);
        fetterInformation = GameObject.Find("FettrrControl").GetComponent<FetterContronl>().init_One(heroIdDate);
        //显示点击英雄的名字  及  阶数
        topBar.GetComponentsInChildren<Text>()[1].fontSize = HeroData[1].Length > 2 ? 50 : 70;
        topBar.GetComponentsInChildren<Text>()[1].text = HeroData[1] + "\u1500" /*+ Grade_hero*/;
        //显示兵种及英雄相关属性
        topBar.GetComponentsInChildren<Text>()[2].text = GetHeroTypeName(int.Parse(HeroData[3])) + "\u2000" + "攻击" + HeroData[6] + "\u2000" + "防御" + HeroData[7] + "\u2000" + "士兵" + HeroData[8];
        //显示英雄简介
        topBar.GetComponentsInChildren<Text>()[3].text = HeroData[20];
        //显示羁绊内容
        //if (fetterInformation.Count > 0)
        //{
        //    ////传递给显示详细信息
        //    for (int j = 0; j < fetterInformation.Count; j++)
        //    {
        //        for (int i = 0; i < fetterInformation[j].Count; i++)
        //        {
        //            topBar.GetComponentsInChildren<Text>()[0].text += "[" + fetterInformation[j][1] + "]" + fetterInformation[j][9] + "同时上阵时," + "攻击+" + fetterInformation[j][3] + "%" +","+ "防御+" + fetterInformation[j][4] + "%"+"," + "士兵+" + fetterInformation[j][5] + "%" + "\t";
        //            break;
        //        }
        //    }
        //}
        //else
        //{
        //    topBar.GetComponentInChildren<Text>().text = "\u3000" + "此英雄无羁绊";
        //}
        heroIdDate.Clear();
    }

    //获取兵种名字
    private string GetHeroTypeName(int kindType)
    {
        return LoadJsonFile.SoldierTypeDates[kindType - 1][1];
    }
}