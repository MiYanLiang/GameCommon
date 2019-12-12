using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 战斗控制脚本
/// </summary>
public class FightControll : MonoBehaviour
{
    public static float textDelayTime = 0.1f;   //延迟加载文字
    [SerializeField]
    private float speed_time = 0.3f;
    public static float speedTime;    //卡牌移动速度
    public static int fightPosCha;    //卡牌攻击位置差
    
    [SerializeField]
    Transform backGround;   //用来获取背景上的代码UIControl

    public GameObject hero_Card;    //英雄卡片预制件
    
    [HideInInspector]
    public List<Transform> JiuGongGes; //npc九宫格集合

    //记录敌方势力要发展的兵种类型
    List<int[]> npcUnits = new List<int[]>();
    //记录NPC上阵英雄的数据
    List<List<string>[]> enemyHeroDatas = new List<List<string>[]>();
    //存储某npc需要传递的数据
    List<string>[] sendData = new List<string>[9];
    //记录npc势力血量
    [HideInInspector]
    public int[] npcPlayerHps;
    //npc玩家血条
    [HideInInspector]
    public List<Slider> npcPlayer_hpSliders;

    [HideInInspector]
    public int difnum = 0; //记录难度值

    [SerializeField]
    private int npcLessHpValue = 0; //npc减血加成
    [SerializeField]
    private int minNum = 30; //胜率随机最小值
    [SerializeField]
    private int maxNum = 70; //胜率随机最大值
    //记录npc的胜率
    private int[] npcWinRate;
    //记录npc胜利总数量
    [HideInInspector]
    public int[] allWinTimes;
    [SerializeField]
    Transform textList; //结算战况集合
    public static int playerHeroHps = 0;  //玩家英雄总血量

    //各个战斗控制控件0为玩家
    public Transform[] FightCardSps = new Transform[2];
    //各个战斗画面
    public GameObject[] FightTVs = new GameObject[2];

    private void Awake()
    {
        fightPosCha = (int)(175f / 1920 * Screen.height);
        difnum = PlayerPrefs.GetInt("DifficultyType");
    }

    private void Update()
    {
        //更新卡牌移动速度
        if (speedTime != speed_time)
        {
            speedTime = speed_time;
        }
    }

    /// <summary>
    /// 初始化npc数据（存放链表，阵容等）
    /// </summary>
    public void InitNpcDataList(int npcNums)
    {
        enemyHeroDatas.Clear();
        for (int i = 0; i < npcNums; i++)
        {
            enemyHeroDatas.Add(new List<string>[9]);
        }
        //初始化所有NPC势力发展阵容
        CreateEnemyUnits(npcNums);
        //初始化npc玩家血量
        npcPlayerHps = new int[npcNums];
        for (int i = 0; i < npcNums; i++)
        {
            npcPlayerHps[i] = int.Parse(LoadJsonFile.difficultyChooseDatas[difnum - 1][6]);
        }
        //初始化npc胜利数量集合
        allWinTimes = new int[npcNums];
        //记录npc的胜率
        npcWinRate = new int[npcNums];
        //更新npc血条和血量
        NpcHpListUpdate();

        //加载npc武将上阵
        Invoke("ChangeAllEnemyCards", 0.3f);
    }

