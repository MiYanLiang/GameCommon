using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateAndUpdate : MonoBehaviour
{
    [SerializeField]
    GameObject backGround;
    [SerializeField]
    GameObject GoldBoxObj;
    List<int> heroId = new List<int>();
    [HideInInspector]
    public List<int> myCard = new List<int>();
    [HideInInspector]
    public List<string> getCard = new List<string>();
    [HideInInspector]
    public List<int> getCardId = new List<int>();
    [HideInInspector]
    public List<int> sendCardId = new List<int>();
    [HideInInspector]
    public List<int> ChickenRibsHeroId = new List<int>();
    List<int> playerCardAll = new List<int>();
    List<int> player1Card = new List<int>();
    List<int> player2Card = new List<int>();
    List<int> player3Card = new List<int>();
    List<int> player4Card = new List<int>();
    List<int> player5Card = new List<int>();
    public List<GameObject> heroBtn = new List<GameObject>();
    List<int> excelText = new List<int>();
    List<string> heroName = new List<string>();  //招募英雄名字
    List<string> soliderKind = new List<string>(); //招募英雄种类
    List<string> soliderAttack = new List<string>();//招募英雄攻击
    List<string> soliderDefense = new List<string>();//招募英雄防御
    List<string> soliderForce = new List<string>();//招募英雄势力
    List<string> soliderMoney = new List<string>();//招募英雄花费
    List<string> soliderRarity = new List<string>();//招募英雄稀有度
    public List<string> allIdList = new List<string>();     //全部英雄id
    List<string> heroNum = new List<string>();//招募显示英雄数量
    //各色卡牌合集
    List<int> greenCard = new List<int>();
    List<int> blueCard = new List<int>();
    List<int> purpleCard = new List<int>();
    List<int> orangeCard = new List<int>();
    List<int> greenHeroId = new List<int>();
    List<int> blueHeroId = new List<int>();
    List<int> purpleHeroId = new List<int>();
    List<int> orangeHeroId = new List<int>();
    int count;
    int player_count;
    int temp_num;
    int player_temp_num;
    public int playerLevel = 1;
    int attack;
    int peopleHearts_blue;
    int peopleHearts_purple;
    int peopleHearts_orange;
    int peopleHearts_red;

    public static int peopleHearts; //民心
    public int moraleNum;           //士气
    public static int playerHp;     //血量
    public static int money;        //金币
    public static int level;        //等级
    public static int experience;   //经验
    public static int battleNum;    //上阵位
    public static int prepareNum;   //备战位
    public GameObject GoldBox_;

    int damageAll;

    [SerializeField]
    Text text_level;    //等级显示
    [SerializeField]
    Text text_level_fight;  //战斗中等级显示
    [SerializeField]
    Slider player_hp;//玩家血条
    [SerializeField]
    Text textHertNum;   //招募中民心的显示
    
    [SerializeField]
    private Transform prompTran;    //提示文本框
    [SerializeField]
    private GameObject tipsText;    //提示text预制件

    [SerializeField]
    int debug_Money;

    private int difficultNum;   //难度index

    /// <summary>
    /// 战斗结束后增加金币和经验
    /// </summary>
    public void AddMoneyAndExp()
    {
        experience++;
        if (level < LoadJsonFile.levelTableDatas.Count && experience >= int.Parse(LoadJsonFile.levelTableDatas[level][2]))
        {
            level++;
            if (prompTran.childCount < 1)
            {
                GameObject tipObj = Instantiate(tipsText, prompTran);
                if (level < 10)
                {
                    tipObj.GetComponent<Text>().text = string.Format(LoadJsonFile.TipsTableDates[2][2], level);  //升级提示
                }
                else
                {
                    tipObj.GetComponent<Text>().text = string.Format(LoadJsonFile.TipsTableDates[8][2], level);  //满级提示
                }
            }
            experience = 0;
            SetMaxBatAndPre();
        }
    }

    [SerializeField]
    Text goldOfGrade;   //升级所需金币显示
    public void UpdateGoldOfGrade()
    {
        if (level<10)
        {
            goldOfGrade.text = (int.Parse(LoadJsonFile.levelTableDatas[level][2]) - experience).ToString();
        }
    }

    /// <summary>
    /// 更新等级牌
    /// </summary>
    public void ChangeLevelText()
    {
        text_level.text = level + "级";
        text_level_fight.text = level + "级";
        if (level == 10)
        {
            goldOfGrade.transform.parent.GetChild(0).GetComponent<Text>().text = "满级";
            goldOfGrade.transform.parent.GetChild(1).gameObject.SetActive(false);
            goldOfGrade.transform.parent.GetChild(2).gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 使用金币进行升级
    /// </summary>
    public void BuyExpToLevel()
    {
        if (level >= 10)
        {
            GameObject tipObj = Instantiate(tipsText, prompTran);
            tipObj.GetComponent<Text>().text = string.Format(LoadJsonFile.TipsTableDates[8][2], level);  //满级提示
            return;
        }
        if (money >= (int.Parse(LoadJsonFile.levelTableDatas[level][2]) - experience))
        {
            money -= (int.Parse(LoadJsonFile.levelTableDatas[level][2]) - experience);
            level++;
            if (prompTran.childCount < 1)
            {
                GameObject tipObj = Instantiate(tipsText, prompTran);
                if (level < 10)
                {
                    tipObj.GetComponent<Text>().text = string.Format(LoadJsonFile.TipsTableDates[2][2], level);  //升级提示
                }
                else
                {
                    tipObj.GetComponent<Text>().text = string.Format(LoadJsonFile.TipsTableDates[8][2], level);  //满级提示
                }
            }
            experience = 0;
            SetMaxBatAndPre();
            ChangeLevelText();
            //Debug.Log("使用金币升级");
        }
        else
        {
            GoldNotEnough(LoadJsonFile.TipsTableDates[3][2]);
            //Debug.Log("金币不够");
        }
    }

    private void Awake()
    {
        level = 1;
        text_level.text = level + "级";
        text_level_fight.text = level + "级";
        experience = 0;
        difficultNum = PlayerPrefs.GetInt("DifficultyType");
    }

    void Start()
    {
        peopleHearts = int.Parse(LoadJsonFile.difficultyChooseDatas[difficultNum - 1][4]);
        money = int.Parse(LoadJsonFile.difficultyChooseDatas[difficultNum - 1][3]);
        money += debug_Money;
        UpdateGoldOfGrade();
        SetMaxBatAndPre();  //设置最大备战位和上阵位
        SetPeopleHarets();
        getCardId.Clear();
        count = 5;
        player_count = 25;
        GetExcelFile_Nature();
        GetExcelFile1();
        RandomList();
        PlayerRandom();
        GetExcelFile();
        HeroLocation();
        //玩家血量设置
        playerHp = int.Parse(LoadJsonFile.difficultyChooseDatas[difficultNum - 1][2]);
        player_hp.value = playerHp / float.Parse(LoadJsonFile.difficultyChooseDatas[difficultNum - 1][2]);
        player_hp.transform.GetChild(3).GetComponent<Text>().text = CreateAndUpdate.playerHp.ToString();
    }

    /// <summary>
    /// 设置最大备战位和上阵位
    /// </summary>
    private static void SetMaxBatAndPre()
    {
        battleNum = int.Parse(LoadJsonFile.levelTableDatas[level - 1][4]);  //设置最大上阵位
        prepareNum = int.Parse(LoadJsonFile.levelTableDatas[level - 1][5]); //设置最大备战位
    }
    
    void GetExcelFile()
    {
        excelText.Clear();
        for (int i = 0; i < myCard.Count; i++)
        {
            GetValueFromId(int.Parse(myCard[i].ToString()));
        }
        for (int i = 0; i < excelText.Count; i++)
        {
            GetSpecificValue(excelText[i], "roleName");
            GetSpecificValue(excelText[i], "recruitingMoney");
            GetSpecificValue(excelText[i], "soliderDefense");
            GetSpecificValue(excelText[i], "soliderAttack");
            GetSpecificValue(excelText[i], "soliderKind");
            GetSpecificValue(excelText[i], "soliderForce");
            GetSpecificValue(excelText[i], "soliderRarity");
        }
    }

    //读取获得各种稀有度英雄
    void GetExcelFile1()
    {
        excelText.Clear();
        GetHeroRarity();
        GetHeroRarityId();
    }

    //读取英雄的相关属性
    void GetExcelFile_Nature()
    {
        excelText.Clear();
        GetSpecificValue(1, "attack");
    }
    //玩家卡牌随机
    void RandomList()
    {
        //print("ssssssssssssssssssssslevel:"+level);
        while (myCard.Count < count)
        {
            switch (level)
            {
                case 1:
                    //print("sssssssssssssssssssssssssssssssssssssssss:1");
                    temp_num = Random.Range(0, greenCard.Count);
                    if (!myCard.Contains(greenCard[temp_num]))
                    {
                        myCard.Add(greenCard[temp_num]);
                    }
                    else
                    {
                        continue;
                    }
                    break;
                case 2:
                    //print("sssssssssssssssssssssssssssssssssssssssss:2");
                    int pr2 = Random.Range(1, 101);
                    if (pr2 < 6 + peopleHearts_blue)
                    {
                        temp_num = Random.Range(0, blueCard.Count);
                        if (!myCard.Contains(blueCard[temp_num]))
                        {
                            myCard.Add(blueCard[temp_num]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        temp_num = Random.Range(0, greenCard.Count);
                        if (!myCard.Contains(greenCard[temp_num]))
                        {
                            myCard.Add(greenCard[temp_num]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    break;
                case 3:
                    //print("sssssssssssssssssssssssssssssssssssssssss:3");
                    int pr3 = Random.Range(1, 101);
                    if (pr3 < 11 + peopleHearts_blue)
                    {
                        temp_num = Random.Range(0, blueCard.Count);
                        if (!myCard.Contains(blueCard[temp_num]))
                        {
                            myCard.Add(blueCard[temp_num]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        temp_num = Random.Range(0, greenCard.Count);
                        if (!myCard.Contains(greenCard[temp_num]))
                        {
                            myCard.Add(greenCard[temp_num]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    break;
                case 4:
                    //print("sssssssssssssssssssssssssssssssssssssssss:4");
                    int pr4 = Random.Range(1, 101);
                    if (pr4 < 4 + peopleHearts_purple)
                    {
                        temp_num = Random.Range(0, purpleCard.Count);
                        if (!myCard.Contains(purpleCard[temp_num]))
                        {
                            myCard.Add(purpleCard[temp_num]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (pr4 > 3 && pr4 < 20 + peopleHearts_blue)
                    {
                        temp_num = Random.Range(0, blueCard.Count);
                        if (!myCard.Contains(blueCard[temp_num]))
                        {
                            myCard.Add(blueCard[temp_num]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        temp_num = Random.Range(0, greenCard.Count);
                        if (!myCard.Contains(greenCard[temp_num]))
                        {
                            myCard.Add(greenCard[temp_num]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    break;
                case 5:
                    //print("sssssssssssssssssssssssssssssssssssssssss:5");
                    int pr5 = Random.Range(1, 101);
                    if (pr5 < 7 + peopleHearts_purple)
                    {
                        temp_num = Random.Range(0, purpleCard.Count);
                        if (!myCard.Contains(purpleCard[temp_num]))
                        {
                            myCard.Add(purpleCard[temp_num]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (pr5 > 6 && pr5 < 30 + peopleHearts_blue)
                    {
                        temp_num = Random.Range(0, blueCard.Count);
                        if (!myCard.Contains(blueCard[temp_num]))
                        {
                            myCard.Add(blueCard[temp_num]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        temp_num = Random.Range(0, greenCard.Count);
                        if (!myCard.Contains(greenCard[temp_num]))
                        {
                            myCard.Add(greenCard[temp_num]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    break;
                case 6:
                   // print("sssssssssssssssssssssssssssssssssssssssss:6");
                    int pr6 = Random.Range(1, 101);
                    if (pr6 < 2 + peopleHearts_orange)
                    {
                        temp_num = Random.Range(0, orangeCard.Count);
                        if (!myCard.Contains(orangeCard[temp_num]))
                        {
                            myCard.Add(orangeCard[temp_num]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (1 < pr6 && pr6 < 12 + peopleHearts_orange)
                    {
                        temp_num = Random.Range(0, purpleCard.Count);
                        if (!myCard.Contains(purpleCard[temp_num]))
                        {
                            myCard.Add(purpleCard[temp_num]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (pr6 > 11 && pr6 < 42 + peopleHearts_blue)
                    {
                        temp_num = Random.Range(0, blueCard.Count);
                        if (!myCard.Contains(blueCard[temp_num]))
                        {
                            myCard.Add(blueCard[temp_num]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        temp_num = Random.Range(0, greenCard.Count);
                        if (!myCard.Contains(greenCard[temp_num]))
                        {
                            myCard.Add(greenCard[temp_num]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    break;
                case 7:
                    //print("sssssssssssssssssssssssssssssssssssssssss:7");
                    int pr7 = Random.Range(1, 101);
                    if (pr7 < 3 + peopleHearts_orange)
                    {
                        temp_num = Random.Range(0, orangeCard.Count);
                        if (!myCard.Contains(orangeCard[temp_num]))
                        {
                            myCard.Add(orangeCard[temp_num]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (2 < pr7 && pr7 < 18 + peopleHearts_purple)
                    {
                        temp_num = Random.Range(0, purpleCard.Count);
                        if (!myCard.Contains(purpleCard[temp_num]))
                        {
                            myCard.Add(purpleCard[temp_num]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (pr7 > 17 && pr7 < 51 + peopleHearts_blue)
                    {
                        temp_num = Random.Range(0, blueCard.Count);
                        if (!myCard.Contains(blueCard[temp_num]))
                        {
                            myCard.Add(blueCard[temp_num]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        temp_num = Random.Range(0, greenCard.Count);
                        if (!myCard.Contains(greenCard[temp_num]))
                        {
                            myCard.Add(greenCard[temp_num]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    break;
                case 8:
                   // print("sssssssssssssssssssssssssssssssssssssssss:8");
                    int pr8 = Random.Range(1, 101);
                    if (pr8 < 4 + peopleHearts_orange)
                    {
                        temp_num = Random.Range(0, orangeCard.Count);
                        if (!myCard.Contains(orangeCard[temp_num]))
                        {
                            myCard.Add(orangeCard[temp_num]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (3 < pr8 && pr8 < 20 + peopleHearts_purple)
                    {
                        temp_num = Random.Range(0, purpleCard.Count);
                        if (!myCard.Contains(purpleCard[temp_num]))
                        {
                            myCard.Add(purpleCard[temp_num]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (pr8 > 20 && pr8 < 56 + peopleHearts_blue)
                    {
                        temp_num = Random.Range(0, blueCard.Count);
                        if (!myCard.Contains(blueCard[temp_num]))
                        {
                            myCard.Add(blueCard[temp_num]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        temp_num = Random.Range(0, greenCard.Count);
                        if (!myCard.Contains(greenCard[temp_num]))
                        {
                            myCard.Add(greenCard[temp_num]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    break;
                case 9:
                   // print("sssssssssssssssssssssssssssssssssssssssss:9");
                    int pr9 = Random.Range(1, 101);
                    if (pr9 < 4 + peopleHearts_orange)
                    {
                        temp_num = Random.Range(0, orangeCard.Count);
                        if (!myCard.Contains(orangeCard[temp_num]))
                        {
                            myCard.Add(orangeCard[temp_num]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (3 < pr9 && pr9 < 20 + peopleHearts_purple)
                    {
                        temp_num = Random.Range(0, purpleCard.Count);
                        if (!myCard.Contains(purpleCard[temp_num]))
                        {
                            myCard.Add(purpleCard[temp_num]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (pr9 > 20 && pr9 < 56 + peopleHearts_blue)
                    {
                        temp_num = Random.Range(0, blueCard.Count);
                        if (!myCard.Contains(blueCard[temp_num]))
                        {
                            myCard.Add(blueCard[temp_num]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        temp_num = Random.Range(0, greenCard.Count);
                        if (!myCard.Contains(greenCard[temp_num]))
                        {
                            myCard.Add(greenCard[temp_num]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        //for (int i = 0; i < greenCard.Count; i++)
        //{
        //    print("ssssssssssssssssssssssssssCardid:" + greenCard[i]);
        //}
    }
    //生成其他势力的随机数
    void PlayerRandom()
    {
        switch (playerLevel)
        {
            case 1:
                while (playerCardAll.Count < player_count)
                {
                    player_temp_num = Random.Range(0, greenCard.Count);
                    if (!playerCardAll.Contains(greenCard[player_temp_num]))
                    {
                        if (!myCard.Contains(greenCard[player_temp_num]))
                        {
                            playerCardAll.Add(greenCard[player_temp_num]);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                break;
            case 2:
                while (playerCardAll.Count < player_count)
                {
                    int pr2 = Random.Range(1, 101);
                    if (pr2 < 6)
                    {
                        player_temp_num = Random.Range(0, blueCard.Count);
                        if (!playerCardAll.Contains(blueCard[player_temp_num]))
                        {
                            if (!myCard.Contains(blueCard[player_temp_num]))
                            {
                                playerCardAll.Add(blueCard[player_temp_num]);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        player_temp_num = Random.Range(0, greenCard.Count);
                        if (!playerCardAll.Contains(greenCard[player_temp_num]))
                        {
                            if (!myCard.Contains(greenCard[player_temp_num]))
                            {
                                playerCardAll.Add(greenCard[player_temp_num]);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                break;
            case 3:
                while (playerCardAll.Count < player_count)
                {
                    int pr3 = Random.Range(1, 101);
                    if (pr3 < 11)
                    {
                        player_temp_num = Random.Range(0, blueCard.Count);
                        if (!playerCardAll.Contains(blueCard[player_temp_num]))
                        {
                            if (!myCard.Contains(blueCard[player_temp_num]))
                            {
                                playerCardAll.Add(blueCard[player_temp_num]);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        player_temp_num = Random.Range(0, greenCard.Count);
                        if (!playerCardAll.Contains(greenCard[player_temp_num]))
                        {
                            if (!myCard.Contains(greenCard[player_temp_num]))
                            {
                                playerCardAll.Add(greenCard[player_temp_num]);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                break;
            case 4:
                while (playerCardAll.Count < player_count)
                {
                    int pr4 = Random.Range(1, 101);
                    if (pr4 < 4)
                    {
                        player_temp_num = Random.Range(0, purpleCard.Count);
                        if (!playerCardAll.Contains(purpleCard[player_temp_num]))
                        {
                            if (!myCard.Contains(purpleCard[player_temp_num]))
                            {
                                playerCardAll.Add(purpleCard[player_temp_num]);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (pr4 > 19)
                    {
                        player_temp_num = Random.Range(0, greenCard.Count);
                        if (!playerCardAll.Contains(greenCard[player_temp_num]))
                        {
                            if (!myCard.Contains(greenCard[player_temp_num]))
                            {
                                playerCardAll.Add(greenCard[player_temp_num]);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        player_temp_num = Random.Range(0, blueCard.Count);
                        if (!playerCardAll.Contains(blueCard[player_temp_num]))
                        {
                            if (!myCard.Contains(blueCard[player_temp_num]))
                            {
                                playerCardAll.Add(blueCard[player_temp_num]);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                break;
            case 5:
                while (playerCardAll.Count < player_count)
                {
                    int pr5 = Random.Range(1, 101);
                    if (pr5 < 7)
                    {
                        player_temp_num = Random.Range(0, purpleCard.Count);
                        if (!playerCardAll.Contains(purpleCard[player_temp_num]))
                        {
                            if (!myCard.Contains(purpleCard[player_temp_num]))
                            {
                                playerCardAll.Add(purpleCard[player_temp_num]);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (pr5 > 29)
                    {
                        player_temp_num = Random.Range(0, greenCard.Count);
                        if (!playerCardAll.Contains(greenCard[player_temp_num]))
                        {
                            if (!myCard.Contains(greenCard[player_temp_num]))
                            {
                                playerCardAll.Add(greenCard[player_temp_num]);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        player_temp_num = Random.Range(0, blueCard.Count);
                        if (!playerCardAll.Contains(blueCard[player_temp_num]))
                        {
                            if (!myCard.Contains(blueCard[player_temp_num]))
                            {
                                playerCardAll.Add(blueCard[player_temp_num]);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                break;
            case 6:
                while (playerCardAll.Count < player_count)
                {
                    int pr6 = Random.Range(1, 101);
                    if (pr6 < 2)
                    {
                        player_temp_num = Random.Range(0, orangeCard.Count);
                        if (!playerCardAll.Contains(orangeCard[player_temp_num]))
                        {
                            if (!myCard.Contains(orangeCard[player_temp_num]))
                            {
                                playerCardAll.Add(orangeCard[player_temp_num]);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (1 < pr6 && pr6 < 12)
                    {
                        player_temp_num = Random.Range(0, purpleCard.Count);
                        if (!playerCardAll.Contains(purpleCard[player_temp_num]))
                        {
                            if (!myCard.Contains(purpleCard[player_temp_num]))
                            {
                                playerCardAll.Add(purpleCard[player_temp_num]);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (pr6 > 41)
                    {
                        player_temp_num = Random.Range(0, greenCard.Count);
                        if (!playerCardAll.Contains(greenCard[player_temp_num]))
                        {
                            if (!myCard.Contains(greenCard[player_temp_num]))
                            {
                                playerCardAll.Add(greenCard[player_temp_num]);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        player_temp_num = Random.Range(0, blueCard.Count);
                        if (!playerCardAll.Contains(blueCard[player_temp_num]))
                        {
                            if (!myCard.Contains(blueCard[player_temp_num]))
                            {
                                playerCardAll.Add(blueCard[player_temp_num]);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                break;
            case 7:
                while (playerCardAll.Count < player_count)
                {
                    int pr7 = Random.Range(1, 101);
                    if (pr7 < 3)
                    {
                        player_temp_num = Random.Range(0, orangeCard.Count);
                        if (!playerCardAll.Contains(orangeCard[player_temp_num]))
                        {
                            if (!myCard.Contains(orangeCard[player_temp_num]))
                            {
                                playerCardAll.Add(orangeCard[player_temp_num]);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (2 < pr7 && pr7 < 18)
                    {
                        player_temp_num = Random.Range(0, purpleCard.Count);
                        if (!playerCardAll.Contains(purpleCard[player_temp_num]))
                        {
                            if (!myCard.Contains(purpleCard[player_temp_num]))
                            {
                                playerCardAll.Add(purpleCard[player_temp_num]);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (pr7 > 50)
                    {
                        player_temp_num = Random.Range(0, greenCard.Count);
                        if (!playerCardAll.Contains(greenCard[player_temp_num]))
                        {
                            if (!myCard.Contains(greenCard[player_temp_num]))
                            {
                                playerCardAll.Add(greenCard[player_temp_num]);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        player_temp_num = Random.Range(0, blueCard.Count);
                        if (!playerCardAll.Contains(blueCard[player_temp_num]))
                        {
                            if (!myCard.Contains(blueCard[player_temp_num]))
                            {
                                playerCardAll.Add(blueCard[player_temp_num]);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                break;
            case 8:
                while (playerCardAll.Count < player_count)
                {
                    int pr8 = Random.Range(1, 101);
                    if (pr8 < 4)
                    {
                        player_temp_num = Random.Range(0, orangeCard.Count);
                        if (!playerCardAll.Contains(orangeCard[player_temp_num]))
                        {
                            if (!myCard.Contains(orangeCard[player_temp_num]))
                            {
                                playerCardAll.Add(orangeCard[player_temp_num]);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (3 < pr8 && pr8 < 20)
                    {
                        player_temp_num = Random.Range(0, purpleCard.Count);
                        if (!playerCardAll.Contains(purpleCard[player_temp_num]))
                        {
                            if (!myCard.Contains(purpleCard[player_temp_num]))
                            {
                                playerCardAll.Add(purpleCard[player_temp_num]);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (pr8 > 55)
                    {
                        player_temp_num = Random.Range(0, greenCard.Count);
                        if (!playerCardAll.Contains(greenCard[player_temp_num]))
                        {
                            if (!myCard.Contains(greenCard[player_temp_num]))
                            {
                                playerCardAll.Add(greenCard[player_temp_num]);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        player_temp_num = Random.Range(0, blueCard.Count);
                        if (!playerCardAll.Contains(blueCard[player_temp_num]))
                        {
                            if (!myCard.Contains(blueCard[player_temp_num]))
                            {
                                playerCardAll.Add(blueCard[player_temp_num]);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                break;
            case 9:
                while (playerCardAll.Count < player_count)
                {
                    int pr9 = Random.Range(1, 101);
                    if (pr9 < 4)
                    {
                        player_temp_num = Random.Range(0, orangeCard.Count);
                        if (!playerCardAll.Contains(orangeCard[player_temp_num]))
                        {
                            if (!myCard.Contains(orangeCard[player_temp_num]))
                            {
                                playerCardAll.Add(orangeCard[player_temp_num]);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (3 < pr9 && pr9 < 20)
                    {
                        player_temp_num = Random.Range(0, purpleCard.Count);
                        if (!playerCardAll.Contains(purpleCard[player_temp_num]))
                        {
                            if (!myCard.Contains(purpleCard[player_temp_num]))
                            {
                                playerCardAll.Add(purpleCard[player_temp_num]);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (pr9 > 55)
                    {
                        player_temp_num = Random.Range(0, greenCard.Count);
                        if (!playerCardAll.Contains(greenCard[player_temp_num]))
                        {
                            if (!myCard.Contains(greenCard[player_temp_num]))
                            {
                                playerCardAll.Add(greenCard[player_temp_num]);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        player_temp_num = Random.Range(0, blueCard.Count);
                        if (!playerCardAll.Contains(blueCard[player_temp_num]))
                        {
                            if (!myCard.Contains(blueCard[player_temp_num]))
                            {
                                playerCardAll.Add(blueCard[player_temp_num]);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                break;
            default:
                break;
        }
        for (int i = 0; i < playerCardAll.Count; i++)
        {
            if (i < 5)
            {
                player1Card.Add(playerCardAll[i]);
            }
            else if (i > 4 && i < 10)
            {
                player2Card.Add(playerCardAll[i]);
            }
            else if (i > 9 && i < 15)
            {
                player3Card.Add(playerCardAll[i]);
            }
            else if (i > 14 && i < 20)
            {
                player4Card.Add(playerCardAll[i]);
            }
            else
            {
                player5Card.Add(playerCardAll[i]);
            }
        }
    }

    //玩家点击刷新
    public void UpdateCard()
    {
        if (money >= 2)
        {
            GameObject topBar = GameObject.Find("TopInformationBar");
            if (topBar == null)
                return;
            //点击刷新，清空上显示栏
            for (int i = 0; i < 4; i++)
            {
                topBar.GetComponentsInChildren<Text>()[i].text = "";
            }

            money -= 2;
            getCardId.Clear();
            SetPeopleHarets();
            myCard.Clear();
            heroName.Clear();
            soliderAttack.Clear();
            soliderMoney.Clear();
            soliderDefense.Clear();
            soliderKind.Clear();
            soliderForce.Clear();
            soliderRarity.Clear();
            switch (level)
            {
                case 1:
                    //print("ssssssssssssssssssssssssssssssssssssssssss1");
                    while (myCard.Count < count)
                    {
                        temp_num = Random.Range(0, greenCard.Count);
                        if (!myCard.Contains(greenCard[temp_num]))
                        {
                            if (!playerCardAll.Contains(greenCard[temp_num]))
                            {
                                if (!getCardId.Contains(greenCard[temp_num]))
                                {
                                    myCard.Add(greenCard[temp_num]);
                                }
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    break;
                case 2:
                    //print("ssssssssssssssssssssssssssssssssssssssssss2");
                    while (myCard.Count < count)
                    {
                        int pr2 = Random.Range(1, 101);
                        if (pr2 < 6 + peopleHearts_blue)
                        {
                            temp_num = Random.Range(0, blueCard.Count);
                            if (!myCard.Contains(blueCard[temp_num]))
                            {
                                if (!playerCardAll.Contains(blueCard[temp_num]))
                                {
                                    if (!getCardId.Contains(blueCard[temp_num]))
                                    {
                                        myCard.Add(blueCard[temp_num]);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            temp_num = Random.Range(0, greenCard.Count);
                            if (!myCard.Contains(greenCard[temp_num]))
                            {
                                if (!playerCardAll.Contains(greenCard[temp_num]))
                                {
                                    if (!getCardId.Contains(greenCard[temp_num]))
                                    {
                                        myCard.Add(greenCard[temp_num]);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    break;
                case 3:
                    while (myCard.Count < count)
                    {
                        int pr3 = Random.Range(1, 101);
                        if (pr3 < 11 + peopleHearts_blue)
                        {
                            temp_num = Random.Range(0, blueCard.Count);
                            if (!myCard.Contains(blueCard[temp_num]))
                            {
                                if (!playerCardAll.Contains(blueCard[temp_num]))
                                {
                                    if (!getCardId.Contains(blueCard[temp_num]))
                                    {
                                        myCard.Add(blueCard[temp_num]);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            temp_num = Random.Range(0, greenCard.Count);
                            if (!myCard.Contains(greenCard[temp_num]))
                            {
                                if (!playerCardAll.Contains(greenCard[temp_num]))
                                {
                                    if (!getCardId.Contains(greenCard[temp_num]))
                                    {
                                        myCard.Add(greenCard[temp_num]);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    break;
                case 4:
                    while (myCard.Count < count)
                    {
                        int pr4 = Random.Range(1, 101);
                        if (pr4 < 4 + peopleHearts_purple)
                        {
                            temp_num = Random.Range(0, purpleCard.Count);
                            if (!myCard.Contains(purpleCard[temp_num]))
                            {
                                if (!playerCardAll.Contains(purpleCard[temp_num]))
                                {
                                    if (!getCardId.Contains(purpleCard[temp_num]))
                                    {
                                        myCard.Add(purpleCard[temp_num]);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else if (pr4 > 3 && pr4 < 20 + peopleHearts_blue)
                        {
                            temp_num = Random.Range(0, blueCard.Count);
                            if (!myCard.Contains(blueCard[temp_num]))
                            {
                                if (!playerCardAll.Contains(blueCard[temp_num]))
                                {
                                    if (!getCardId.Contains(blueCard[temp_num]))
                                    {
                                        myCard.Add(blueCard[temp_num]);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            temp_num = Random.Range(0, greenCard.Count);
                            if (!myCard.Contains(greenCard[temp_num]))
                            {
                                if (!playerCardAll.Contains(greenCard[temp_num]))
                                {
                                    if (!getCardId.Contains(greenCard[temp_num]))
                                    {
                                        myCard.Add(greenCard[temp_num]);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    break;
                case 5:
                    while (myCard.Count < count)
                    {
                        int pr5 = Random.Range(1, 101);
                        if (pr5 < 7 + peopleHearts_purple)
                        {
                            temp_num = Random.Range(0, purpleCard.Count);
                            if (!myCard.Contains(purpleCard[temp_num]))
                            {
                                if (!playerCardAll.Contains(purpleCard[temp_num]))
                                {
                                    if (!getCardId.Contains(purpleCard[temp_num]))
                                    {
                                        myCard.Add(purpleCard[temp_num]);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else if (pr5 > 6 && pr5 < 30 + peopleHearts_blue)
                        {
                            temp_num = Random.Range(0, blueCard.Count);
                            if (!myCard.Contains(blueCard[temp_num]))
                            {
                                if (!playerCardAll.Contains(blueCard[temp_num]))
                                {
                                    if (!getCardId.Contains(blueCard[temp_num]))
                                    {
                                        myCard.Add(blueCard[temp_num]);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            temp_num = Random.Range(0, greenCard.Count);
                            if (!myCard.Contains(greenCard[temp_num]))
                            {
                                if (!playerCardAll.Contains(greenCard[temp_num]))
                                {
                                    if (!getCardId.Contains(greenCard[temp_num]))
                                    {
                                        myCard.Add(greenCard[temp_num]);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    break;
                case 6:
                    while (myCard.Count < count)
                    {
                        int pr6 = Random.Range(1, 101);
                        if (pr6 < 2 + peopleHearts_orange)
                        {
                            temp_num = Random.Range(0, orangeCard.Count);
                            if (!myCard.Contains(orangeCard[temp_num]))
                            {
                                if (!playerCardAll.Contains(orangeCard[temp_num]))
                                {
                                    if (!getCardId.Contains(orangeCard[temp_num]))
                                    {
                                        myCard.Add(orangeCard[temp_num]);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        if (1 < pr6 && pr6 < 12 + peopleHearts_purple)
                        {
                            temp_num = Random.Range(0, purpleCard.Count);
                            if (!myCard.Contains(purpleCard[temp_num]))
                            {
                                if (!playerCardAll.Contains(purpleCard[temp_num]))
                                {
                                    if (!getCardId.Contains(purpleCard[temp_num]))
                                    {
                                        myCard.Add(purpleCard[temp_num]);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else if (pr6 > 11 && pr6 < 43 + peopleHearts_blue)
                        {
                            temp_num = Random.Range(0, blueCard.Count);
                            if (!myCard.Contains(blueCard[temp_num]))
                            {
                                if (!playerCardAll.Contains(blueCard[temp_num]))
                                {
                                    if (!getCardId.Contains(blueCard[temp_num]))
                                    {
                                        myCard.Add(blueCard[temp_num]);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            temp_num = Random.Range(0, greenCard.Count);
                            if (!myCard.Contains(greenCard[temp_num]))
                            {
                                if (!playerCardAll.Contains(greenCard[temp_num]))
                                {
                                    if (!getCardId.Contains(greenCard[temp_num]))
                                    {
                                        myCard.Add(greenCard[temp_num]);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    break;
                case 7:
                    while (myCard.Count < count)
                    {
                        int pr7 = Random.Range(1, 101);
                        if (pr7 < 3 + peopleHearts_orange)
                        {
                            temp_num = Random.Range(0, orangeCard.Count);
                            if (!myCard.Contains(orangeCard[temp_num]))
                            {
                                if (!playerCardAll.Contains(orangeCard[temp_num]))
                                {
                                    if (!getCardId.Contains(orangeCard[temp_num]))
                                    {
                                        myCard.Add(orangeCard[temp_num]);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        if (2 < pr7 && pr7 < 18 + peopleHearts_purple)
                        {
                            temp_num = Random.Range(0, purpleCard.Count);
                            if (!myCard.Contains(purpleCard[temp_num]))
                            {
                                if (!playerCardAll.Contains(purpleCard[temp_num]))
                                {
                                    if (!getCardId.Contains(purpleCard[temp_num]))
                                    {
                                        myCard.Add(purpleCard[temp_num]);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else if (pr7 > 17 && pr7 < 52 + peopleHearts_blue)
                        {
                            temp_num = Random.Range(0, blueCard.Count);
                            if (!myCard.Contains(blueCard[temp_num]))
                            {
                                if (!playerCardAll.Contains(blueCard[temp_num]))
                                {
                                    if (!getCardId.Contains(blueCard[temp_num]))
                                    {
                                        myCard.Add(blueCard[temp_num]);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            temp_num = Random.Range(0, greenCard.Count);
                            if (!myCard.Contains(greenCard[temp_num]))
                            {
                                if (!playerCardAll.Contains(greenCard[temp_num]))
                                {
                                    if (!getCardId.Contains(greenCard[temp_num]))
                                    {
                                        myCard.Add(greenCard[temp_num]);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    break;
                case 8:
                    while (myCard.Count < count)
                    {
                        if (myCard.Count == count)
                        {
                            break;
                        }
                        int pr8 = Random.Range(1, 101);
                        if (pr8 < 4 + peopleHearts_orange)
                        {
                            temp_num = Random.Range(0, orangeCard.Count);
                            if (!myCard.Contains(orangeCard[temp_num]))
                            {
                                if (!playerCardAll.Contains(orangeCard[temp_num]))
                                {
                                    if (!getCardId.Contains(orangeCard[temp_num]))
                                    {
                                        myCard.Add(orangeCard[temp_num]);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        if (3 < pr8 && pr8 < 20 + peopleHearts_purple)
                        {
                            temp_num = Random.Range(0, purpleCard.Count);
                            if (!myCard.Contains(purpleCard[temp_num]))
                            {
                                if (!playerCardAll.Contains(purpleCard[temp_num]))
                                {
                                    if (!getCardId.Contains(purpleCard[temp_num]))
                                    {
                                        myCard.Add(purpleCard[temp_num]);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else if (pr8 > 20 && pr8 < 56 + peopleHearts_blue)
                        {
                            temp_num = Random.Range(0, blueCard.Count);
                            if (!myCard.Contains(blueCard[temp_num]))
                            {
                                if (!playerCardAll.Contains(blueCard[temp_num]))
                                {
                                    if (!getCardId.Contains(blueCard[temp_num]))
                                    {
                                        myCard.Add(blueCard[temp_num]);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            temp_num = Random.Range(0, greenCard.Count);
                            if (!myCard.Contains(greenCard[temp_num]))
                            {
                                if (!playerCardAll.Contains(greenCard[temp_num]))
                                {
                                    if (!getCardId.Contains(greenCard[temp_num]))
                                    {
                                        myCard.Add(greenCard[temp_num]);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    break;
                default :
                    while (myCard.Count < count)
                    {
                        int pr9 = Random.Range(1, 101);
                        if (pr9 < 4 + peopleHearts_orange)
                        {
                            temp_num = Random.Range(0, orangeCard.Count);
                            if (!myCard.Contains(orangeCard[temp_num]))
                            {
                                if (!playerCardAll.Contains(orangeCard[temp_num]))
                                {
                                    if (!getCardId.Contains(orangeCard[temp_num]))
                                    {
                                        myCard.Add(orangeCard[temp_num]);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        if (3 < pr9 && pr9 < 20 + peopleHearts_purple)
                        {
                            temp_num = Random.Range(0, purpleCard.Count);
                            if (!myCard.Contains(purpleCard[temp_num]))
                            {
                                if (!playerCardAll.Contains(purpleCard[temp_num]))
                                {
                                    if (!getCardId.Contains(purpleCard[temp_num]))
                                    {
                                        myCard.Add(purpleCard[temp_num]);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else if (pr9 > 20 && pr9 < 56 + peopleHearts_blue)
                        {
                            temp_num = Random.Range(0, blueCard.Count);
                            if (!myCard.Contains(blueCard[temp_num]))
                            {
                                if (!playerCardAll.Contains(blueCard[temp_num]))
                                {
                                    if (!getCardId.Contains(blueCard[temp_num]))
                                    {
                                        myCard.Add(blueCard[temp_num]);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            temp_num = Random.Range(0, greenCard.Count);
                            if (!myCard.Contains(greenCard[temp_num]))
                            {
                                if (!playerCardAll.Contains(greenCard[temp_num]))
                                {
                                    if (!getCardId.Contains(greenCard[temp_num]))
                                    {
                                        myCard.Add(greenCard[temp_num]);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    break;
            }
            if (myCard.Count == 6)
            {
                myCard.RemoveAt(5);
            }
            GetExcelFile();
            //heroBtn.Clear();
            HeroLocation();
            for (int i = 0; i < myCard.Count; i++)
            {
                heroBtn[i].transform.GetChild(heroBtn[i].transform.childCount - 1).gameObject.SetActive(false);
                //heroBtn[i].GetComponent<Button>().enabled = true;   //开启点击事件
            }
        }
        else
        {
            //print("啊，呸，穷鬼");
            GoldNotEnough(LoadJsonFile.TipsTableDates[3][2]);
        }
        LateInitHeroNums(0.1f);    //刷新购买的武将拥有数量显示
    }

    //招募显示
    private void HeroLocation()
    {
        //招募中民心的显示
        if (heroName.Count == 6)
        {
            heroName.RemoveAt(5);
        }
        textHertNum.text= peopleHearts.ToString();
        int datasNums = LoadJsonFile.RoleTableDatas[0].Count;
        for (int i = 0; i < heroName.Count; i++)
        {
            //兵种显示
            heroBtn[i].GetComponentsInChildren<Text>()[5].text = LoadJsonFile.SoldierTypeDates[int.Parse(soliderKind[i]) - 1][2];
            
            //显示武将背景图片
            heroBtn[i].transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load("Image/RoleIcon/" + LoadJsonFile.RoleTableDatas[excelText[i] - 1][datasNums - 1] , typeof(Sprite)) as Sprite;

            //英雄名字显示
            heroBtn[i].GetComponentsInChildren<Text>()[0].text = "";// heroName[i].ToString();
            //根据稀有度设置字体颜色
            if (soliderRarity[i] == "1")
            {
                heroBtn[i].GetComponentsInChildren<Text>()[0].color = ColorData.green_Color;  //绿色
            }
            else if (soliderRarity[i] == "2")
            {
                heroBtn[i].GetComponentsInChildren<Text>()[0].color = ColorData.blue_Color_hero; //蓝色
            }
            else if (soliderRarity[i] == "3")
            {
                heroBtn[i].GetComponentsInChildren<Text>()[0].color = ColorData.purple_Color; //紫色
            }
            else if (soliderRarity[i] == "4")
            {
                heroBtn[i].GetComponentsInChildren<Text>()[0].color = ColorData.red_Color_hero;  //红色
            }
            //花费显示
            heroBtn[i].GetComponentsInChildren<Text>()[3].text = soliderMoney[i];
            //防御显示
            heroBtn[i].GetComponentsInChildren<Text>()[4].text = soliderDefense[i];
            //势力显示
            heroBtn[i].name = getCardId[i].ToString();
        }
    }

    /// <summary>
    /// 延时加载武将拥有数量
    /// </summary>
    /// <param name="times"></param>
    public void LateInitHeroNums(float times)
    {
        Invoke("UpdateHadHeroNumsText", times);
    }

    /// <summary>
    /// 刷新购买的武将拥有数量显示
    /// </summary>
    private void UpdateHadHeroNumsText()
    {
        Debug.Log(string.Format("{0},{1},{2},{3},{4}", excelText[0], excelText[1], excelText[2], excelText[3], excelText[4]));
        for (int i = 0; i < heroBtn.Count; i++)
        {
            heroBtn[i].GetComponentsInChildren<Text>()[2].text = backGround.GetComponent<HeroIdChangeAndSave>().StatisticsHeroNums(excelText[i]).ToString();
        }
    }

    //输入id获取整行数据--招募表
    void GetValueFromId(int num)
    {
        for (int i = 0; i < 792; i++)
        {
            if (int.Parse(LoadJsonFile.RandowTableDates[i][1]) == num)
            {
                excelText.Add(int.Parse(LoadJsonFile.RandowTableDates[i][0]));
                getCardId.Add(int.Parse(LoadJsonFile.RandowTableDates[i][1]));
            }

        }
    }
    //根据id和列名拿到具体的值--英雄表
    void GetSpecificValue(int id, string name)
    {
        int hreoNums = LoadJsonFile.RoleTableDatas.Count;
        for (int i = 0; i < hreoNums; i++)
        {
            if (int.Parse(LoadJsonFile.RoleTableDatas[i][0]) == id)
            {
                if (name == "roleName")
                {
                    heroName.Add(LoadJsonFile.RoleTableDatas[i][1]);
                }
                if (name == "attack")
                {
                    attack = int.Parse(LoadJsonFile.RoleTableDatas[i][6]);
                }
                if (name == "recruitingMoney")
                {
                    soliderMoney.Add(LoadJsonFile.RoleTableDatas[i][5]);
                }
                if (name == "soliderAttack")
                {
                    soliderAttack.Add(LoadJsonFile.RoleTableDatas[i][6]);
                }
                if (name == "soliderDefense")
                {
                    soliderDefense.Add(LoadJsonFile.RoleTableDatas[i][7]);
                }
                if (name == "soliderKind")
                {
                    soliderKind.Add(LoadJsonFile.RoleTableDatas[i][3]);
                }
                if (name == "soliderForce")
                {
                    soliderForce.Add(LoadJsonFile.RoleTableDatas[i][2]);
                }
                if (name == "soliderRarity")
                {
                    soliderRarity.Add(LoadJsonFile.RoleTableDatas[i][4]);
                }
            }
        }
    }

    //把英雄按照稀有度分类
    void GetHeroRarity()
    {
        int hreoNums = LoadJsonFile.RoleTableDatas.Count;
        for (int i = 0; i < hreoNums; i++)
        {
            if (LoadJsonFile.RoleTableDatas[i][4] == "1")             //拿到绿色
            {
                greenHeroId.Add(i);
            }
            else if (LoadJsonFile.RoleTableDatas[i][4] == "2")
            {
                blueHeroId.Add(i);
            }
            else if (LoadJsonFile.RoleTableDatas[i][4] == "3")
            {
                purpleHeroId.Add(i);
            }
            else if (LoadJsonFile.RoleTableDatas[i][4] == "4")
            {
                orangeHeroId.Add(i);
            }
        }
    }
    //在表2中拿到相关稀有度的英雄Id并存储
    void GetHeroRarityId()
    {
        for (int i = 0; i < 738; i++)
        {

                for (int y = 0; y < greenHeroId.Count; y++)
                {
                    if (LoadJsonFile.RandowTableDates[i][0] == (greenHeroId[y]+1).ToString())
                    {
                        greenCard.Add(int.Parse(LoadJsonFile.RandowTableDates[i][1]));
                    }
                }
                for (int y = 0; y < blueHeroId.Count; y++)
                {
                    if (LoadJsonFile.RandowTableDates[i][0] == (blueHeroId[y]+1).ToString())
                    {
                        blueCard.Add(int.Parse(LoadJsonFile.RandowTableDates[i][1]));
                    }
                }
                for (int y = 0; y < purpleHeroId.Count; y++)
                {
                    if (LoadJsonFile.RandowTableDates[i][0] == (purpleHeroId[y]+1).ToString())
                    {
                        purpleCard.Add(int.Parse(LoadJsonFile.RandowTableDates[i][1]));
                    }
                }
                for (int y = 0; y < orangeHeroId.Count; y++)
                {
                    if (LoadJsonFile.RandowTableDates[i][0] == (orangeHeroId[y]+1).ToString())
                    {
                        orangeCard.Add(int.Parse(LoadJsonFile.RandowTableDates[i][1]));
                    }
                }
            
        }
    }
    //民心加成
    void SetPeopleHarets()
    {
        if (peopleHearts >= 0 && peopleHearts <= 10)
        {
            peopleHearts_blue = -3;
            peopleHearts_purple = -4;
            peopleHearts_orange = -2;
            peopleHearts_red = -3;
        }
        else if (peopleHearts >= 11 && peopleHearts <= 20)
        {
            peopleHearts_blue = -2;
            peopleHearts_purple = -3;
            peopleHearts_orange = -1;
            peopleHearts_red = -1;
        }
        else if (peopleHearts >= 21 && peopleHearts <= 30)
        {
            peopleHearts_blue = -2;
            peopleHearts_purple = -2;
            peopleHearts_orange = 0;
            peopleHearts_red = 0;
        }
        else if (peopleHearts >= 31 && peopleHearts <= 40)
        {
            peopleHearts_blue = -2;
            peopleHearts_purple = -1;
            peopleHearts_orange = 0;
            peopleHearts_red = 0;
        }
        else if (peopleHearts >= 41 && peopleHearts <= 49)
        {
            peopleHearts_blue = -1;
            peopleHearts_purple = 0;
            peopleHearts_orange = 0;
            peopleHearts_red = 0;
        }
        else if (peopleHearts == 50)
        {
            peopleHearts_blue = 0;
            peopleHearts_purple = 0;
            peopleHearts_orange = 0;
            peopleHearts_red = 0;
        }
        else if (peopleHearts >= 51 && peopleHearts <= 60)
        {
            peopleHearts_blue = 1;
            peopleHearts_purple = 0;
            peopleHearts_orange = 0;
            peopleHearts_red = 0;
        }
        else if (peopleHearts >= 61 && peopleHearts <= 70)
        {
            peopleHearts_blue = 2;
            peopleHearts_purple = 1;
            peopleHearts_orange = 0;
            peopleHearts_red = 0;
        }
        else if (peopleHearts >= 71 && peopleHearts <= 80)
        {
            peopleHearts_blue = 2;
            peopleHearts_purple = 2;
            peopleHearts_orange = 0;
            peopleHearts_red = 0;
        }
        else if (peopleHearts >= 81 && peopleHearts <= 90)
        {
            peopleHearts_blue = 2;
            peopleHearts_purple = 3;
            peopleHearts_orange = 1;
            peopleHearts_red = 1;
        }
        else if (peopleHearts >= 91 && peopleHearts <= 100)
        {
            peopleHearts_blue = 3;
            peopleHearts_purple = 4;
            peopleHearts_orange = 2;
            peopleHearts_red = 2;
        }
    }
    //士气加成
    void SetMorale()
    {
        if (moraleNum >= 0 && moraleNum <= 10)
        {
            damageAll = -9;
        }
        else if (moraleNum >= 11 && moraleNum <= 20)
        {
            damageAll = -7;
        }
        else if (moraleNum >= 21 && moraleNum <= 30)
        {
            damageAll = -5;
        }
        else if (moraleNum >= 31 && moraleNum <= 40)
        {
            damageAll = -3;
        }
        else if (moraleNum >= 41 && moraleNum <= 49)
        {
            damageAll = -1;
        }
        else if (moraleNum == 50)
        {
            damageAll = 0;
        }
        else if (moraleNum >= 51 && moraleNum <= 60)
        {
            damageAll = 1;
        }
        else if (moraleNum >= 61 && moraleNum <= 70)
        {
            damageAll = 3;
        }
        else if (moraleNum >= 71 && moraleNum <= 80)
        {
            damageAll = 5;
        }
        else if (moraleNum >= 81 && moraleNum <= 90)
        {
            damageAll = 7;
        }
        else if (moraleNum >= 91 && moraleNum <= 100)
        {
            damageAll = 9;
        }
        else if (moraleNum >= 101 && moraleNum <= 110)
        {
            damageAll = 10;
        }
    }

    //宝箱功能
    public void GoldBox()
    {
        string money_str = LoadJsonFile.levelTableDatas[level - 1][3];
        string[] moneyStr = money_str.Split(',');
        int taxMin = int.Parse(moneyStr[0]);
        int taxMax = int.Parse(moneyStr[1]) + 1;
        int getMoney = Random.Range(taxMin,taxMax);
        money += getMoney;
        GoldBoxObj.GetComponent<Image>().overrideSprite= Resources.Load("Image/mainImage/宝箱_开", typeof(Sprite)) as Sprite;
        GoldBoxObj.GetComponent<Button>().enabled = false;
        if (prompTran.childCount<1)
        {
            GameObject tipObj = Instantiate(tipsText, prompTran);
            tipObj.GetComponent<Text>().text = string.Format(LoadJsonFile.TipsTableDates[0][2], getMoney);  //获得金币提示
        }
        Invoke("NotShowBox",1f);
    }
    //延时后隐藏宝箱
    void NotShowBox()
    {
        GoldBoxObj.SetActive(false);
    }
    //产生宝箱
    public void ShowGlodBox()
    {
        GoldBox_.SetActive(true);
        GoldBoxObj.GetComponent<Image>().overrideSprite = Resources.Load("Image/mainImage/宝箱_闭", typeof(Sprite)) as Sprite;
        GoldBoxObj.GetComponent<Button>().enabled = true;
    }

    /// <summary>
    /// 纯文字提示文本
    /// </summary>
    public void GoldNotEnough(string str)
    {
        if (prompTran.childCount < 1)
        {
            GameObject tipObj = Instantiate(tipsText, prompTran);
            tipObj.GetComponent<Text>().text = str;  //金币不足提示
        }
    }

    /// <summary>
    /// 敬请期待提示
    /// </summary>
    public void StayTunedTips()
    {
        GoldNotEnough(LoadJsonFile.TipsTableDates[7][2]);
    }
}