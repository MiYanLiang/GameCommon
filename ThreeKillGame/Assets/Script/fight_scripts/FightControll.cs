﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 战斗控制脚本
/// </summary>
public class FightControll : MonoBehaviour
{

    //private int playerForceId;    //玩家势力的ID
    //private int[] forceIds;     //记录其他势力的ID
    [SerializeField]
    private float speed_time = 0.3f;
    public static float speedTime;    //卡牌移动速度
    public static int fightPosCha;    //卡牌攻击位置差

    //public Text[] forceNames;   //游戏内势力单个字显示
    [SerializeField]
    Transform backGround;   //用来获取背景上的代码UIControl
    [SerializeField]
    Image playerForceFlag;    //玩家势力头像
    [SerializeField]
    Image rivalForceFlag;    //其他势力头像

    public GameObject hero_Card;    //英雄卡片预制件
    //各个npc上阵九宫格
    public Transform[] JiuGongGes = new Transform[5];
    //各个战斗控制控件0为玩家
    public Transform[] FightCardSps=new Transform[6];
    //各个战斗画面
    public GameObject[] FightTVs = new GameObject[6];
    //记录敌方势力要发展的兵种类型
    int[][] enemyUnits = new int[5][];
    //记录NPC上阵英雄的数据
    static List<List<string>[]> enemyHeroDatas = new List<List<string>[]>(); 
    static List<string>[] sendData = new List<string>[9];     //存储需要传递的数据
    [HideInInspector]
    public int difnum = 0; //记录难度值
    [SerializeField]
    private int npcLessHpValue = 0; //npc减血加成
    public int[] allWinTimes = new int[5] { 0, 0, 0, 0, 0 };  //npc胜利总数量
    [SerializeField]
    private int minNum = 30; //胜率随机最小值
    [SerializeField]
    private int maxNum = 70; //胜率随机最大值
    [SerializeField]
    private int[] npcWinRate = new int[5] { 0, 0, 0, 0, 0 };   //npc的默认胜率0-100
    public int[] npcPlayerHps = new int[5];     //npc玩家血量
    public static int playerHeroHps = 0;  //玩家英雄总血量
    [SerializeField]
    Slider[] npcPlayer_hp;//npc玩家血条
    [SerializeField]
    Transform textList; //结算战况集合

    private void Awake()
    {
        fightPosCha = (int)(175f / 1920 * Screen.height);

        enemyHeroDatas.Clear();
        //初始化npc卡牌存储列表
        for (int i = 0; i < 5; i++)
        {
            enemyHeroDatas.Add(new List<string>[9]);
        }
        for (int i = 0; i < 9; i++)
        {
            sendData[i] = new List<string>();
        }
    }