    /// <summary>
    /// 更新所有NPC阵容
    /// </summary>
    public void ChangeAllEnemyCards()
    {
        List<string>[] enemyHeroData = new List<string>[9];

        for (int i = 0; i < enemyHeroDatas.Count; i++)
        {
            enemyHeroData = enemyHeroDatas[i];

            for (int j = 0; j < 9; j++)
            {
                if (enemyHeroData[j] != null)
                {
                    sendData[j] = new List<string>();
                    //传递ID,品阶,战斗周目数
                    sendData[j].Add(enemyHeroData[j][0]);
                    sendData[j].Add(enemyHeroData[j][enemyHeroData[j].Count - 2]);
                    sendData[j].Add(enemyHeroData[j][enemyHeroData[j].Count - 1]);
                    enemyHeroData[j].Clear();   //传递后清理
                }
                else
                {
                    sendData[j] = null;
                }
            }
            EmFightControll emFctll = new EmFightControll();
            //npc上阵武将数据更新
            enemyHeroDatas[i] = emFctll.SendHeroData(sendData, npcUnits[i], FightCardSps[0].GetComponent<FightCardSP>().battles - 1, i);
            //清理临时传递的数据
            for (int n = 0; n < sendData.Length; n++)
            {
                if (sendData[n] != null)
                {
                    sendData[n].Clear();
                }
            }
            //清空当前NPC九宫格武将卡牌
            for (int m = 0; m < 9; m++)
            {
                if (JiuGongGes[i].GetChild(m).childCount > 0)
                {
                    Destroy(JiuGongGes[i].GetChild(m).GetChild(0).gameObject);
                }
            }
            //NPC新的武将卡牌上阵九宫格
            for (int m = 0; m < 9; m++)
            {
                if (enemyHeroDatas[i][m] != null)
                {
                    //实例化武将卡牌到备战位,并传递数据过去
                    GameObject newheroCard = Instantiate(hero_Card, JiuGongGes[i].GetChild(m));
                    newheroCard.transform.position = JiuGongGes[i].GetChild(m).position;
                    newheroCard.GetComponent<Image>().raycastTarget = false;    //关闭射线控制拖拽
                    newheroCard.GetComponent<HeroDataControll>().HeroData = enemyHeroDatas[i][m];   //传递卡牌数据
                    newheroCard.GetComponent<HeroDataControll>().Grade_hero = int.Parse(enemyHeroDatas[i][m][enemyHeroDatas[i][m].Count - 2]); //传递阶值
                    newheroCard.GetComponent<HeroDataControll>().HeroData[6] = (int.Parse(LoadJsonFile.RoleTableDatas[int.Parse(newheroCard.GetComponent<HeroDataControll>().HeroData[0]) - 1][6]) * Mathf.Pow(2, int.Parse(enemyHeroDatas[i][m][enemyHeroDatas[i][m].Count - 2]) - 1)).ToString();
                    newheroCard.GetComponent<HeroDataControll>().HeroData[8] = (int.Parse(LoadJsonFile.RoleTableDatas[int.Parse(newheroCard.GetComponent<HeroDataControll>().HeroData[0]) - 1][8]) * Mathf.Pow(2, int.Parse(enemyHeroDatas[i][m][enemyHeroDatas[i][m].Count - 2]) - 1)).ToString();

                    newheroCard.GetComponent<HeroDataControll>().BattleNums = int.Parse(enemyHeroDatas[i][m][enemyHeroDatas[i][m].Count - 1]); //传递战斗回合数
                    newheroCard.transform.GetComponent<Image>().sprite = Resources.Load("Image/ArmsPicture/" + enemyHeroDatas[i][m][3], typeof(Sprite)) as Sprite;  //设置敌方武将兵种背景图片
                    //设置品阶颜色表现和属性
                    newheroCard.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = enemyHeroDatas[i][m][enemyHeroDatas[i][m].Count - 2];
                    //设置左上角兵种文字
                    newheroCard.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = LoadJsonFile.SoldierTypeDates[int.Parse(enemyHeroDatas[i][m][3]) - 1][2];

                    //设置文字颜色，体现卡牌稀有度
                    switch (int.Parse(enemyHeroDatas[i][m][4]))
                    {
                        case 1:
                            //newheroCard.transform.GetChild(0).GetComponent<Text>().color = ColorData.green_Color;  //绿色
                            newheroCard.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = ColorData.green_Color;  //绿色
                            break;
                        case 2:
                            //newheroCard.transform.GetChild(0).GetComponent<Text>().color = ColorData.blue_Color_hero; //蓝色
                            newheroCard.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = ColorData.blue_Color_hero; //蓝色
                            break;
                        case 3:
                            //newheroCard.transform.GetChild(0).GetComponent<Text>().color = ColorData.purple_Color; //紫色
                            newheroCard.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = ColorData.purple_Color; //紫色
                            break;
                        case 4:
                            //newheroCard.transform.GetChild(0).GetComponent<Text>().color = ColorData.red_Color_hero;  //红色
                            newheroCard.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = ColorData.red_Color_hero;  //红色
                            break;
                    }
                    //显示英雄名等信息
                    newheroCard.transform.GetChild(0).GetComponent<Text>().text = enemyHeroDatas[i][m][1];
                    newheroCard.transform.GetChild(1).GetComponent<Text>().text = enemyHeroDatas[i][m][6];
                }
            }
        }
    }
    
    public int selectForce = -1;   //为守
    public void ChangeSelectForce(int index)
    {
        selectForce = index;
    }

