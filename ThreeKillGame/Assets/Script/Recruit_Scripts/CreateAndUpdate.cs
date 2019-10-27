using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using OfficeOpenXml;    //引入使用EPPlus类库

public class CreateAndUpdate : MonoBehaviour
{
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
    List<string> heroName = new List<string>();
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

    public int peopleHearts;        //民心
    public int moraleNum;           //士气
    public int playerHp;            //血量
    public static int money;        //金币
    public static int level;        //等级
    public static int experience;   //经验
    int damageAll;

    [SerializeField]
    Text text_level;    //等级显示
    UseEPPlusFun useFun = new UseEPPlusFun();

    /// <summary>
    /// 战斗结束后增加金币和经验
    /// </summary>
    public static void AddMoneyAndExp()
    {
        money += int.Parse(LoadJsonFile.levelTableDatas[level][3]);
        experience++;
        if (experience >= int.Parse(LoadJsonFile.levelTableDatas[level][2]))
        {
            level++;
            experience = 0;
        }
    }
    /// <summary>
    /// 更新等级牌
    /// </summary>
    public void ChangeLevelText()
    {
        text_level.text = level.ToString();
    }

    /// <summary>
    /// 使用金币进行升级
    /// </summary>
    public void BuyExpToLevel()
    {
        if (money >= (int.Parse(LoadJsonFile.levelTableDatas[level][2]) - experience))
        {
            money -= (int.Parse(LoadJsonFile.levelTableDatas[level][2]) - experience);
            level++;
            ChangeLevelText();
            Debug.Log("使用金币升级");
        }
        else
        {
            Debug.Log("金币不够");
        }
    }


    private void Awake()
    {
        level = 1;
        text_level.text = level.ToString();
        experience = 0;
        money = 100;
    }

    void Start()
    {
        //ChangeLevelText();
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
    }