    private void Start()
    {
        difnum = PlayerPrefs.GetInt("DifficultyType");
        //更新npc玩家血条
        for (int i = 0; i < 5; i++)
        {
            npcPlayerHps[i] = int.Parse(LoadJsonFile.difficultyChooseDatas[difnum - 1][6]);
        }
        NpcHpListUpdate();

        CreateEnemyUnits(); //初始化所有NPC势力发展阵容

        Invoke("ChangeAllEnemyCards", 0.3f);    //延时加载npc武将
        //ChangeAllEnemyCards();
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
    /// 更新所有NPC阵容
    /// </summary>
    public void ChangeAllEnemyCards()
    {
        List<string>[] enemyHeroData = new List<string>[9];

        for (int i = 0; i < enemyHeroDatas.Count; i++)
        {
            enemyHeroData = enemyHeroDatas[i];

            for (int j  = 0; j < 9; j++)
            {
                if (enemyHeroData[j] != null)
                {
                    sendData[j] = new List<string>();
                    //传递ID,品阶,战斗周目数
                    sendData[j].Add(enemyHeroData[j][0]);
                    sendData[j].Add(enemyHeroData[j][enemyHeroData[j].Count - 2]);
                    sendData[j].Add(enemyHeroData[j][enemyHeroData[j].Count - 1]);
                    //Debug.Log("heroid:" + sendData[j][0] + "   herograde:" + sendData[j][1]+"   fightNums:"+sendData[j][2]);
                    enemyHeroData[j].Clear();   //传递后清理
                }
                else
                {
                    sendData[j] = null;
                }
            }
            EmFightControll emFctll=new EmFightControll();
            //npc上阵武将数据更新
            enemyHeroDatas[i]= emFctll.SendHeroData(sendData, enemyUnits[i], FightCardSps[0].GetComponent<FightCardSP>().battles - 1);
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
                if (enemyHeroDatas[i][m]!=null)
                {
                    //实例化武将卡牌到备战位,并传递数据过去
                    GameObject newheroCard = Instantiate(hero_Card, JiuGongGes[i].GetChild(m));
                    newheroCard.transform.position = JiuGongGes[i].GetChild(m).position;
                    newheroCard.GetComponent<Image>().raycastTarget = false;    //关闭射线控制拖拽
                    newheroCard.GetComponent<HeroDataControll>().HeroData = enemyHeroDatas[i][m];   //传递卡牌数据
                    newheroCard.GetComponent<HeroDataControll>().Grade_hero = int.Parse(enemyHeroDatas[i][m][enemyHeroDatas[i][m].Count-2]); //传递阶值
                    newheroCard.GetComponent<HeroDataControll>().HeroData[6] = (int.Parse(LoadJsonFile.RoleTableDatas[int.Parse(newheroCard.GetComponent<HeroDataControll>().HeroData[0]) - 1][6]) * Mathf.Pow(2, int.Parse(enemyHeroDatas[i][m][enemyHeroDatas[i][m].Count - 2])-1)).ToString();
                    newheroCard.GetComponent<HeroDataControll>().HeroData[8] = (int.Parse(LoadJsonFile.RoleTableDatas[int.Parse(newheroCard.GetComponent<HeroDataControll>().HeroData[0]) - 1][8]) * Mathf.Pow(2, int.Parse(enemyHeroDatas[i][m][enemyHeroDatas[i][m].Count - 2])-1)).ToString();


                    newheroCard.GetComponent<HeroDataControll>().BattleNums = int.Parse(enemyHeroDatas[i][m][enemyHeroDatas[i][m].Count-1]); //传递战斗回合数
                    newheroCard.transform.GetComponent<Image>().sprite = Resources.Load("Image/ArmsPicture/" + enemyHeroDatas[i][m][3], typeof(Sprite)) as Sprite;  //设置敌方武将兵种背景图片
                    //设置品阶颜色表现和属性
                    newheroCard.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = enemyHeroDatas[i][m][enemyHeroDatas[i][m].Count - 2];
                    //设置左上角兵种文字
                    newheroCard.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = LoadJsonFile.SoldierTypeDates[int.Parse(enemyHeroDatas[i][m][3]) - 1][2];

                    //设置文字颜色，体现卡牌稀有度
                    switch (int.Parse(enemyHeroDatas[i][m][4]))
                    {
                        case 1:
                            newheroCard.transform.GetChild(0).GetComponent<Text>().color = new Color(49f/ 255f, 193f / 255f, 82f / 255f, 1);  //绿色
                            break;
                        case 2:
                            newheroCard.transform.GetChild(0).GetComponent<Text>().color = new Color(48f / 255f, 127f / 255f, 192f / 255f, 1); //蓝色
                            break;
                        case 3:
                            newheroCard.transform.GetChild(0).GetComponent<Text>().color = new Color(215f / 255f, 37f / 255f, 236f / 255f, 1); //紫色
                            break;
                        case 4:
                            newheroCard.transform.GetChild(0).GetComponent<Text>().color = new Color(227f / 255f, 16f / 255f, 16f / 255f, 1);  //红色
                            break;
                    }
                    //显示英雄名等信息
                    //newheroCard.transform.GetChild(0).GetComponent<Text>().text = enemyHeroDatas[i][m][0] + ":" + enemyHeroDatas[i][m][1];
                    newheroCard.transform.GetChild(0).GetComponent<Text>().text = enemyHeroDatas[i][m][1];
                    newheroCard.transform.GetChild(1).GetComponent<Text>().text = enemyHeroDatas[i][m][6];
                    //newheroCard.transform.GetChild(2).GetComponent<Text>().text = enemyHeroDatas[i][m][7];
                }
            }
        }
    }

    /// <summary>
    /// 点击开始战斗，传递数据到战斗界面
    /// </summary>
    public void StartFightSendHeroData()
    {
        int rivalId= Random.Range(0, 5);    //随机挑选对手0,1,2,3,4
        //玩家的敌人设置
        while (true)
        {
            if (npcPlayerHps[rivalId]>0)
                break;
            else
                rivalId = Random.Range(0, 5);
        }
        FightCardSps[0].GetComponent<FightCardSP>().array_str = enemyHeroDatas[rivalId];
        FightCardSps[0].GetComponent<FightCardSP>().enemyForceId = rivalId;
        playerForceFlag.sprite = Resources.Load("Image/calligraphy/" + UIControl.playerForceId, typeof(Sprite)) as Sprite;           //设置玩家势力的头像
        rivalForceFlag.sprite = Resources.Load("Image/calligraphy/" + UIControl.array_forces[rivalId], typeof(Sprite)) as Sprite;    //设置对手势力的头像

        FightCardSps[0].gameObject.SetActive(true);
    }