    /// <summary>
    /// 战斗npc之间的结算
    /// </summary>
    public void BattleSettlement()
    {
        int npcCount = npcWinRate.Length;
        int index_list = 1;
        int[] enemyForceIndex = new int[npcCount];//记录每个npc匹配到的对手势力
        //重置npc胜率
        for (int i = 0; i < npcCount; i++)
        {
            npcWinRate[i] = Random.Range(minNum, maxNum);
            enemyForceIndex[i] = -1;
        }
        //挑选对手
        for (int i = 0; i < npcCount; i++)
        {
            if (npcPlayerHps[i] > 0)
            {
                int enem = Random.Range(0, npcCount);  //最后一个为玩家自身
                while (true)
                {
                    if (enem == npcCount - 1)
                        break;
                    if (enem != i && npcPlayerHps[enem] > 0)
                        break;
                    enem = Random.Range(0, npcCount);
                }
                enemyForceIndex[i] = enem;
                if (enem == npcCount - 1)
                {
                    if (npcWinRate[i] <= 50)  //小于等于50则输
                    {
                        //输了扣血
                        npcPlayerHps[i] -= Random.Range(1 + npcLessHpValue, 11 + npcLessHpValue);
                        //显示战况信息
                        if (FightCardSps[0].GetComponent<FightCardSP>().isSpecialLevel)
                        {
                            //textList.GetChild(index_list).GetComponent<Text>().text = string.Format("<color=#332D2D>{0}</color>        <color=#E04638>{1}</color>        <color=#FFF0F5>{2}</color>", LoadJsonFile.forcesTableDatas[UIControl.enemy_forces[i] - 1][1], "败", LoadJsonFile.NPCTableDates[FightCardSps[0].GetComponent<FightCardSP>().specialLevelId][15]);
                        }
                        else
                        {
                            //textList.GetChild(index_list).GetComponent<Text>().text = string.Format("<color=#332D2D>{0}</color>        <color=#E04638>{1}</color>        <color=#CDCDCD>{2}</color>", LoadJsonFile.forcesTableDatas[UIControl.enemy_forces[i] - 1][1], "败", LoadJsonFile.forcesTableDatas[UIControl.playerForceId - 1][1]);
                        }
                    }
                    else
                    {
                        //赢了加胜利场数
                        allWinTimes[i]++;
                        if (FightCardSps[0].GetComponent<FightCardSP>().isSpecialLevel)
                        {
                            //textList.GetChild(index_list).GetComponent<Text>().text = string.Format("<color=#332D2D>{0}</color>        <color=#57A65F>{1}</color>        <color=#FFF0F5>{2}</color>", LoadJsonFile.forcesTableDatas[UIControl.enemy_forces[i] - 1][1], "胜", LoadJsonFile.NPCTableDates[FightCardSps[0].GetComponent<FightCardSP>().specialLevelId][15]);
                        }
                        else
                        {
                            //textList.GetChild(index_list).GetComponent<Text>().text = string.Format("<color=#332D2D>{0}</color>        <color=#57A65F>{1}</color>        <color=#CDCDCD>{2}</color>", LoadJsonFile.forcesTableDatas[UIControl.enemy_forces[i] - 1][1], "胜", LoadJsonFile.forcesTableDatas[UIControl.playerForceId - 1][1]);
                        }
                    }
                }
                else
                {
                    if (npcWinRate[i] <= 50)
                    {
                        npcPlayerHps[i] -= Random.Range(1 + npcLessHpValue, 11 + npcLessHpValue);
                        if (FightCardSps[0].GetComponent<FightCardSP>().isSpecialLevel)
                        {
                            //textList.GetChild(index_list).GetComponent<Text>().text = string.Format("<color=#332D2D>{0}</color>        <color=#E04638>{1}</color>        <color=#FFF0F5>{2}</color>", LoadJsonFile.forcesTableDatas[UIControl.enemy_forces[i] - 1][1], "败", LoadJsonFile.NPCTableDates[FightCardSps[0].GetComponent<FightCardSP>().specialLevelId][15]);
                        }
                        else
                        {
                            //textList.GetChild(index_list).GetComponent<Text>().text = string.Format("<color=#332D2D>{0}</color>        <color=#E04638>{1}</color>        <color=#332D2D>{2}</color>", LoadJsonFile.forcesTableDatas[UIControl.enemy_forces[i] - 1][1], "败", LoadJsonFile.forcesTableDatas[UIControl.enemy_forces[enem] - 1][1]);
                        }
                    }
                    else
                    {
                        allWinTimes[i]++;
                        if (FightCardSps[0].GetComponent<FightCardSP>().isSpecialLevel)
                        {
                            //textList.GetChild(index_list).GetComponent<Text>().text = string.Format("<color=#332D2D>{0}</color>        <color=#57A65F>{1}</color>        <color=#FFF0F5>{2}</color>", LoadJsonFile.forcesTableDatas[UIControl.enemy_forces[i] - 1][1], "胜", LoadJsonFile.NPCTableDates[FightCardSps[0].GetComponent<FightCardSP>().specialLevelId][15]);
                        }
                        else
                        {
                            //textList.GetChild(index_list).GetComponent<Text>().text = string.Format("<color=#332D2D>{0}</color>        <color=#57A65F>{1}</color>        <color=#332D2D>{2}</color>", LoadJsonFile.forcesTableDatas[UIControl.enemy_forces[i] - 1][1], "胜", LoadJsonFile.forcesTableDatas[UIControl.enemy_forces[enem] - 1][1]);
                        }
                    }
                }
                index_list++;
            }
        }
    }