    // Update is called once per frame
    void Update()
    {

    }
    //读取excel表格
    void GetExcelFile()
    {
        excelText.Clear();
        //string filePath = "F:/dev/GameCommon/111.xlsx";   //绝对路径
        string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        FileInfo fileinfo = new FileInfo(filePath);
        using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))   //using用来强行做资源释放
        {
            ExcelWorksheet worksheet1 = excelpackge.Workbook.Worksheets[1];
            ExcelWorksheet worksheet2 = excelpackge.Workbook.Worksheets[2];
            //print(worksheet.Cells[2,1].Value.ToString());
            //GetValueFromId(18, worksheet2);
            for (int i = 0; i < myCard.Count; i++)
            {
                GetValueFromId(int.Parse(myCard[i].ToString()) + 1, worksheet2);
                //heroBtn[i].GetComponentInChildren<Text>().text = myCard[i].ToString();
            }
            for (int i = 0; i < excelText.Count; i++)
            {
                GetSpecificValue(excelText[i], worksheet1, "roleName");
            }
            GetHeroRarity(worksheet1, "rarity");
            GetHeroRarityId(worksheet2);
            for (int i = 0; i < getCardId.Count; i++)
            {
                //print(myCard[i].ToString() + ".." + excelText[i].ToString()+".."+excelText.Count);
                //print("CardId:"+ getCardId[i]);
            }
            //print(worksheet1.Cells.Columns);
        }
    }
    //读取获得各种稀有度英雄
    void GetExcelFile1()
    {
        excelText.Clear();
        //string filePath = "F:/dev/GameCommon/111.xlsx";   //绝对路径
        string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        FileInfo fileinfo = new FileInfo(filePath);
        using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))   //using用来强行做资源释放
        {
            ExcelWorksheet worksheet1 = excelpackge.Workbook.Worksheets[1];
            ExcelWorksheet worksheet2 = excelpackge.Workbook.Worksheets[2];
            GetHeroRarity(worksheet1, "rarity");
            GetHeroRarityId(worksheet2);
        }
    }
    //读取英雄的相关属性
    void GetExcelFile_Nature()
    {
        excelText.Clear();
        //string filePath = "F:/dev/GameCommon/111.xlsx";   //绝对路径
        string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        FileInfo fileinfo = new FileInfo(filePath);
        using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))   //using用来强行做资源释放
        {
            ExcelWorksheet worksheet1 = excelpackge.Workbook.Worksheets[1];
            GetSpecificValue(1, worksheet1, "attack");
            //print(attack);
        }
    }
    //玩家卡牌随机
    void RandomList()
    {
        while (myCard.Count < count)
        {
            switch (level)
            {
                case 1:
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
        if (money > 2)
        {
            money -= 2;
            getCardId.Clear();
            SetPeopleHarets();
            myCard.Clear();
            heroName.Clear();
            //heroBtn.Clear();
            switch (level)
            {
                case 1:
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
                case 9:
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
                default:
                    break;
            }
            GetExcelFile();
            //heroBtn.Clear();
            HeroLocation();
            for (int i = 0; i < myCard.Count; i++)
            {
                heroBtn[i].transform.Find("Image").gameObject.SetActive(false);
                heroBtn[i].GetComponent<Button>().enabled = true;   //开启点击事件
            }
        }
        else
        {
            print("啊，呸，穷鬼");
        }
    }
    //招募显示
    private void HeroLocation()
    {
        for (int i = 0; i < heroName.Count; i++)
        {

            heroBtn[i].GetComponentInChildren<Text>().text = heroName[i].ToString();
            //heroBtn[i].GetComponentInChildren<Text>().tag = getHeroId[i].ToString();
            heroBtn[i].name = getCardId[i].ToString();
        }
    }
    //输入id获取整行数据--招募表
    void GetValueFromId(int num, ExcelWorksheet worksheet)
    {
        int Num = 0;
        string rowTxt = "";
        for (int i = 1; i < 793 + 1; i++)
        {
            for (int j = 1; j < 2 + 1; j++)
            {
                if (j == 2 && i > 1)
                {
                    if (int.Parse(worksheet.Cells[i, j].Value.ToString()) == num)
                    {
                        string n = worksheet.Cells[i, j].GetEnumerator().ToString();
                        for (int x = 0; x < n.Length; x++)
                        {
                            if (x > 0)
                            {
                                rowTxt = rowTxt + n[x];
                            }
                        }
                        Num = int.Parse(rowTxt);
                    }
                }
            }
        }
        excelText.Add(int.Parse(worksheet.Cells[Num, 1].Value.ToString()));
        getCardId.Add(int.Parse(worksheet.Cells[Num, 2].Value.ToString()));
        //print(excelText.Count);
    }
    //根据id和列名拿到具体的值--英雄表
    void GetSpecificValue(int id, ExcelWorksheet worksheet, string name)
    {
        int num = 0;
        string numy = "";
        string rowTxt = "";
        for (int i = 1; i < 89 + 1; i++)
        {
            for (int j = 1; j < 15 + 1; j++)
            {
                if (j == 1 && i > 1)
                {
                    if (int.Parse(worksheet.Cells[i, j].Value.ToString()) == id)
                    {
                        string n = worksheet.Cells[i, j].GetEnumerator().ToString();
                        for (int x = 0; x < n.Length; x++)
                        {
                            if (x > 0)
                            {
                                rowTxt = rowTxt + n[x];
                            }
                        }
                        num = int.Parse(rowTxt);
                    }
                }
                if (i == 1)
                {
                    if (worksheet.Cells[i, j].Value.ToString() == name)
                    {
                        string n = worksheet.Cells[i, j].GetEnumerator().ToString();
                        numy = n[0].ToString();
                    }
                }
            }
        }
        for (int y = 1; y < 15 + 1; y++)
        {
            if (name == "roleName")
            {
                if (worksheet.Cells[num, y].GetEnumerator().ToString() == numy + num.ToString())
                {
                    //print(worksheet.Cells[num, y].Value.ToString());
                    heroName.Add(worksheet.Cells[num, y].Value.ToString());
                }
            }
            if (name == "attack")
            {
                if (worksheet.Cells[num, y].GetEnumerator().ToString() == numy + num.ToString())
                {
                    //print(worksheet.Cells[num, y].Value.ToString());
                    attack = int.Parse(worksheet.Cells[num, y].Value.ToString());
                }
            }
        }
    }

    //把英雄按照稀有度分类
    void GetHeroRarity(ExcelWorksheet worksheet, string name)
    {
        string numy = "";
        for (int i = 1; i < 89 + 1; i++)
        {
            for (int j = 1; j < 15 + 1; j++)
            {
                if (i == 1)
                {
                    if (worksheet.Cells[i, j].Value.ToString() == name)
                    {
                        string n = worksheet.Cells[i, j].GetEnumerator().ToString();
                        numy = n[0].ToString();
                    }
                }
                if (i > 1)
                {
                    if (worksheet.Cells[i, j].GetEnumerator().ToString() == numy + i.ToString())    //拿到稀有度那一列
                    {
                        if (worksheet.Cells[i, j].Value.ToString() == "1")             //拿到绿色
                        {
                            greenHeroId.Add(i - 1);
                        }
                        else if (worksheet.Cells[i, j].Value.ToString() == "2")
                        {
                            blueHeroId.Add(i - 1);
                        }
                        else if (worksheet.Cells[i, j].Value.ToString() == "3")
                        {
                            purpleHeroId.Add(i - 1);
                        }
                        else if (worksheet.Cells[i, j].Value.ToString() == "4")
                        {
                            orangeHeroId.Add(i - 1);
                        }
                    }
                }
            }
        }
    }
    //在表2中拿到相关稀有度的英雄Id并存储
    void GetHeroRarityId(ExcelWorksheet worksheet)
    {
        for (int i = 1; i < 793 + 1; i++)
        {
            if (i > 1)
            {
                for (int y = 0; y < greenHeroId.Count; y++)
                {
                    if (worksheet.Cells[i, 1].Value.ToString() == greenHeroId[y].ToString())
                    {
                        greenCard.Add(int.Parse(worksheet.Cells[i, 2].Value.ToString()));
                    }
                }
                for (int y = 0; y < blueHeroId.Count; y++)
                {
                    if (worksheet.Cells[i, 1].Value.ToString() == blueHeroId[y].ToString())
                    {
                        blueCard.Add(int.Parse(worksheet.Cells[i, 2].Value.ToString()));
                    }
                }
                for (int y = 0; y < purpleHeroId.Count; y++)
                {
                    if (worksheet.Cells[i, 1].Value.ToString() == purpleHeroId[y].ToString())
                    {
                        purpleCard.Add(int.Parse(worksheet.Cells[i, 2].Value.ToString()));
                    }
                }
                for (int y = 0; y < orangeHeroId.Count; y++)
                {
                    if (worksheet.Cells[i, 1].Value.ToString() == orangeHeroId[y].ToString())
                    {
                        orangeCard.Add(int.Parse(worksheet.Cells[i, 2].Value.ToString()));
                    }
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
}