    /// <summary>
    /// 战斗npc之间的结算
    /// </summary>
    public void BattleSettlement()
    {
        int index_list = 1;
        int[] enemyForceIndex = new int[5] { -1, -1, -1, -1, -1 };  //记录每个npc匹配到的对手势力
        //重置npc胜率
        for (int i = 0; i < 5; i++)
        {
            npcWinRate[i] = Random.Range(minNum, maxNum);   
        }
        //挑选对手
        for (int i = 0; i < 5; i++)
        {
            if (npcPlayerHps[i] > 0)
            {
                int enem = Random.Range(0, 6);  //5为玩家自身
                while (true)
                {
                    if (enem == 5)
                        break;
                    if (enem != i && npcPlayerHps[enem] > 0)
                        break;
                    enem = Random.Range(0, 6);
                }
                enemyForceIndex[i] = enem;

                if (enem == 5)
                {
                    if (npcWinRate[i] < 50)  //小于等于50则输
                    {
                        //输了扣血
                        npcPlayerHps[i] -= Random.Range(1+ npcLessHpValue, 11+ npcLessHpValue);
                        //显示战况信息
                        textList.GetChild(index_list).GetComponent<Text>().text = string.Format("<color=#332D2D>{0}</color>        <color=#E04638>{1}</color>        <color=#CDCDCD>{2}</color>", LoadJsonFile.forcesTableDatas[UIControl.array_forces[i]-1][1],"败", LoadJsonFile.forcesTableDatas[UIControl.playerForceId - 1][1]);
                    }
                    else
                    {
                        //赢了加胜利场数
                        allWinTimes[i]++;
                        textList.GetChild(index_list).GetComponent<Text>().text = string.Format("<color=#332D2D>{0}</color>        <color=#57A65F>{1}</color>        <color=#CDCDCD>{2}</color>", LoadJsonFile.forcesTableDatas[UIControl.array_forces[i]-1][1],"胜", LoadJsonFile.forcesTableDatas[UIControl.playerForceId - 1][1]);
                    }
                }
                else
                {
                    if (npcWinRate[i] <= 50)
                    {
                        npcPlayerHps[i] -= Random.Range(1+ npcLessHpValue, 11+ npcLessHpValue);
                        textList.GetChild(index_list).GetComponent<Text>().text = string.Format("<color=#332D2D>{0}</color>        <color=#E04638>{1}</color>        <color=#332D2D>{2}</color>", LoadJsonFile.forcesTableDatas[UIControl.array_forces[i] - 1][1], "败", LoadJsonFile.forcesTableDatas[UIControl.array_forces[enem] - 1][1]);
                    }
                    else
                    {
                        allWinTimes[i]++;
                        textList.GetChild(index_list).GetComponent<Text>().text = string.Format("<color=#332D2D>{0}</color>        <color=#57A65F>{1}</color>        <color=#332D2D>{2}</color>", LoadJsonFile.forcesTableDatas[UIControl.array_forces[i] - 1][1], "胜", LoadJsonFile.forcesTableDatas[UIControl.array_forces[enem] - 1][1]);
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
        int allHp = int.Parse(LoadJsonFile.difficultyChooseDatas[difnum - 1][6]);
        for (int i = 0; i < 5; i++)
        {
            npcPlayer_hp[i].value = npcPlayerHps[i] / (float)allHp;
            npcPlayer_hp[i].transform.GetChild(3).GetComponent<Text>().text = ((npcPlayerHps[i] >= 0) ? npcPlayerHps[i] : 0).ToString();
        }
    }


    //打开或者关闭战斗界面
    public void OpenOrCloseFightTV(bool boo)
    {
        if (boo == false)
            NpcHpListUpdate();
        FightTVs[0].gameObject.SetActive(boo);
        //for (int i = 0; i < 6; i++)
        //{
        //    FightTVs[i].gameObject.SetActive(boo);
        //}
    }

    //初始化创建敌方势力和后期要发展的兵种类型
    private void CreateEnemyUnits()
    {
        for (int i = 0; i < 5; i++)
        {
            enemyUnits[i] = new int[3];
            enemyUnits[i][0] = Random.Range(1, 4);     //前排
            enemyUnits[i][1] = Random.Range(4, 7);    //中排
            enemyUnits[i][2] = Random.Range(7, 10);    //后排
            //Debug.Log("///" + enemyUnits[i][0] + "///" + enemyUnits[i][1] + "////" + enemyUnits[i][2]);
            
        }
    }

    /// <summary>
    /// 结束游戏，跳转界面
    /// </summary>
    public void EndOfGame()
    {

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