    /// <summary>
    /// 更新npc玩家血条
    /// </summary>
    private void NpcHpListUpdate()
    {
        //清空战况信息
        for (int i = 0; i < textList.childCount; i++)
        {
            textList.GetChild(i).GetComponent<Text>().text = "";
        }
        //更新血条
        int sliderCount = npcPlayer_hpSliders.Count;
        int allHp = int.Parse(LoadJsonFile.difficultyChooseDatas[difnum - 1][6]);
        int indexHp = 0;
        for (int i = 0; i < sliderCount; i++)
        {
            npcPlayer_hpSliders[i].value = npcPlayerHps[i] / (float)allHp;
            indexHp = int.Parse(npcPlayer_hpSliders[i].transform.GetChild(3).GetComponent<Text>().text);
            npcPlayer_hpSliders[i].transform.GetChild(3).GetComponent<Text>().text = ((npcPlayerHps[i] >= 0) ? npcPlayerHps[i] : 0).ToString();
            npcPlayer_hpSliders[i].transform.GetChild(3).GetChild(0).GetComponent<cutHpTextMove>().content_text = indexHp - ((npcPlayerHps[i] >= 0) ? npcPlayerHps[i] : 0) + "";  //设置城池播放扣血文字内容
        }
    }

    /// <summary>
    /// 点击开始战斗，传递数据到战斗界面
    /// </summary>
    public void StartFightSendHeroData()
    {
        int npcCount = npcPlayerHps.Length;

        int rivalId = Random.Range(0, npcCount);    //随机挑选一个对手

        if (selectForce != -1 && npcPlayerHps[selectForce] > 0)  //选择敌对势力
        {
            rivalId = selectForce;
        }
        else
        {
            //防守战时，玩家的敌人随机
            while (true)
            {
                if (npcPlayerHps[rivalId] > 0)
                    break;
                else
                    rivalId = Random.Range(0, npcCount);
            }
        }

        FightCardSps[0].GetComponent<FightCardSP>().array_str = enemyHeroDatas[rivalId];
        FightCardSps[0].GetComponent<FightCardSP>().enemyForceId = rivalId;
        FightCardSps[0].gameObject.SetActive(true);
    }

    //打开或关闭 玩家 的战斗界面
    public void OpenOrCloseFightTV(bool boo)
    {
        if (boo == false)
            NpcHpListUpdate();
        FightTVs[0].gameObject.SetActive(boo);
    }

    //初始化创建敌方势力和后期要发展的兵种类型
    private void CreateEnemyUnits(int npcNums)
    {
        for (int i = 0; i < npcNums; i++)
        {
            int[] arr = new int[3];
            arr[0] = Random.Range(1, 4);    //前排
            arr[1] = Random.Range(4, 7);    //中排
            arr[2] = Random.Range(7, 10);   //后排
            npcUnits.Add(arr);
        }
    }

    /// <summary>
    /// 结束游戏，跳转界面
    /// </summary>
    public void EndOfGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    /// <summary>
    /// 获取某npc当前战力值
    /// </summary>
    public int GetNowNPCPowerNums(int index)
    {
        int num = 0;
        for(int i = 0; i < JiuGongGes[index].childCount; i++)
        {
            if(JiuGongGes[index].GetChild(i).childCount > 0)
            {
                num += int.Parse(JiuGongGes[index].GetChild(i).GetChild(0).GetComponent<HeroDataControll>().HeroData[6]);
            }
        }
        return num;
    }
}
