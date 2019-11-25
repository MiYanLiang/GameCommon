using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroDataControll : MonoBehaviour {
    
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

    private int battleNums;  //记录此卡牌战斗次数
    public int BattleNums { get => battleNums; set => battleNums = value; }

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
    string heroInformation;

    // Use this for initialization
    void Start () {
        heroidtest = int.Parse(HeroData[0]);
        heroName = HeroData[1];
        heroKindType = int.Parse(HeroData[3]);
        GetHeroTypeName();
        attack = HeroData[6];
        defense = HeroData[7];
        soldierNum = HeroData[8];
        heroInformation = HeroData[20];
    }

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
            gameObject.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = LoadJsonFile.SoldierTypeDates[int.Parse(HeroData[3])-1][2];
            //设置兵种背景
            gameObject.transform.GetComponent<Image>().sprite = Resources.Load("Image/ArmsPicture/"+HeroData[3],typeof(Sprite)) as Sprite;
        }
    }
    
    //获取点击当前卡牌武将的id
    public void GetThisCardId()
    {
        fetterInformation.Clear();
        GameObject.Find("TopInformationBar").GetComponentInChildren<Text>().text = "";
        int heroId = heroidtest;
        heroIdDate.Add(heroId.ToString());
        print("点击的"+heroId);
        fetterInformation=GameObject.Find("FettrrControl").GetComponent<FetterContronl>().init_One(heroIdDate);
        //显示点击英雄的名字  及  阶数（阶数尚未得到）
        GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[1].text = heroName+ "\u1500";
        //显示兵种及英雄相关属性
        //int nums = GameObject.Find("backGround").GetComponent<HeroIdChangeAndSave>().StatisticsHeroNums(heroId);
        GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[2].text = heroKindName + "\u2000" /*+ "拥有" + nums*/ + "\u2000" + "攻击" + attack + "\u2000" + "防御" + defense + "\u2000" + "士兵" + soldierNum;
        //显示羁绊内容
        if (fetterInformation.Count > 0)
        {
            ////传递给显示详细信息
            for (int j = 0; j < fetterInformation.Count; j++)
            {
                for (int i = 0; i < fetterInformation[j].Count; i++)
                {
                    GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[0].text += "[" + fetterInformation[j][1] + "]" + fetterInformation[j][9] + "同时上阵时," + "攻击+" + fetterInformation[j][3] + "%" +","+ "防御+" + fetterInformation[j][4] + "%"+"," + "士兵+" + fetterInformation[j][5] + "%" + "\t";
                    break;
                }
            }
        }
        else
        {
            GameObject.Find("TopInformationBar").GetComponentInChildren<Text>().text = "\u3000" + "此英雄无羁绊";
        }
        GameObject.Find("TopInformationBar").GetComponentsInChildren<Text>()[3].text = heroInformation;
        heroIdDate.Clear();
    }

    //获取兵种名字
    void GetHeroTypeName()
    {
        string name = "";
        if (heroKindType == 1)
        {
            name = "山兽";
            heroKindName = name;
        }
        else if (heroKindType == 2)
        {
            name = "海兽";
            heroKindName = name;
        }
        else if (heroKindType == 3)
        {
            name = "飞兽";
            heroKindName = name;
        }
        else if (heroKindType == 4)
        {
            name = "人杰";
            heroKindName = name;
        }
        else if (heroKindType == 5)
        {
            name = "祖巫";
            heroKindName = name;
        }
        else if (heroKindType == 6)
        {
            name = "散仙";
            heroKindName = name;
        }
        else if (heroKindType == 7)
        {
            name = "辅神";
            heroKindName = name;
        }
        else if (heroKindType == 8)
        {
            name = "魔神";
            heroKindName = name;
        }
        else if (heroKindType == 9)
        {
            name = "天神";
            heroKindName = name;
        }
        else if (heroKindType == 10)
        {
            name = "神兽";
            heroKindName = name;
        }
    }
}