using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightCardSP : MonoBehaviour
{
    [SerializeField]
    Slider player_hp;//玩家血条
    [SerializeField]
    Slider player_hp2;//玩家血条2

    [SerializeField]
    private GameObject SettlementPic;   //结算页面

    private bool isEndOFInit = false;   //记录是否初始化结束
    [SerializeField]
    public Text battle_Text;
    [HideInInspector]
    public int battles;       //记录对战次数（周目数）
    private int winBattles;   //记录胜利场数
    private int roundNum;     //记录当前回合数量
    private int fightNum;     //记录当前攻击武将的位置
    private bool isPlayerBout;//记录是否是玩家的武将攻击回合
    [HideInInspector]
    public static bool isFightNow;  //记录现在是否正在攻击

    public Transform jiugongge_BrforeFight;  //上阵布阵的九宫格
    public GameObject heroFightCard;    //英雄战斗卡片预制件
    public GameObject stateIcon;        //卡牌上状态小标预制件
    public Transform[] OwnJiuGonggePos = new Transform[9];  //战斗槽位置_玩家
    public Transform[] enemyJiuGonggePos = new Transform[9];    //战斗槽位置_敌人

    [HideInInspector]
    public List<string>[] array_str = new List<string>[9]; //接收传递的敌方上阵英雄数据
    [HideInInspector]
    public int enemyForceId;    //敌方势力id

    [HideInInspector]
    public List<string>[] arrayNPC_str = new List<string>[9]; //接收传递的特殊敌方阵容
    [HideInInspector]
    public bool isSpecialLevel = false; //记录是否是特殊关卡
    [HideInInspector]
    public int specialLevelId; //记录特殊关卡ID

    public static GameObject[] enemyCards = new GameObject[9];//存储敌人卡牌

    public static GameObject[] playerCards = new GameObject[9];//存储己方卡牌

    [SerializeField]
    Transform fightControll;    //战斗控制代码

    [SerializeField]
    FightControll fightCtl;     //玩家战斗控制代码
    [SerializeField]
    GameObject gameOverBg;      //游戏结束界面

    private int selectEnemy = -1;   //记录要攻击的敌人编号

    private int[] armsSkillStatus = new int[9]; //存储兵种技能激活状态 0-未激活; 1-激活3兵种; 2-激活6兵种

    bool isStart = false;    //记录回合开始结束

    [SerializeField]
    private float roundWaitTime = 3f;   //回合停顿时间
    public Text roundTextObj;   //展示回合text

    [SerializeField]
    Image playerForceFlag;    //玩家势力头像
    [SerializeField]
    Image rivalForceFlag;    //其他势力头像
    [SerializeField]
    Text specialFightText;  //战役名

    [SerializeField]
    CreateAndUpdate createAndUpdate; //刷新脚本：CreateAndUpdate

    [SerializeField]
    GameObject getOrloseText;   //获得或者失去文本

    private int battleId;   //开局所选战役ID

    [SerializeField]
    Transform playerForceCity; //玩家城池

    [SerializeField]
    ChangeAndGet changeAndGet;

    [SerializeField]
    UIControl uiControl;

    private List<int> npcForceFightPy;

    private float[] playerCardHpPercentage;   //记录玩家卡牌血量百分比

    [SerializeField]
    Transform bigMapObj;
    [SerializeField]
    Transform chooseFightStateObj;
    [SerializeField]
    Transform vsTitleObj;

    [SerializeField]
    GameObject[] needHideObj;

    private int avoidWarNums;   //免战次数
    private bool isMianZhan;    //是否选择免战

    private void Awake()
    {
        battleId = PlayerPrefs.GetInt("battleId");
        battles = 1;
        winBattles = 0;
    }

    private void OnEnable()
    {
        isMianZhan = false;

        avoidWarNums = 1;

        //ControllMonthEvent.instance.AddShowMonthEvent(true, "");

        isStart = false;
        isEndOFInit = false;

        fightNum = 0;
        roundNum = 1;
        isPlayerBout = true;
        UIControl.yearsIndex += 1;
        battles += 1;
        battle_Text.text = "公元" + (battles + int.Parse(LoadJsonFile.BattleTableDates[battleId][4]) - 1) + "年";
        InitArmsSkillStatus();
        //敌人初始化
        InitializeBattleCard();

        playerCardHpPercentage = new float[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };

        //上阵初始化
        OtherInitialization();

        InitForceFlag();

        npcForceFightPy = fightControll.GetComponent<FightControll>().BattleSettlement();
        
        isFightNow = false;
        //roundTextObj.text = "回合 " + roundNum;
        //roundTextObj.gameObject.SetActive(true);
        Invoke("LiteTimeStart", roundWaitTime);
        chooseFightStateObj.gameObject.SetActive(true);
        OpenOrCloseObj(false);
    }

    private bool isOver = false;    //记录是否结束这轮战斗

    /// <summary>
    /// 玩家战斗总控制
    /// </summary>
    void Update()
    {
        //若还在初始化
        if (!isEndOFInit)
            return;

        //回合增加（判断游戏是否结束）
        if (fightNum >= playerCards.Length)
        {
            for (int i = 0; i < 9; i++)
            {
                if (enemyCards[i] != null && enemyCards[i].GetComponent<CardMove>().Health > 0)
                {
                    isOver = false;
                    break;
                }
                else
                {
                    isOver = true;
                }
            }
            if (isOver == false)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (playerCards[i] != null && playerCards[i].GetComponent<CardMove>().Health > 0)
                    {
                        isOver = false;
                        break;
                    }
                    else
                    {
                        isOver = true;
                    }
                }
            }

            //DrumSkillControll.drumNums += 1;    //每回合增加击鼓次数
            DrumSkillControll.isChange = true;
            Debug.Log("///回合" + roundNum + "结束///");
            if (!isOver)
            {
                Debug.Log("sadasdas");
                roundNum++;
                roundTextObj.text = "回合 " + roundNum;
                roundTextObj.gameObject.SetActive(true);
            }

            fightNum = 0;

            isStart = false;
            Invoke("LiteTimeStart", roundWaitTime);
        }

        //回合开始
        if (!isStart)
            return;

        //若有武将正在攻击
        if (isFightNow)
            return;

        //  玩家卡牌槽此位置有卡牌            此卡牌位为先手攻击                                                 此卡牌为后手                                                       敌方此卡牌位无卡牌                敌方在此位置的卡牌血量小于零                                  玩家此卡牌位卡牌武将血量不为零
        if (playerCards[fightNum] != null && (playerCards[fightNum].GetComponent<CardMove>().IsAttack_first || (!playerCards[fightNum].GetComponent<CardMove>().IsAttack_first && (enemyCards[fightNum] == null || enemyCards[fightNum].GetComponent<CardMove>().Health <= 0))) && playerCards[fightNum].GetComponent<CardMove>().Health > 0)
        {
            if (!playerCards[fightNum].GetComponent<CardMove>().isFightInThisBout)
            {
                isPlayerBout = true;
                //先找到需要攻击的敌人并传递敌人编号
                selectEnemy = -1;
                playerCards[fightNum].GetComponent<CardMove>().EnemyObj = FindAnalogue(fightNum);
                playerCards[fightNum].GetComponent<CardMove>().EnemyIndex = (selectEnemy != -1) ? selectEnemy : fightNum;
                //获取当前是否有特殊攻击状态
                playerCards[fightNum].GetComponent<CardMove>().IsHadSpecialState = playerCards[fightNum].GetComponent<CardMove>().Fight_State.GetHadSpState();
                //切换武将状态为正在攻击
                playerCards[fightNum].GetComponent<CardMove>().IsAttack = StateOfAttack.FightNow;
                isFightNow = true;
                ChangeParent(playerCards[fightNum].transform);
                //记录当前武将在该回合已进行过攻击
                playerCards[fightNum].GetComponent<CardMove>().isFightInThisBout = true;
            }
            else
            {
                if (enemyCards[fightNum] != null && enemyCards[fightNum].GetComponent<CardMove>().Health > 0)
                {
                    isPlayerBout = false;
                    enemyCards[fightNum].GetComponent<CardMove>().EnemyObj = FindAnalogue(fightNum);
                    enemyCards[fightNum].GetComponent<CardMove>().EnemyIndex = (selectEnemy != -1) ? selectEnemy : fightNum;
                    //获取当前是否有特殊攻击状态
                    enemyCards[fightNum].GetComponent<CardMove>().IsHadSpecialState = enemyCards[fightNum].GetComponent<CardMove>().Fight_State.GetHadSpState();
                    //切换武将状态为正在攻击
                    enemyCards[fightNum].GetComponent<CardMove>().IsAttack = StateOfAttack.FightNow;
                    isFightNow = true;
                    ChangeParent(enemyCards[fightNum].transform);
                }
                playerCards[fightNum++].GetComponent<CardMove>().isFightInThisBout = false;
            }
            return;
        }

        if (enemyCards[fightNum] != null && (enemyCards[fightNum].GetComponent<CardMove>().IsAttack_first || (!enemyCards[fightNum].GetComponent<CardMove>().IsAttack_first && (playerCards[fightNum] == null || playerCards[fightNum].GetComponent<CardMove>().Health <= 0))) && enemyCards[fightNum].GetComponent<CardMove>().Health > 0)
        {
            if (!enemyCards[fightNum].GetComponent<CardMove>().isFightInThisBout)
            {
                isPlayerBout = false;
                selectEnemy = -1;
                enemyCards[fightNum].GetComponent<CardMove>().EnemyObj = FindAnalogue(fightNum);
                enemyCards[fightNum].GetComponent<CardMove>().EnemyIndex = (selectEnemy != -1) ? selectEnemy : fightNum;
                //获取当前是否有特殊攻击状态
                enemyCards[fightNum].GetComponent<CardMove>().IsHadSpecialState = enemyCards[fightNum].GetComponent<CardMove>().Fight_State.GetHadSpState();
                enemyCards[fightNum].GetComponent<CardMove>().IsAttack = StateOfAttack.FightNow;
                isFightNow = true;
                ChangeParent(enemyCards[fightNum].transform);
                enemyCards[fightNum].GetComponent<CardMove>().isFightInThisBout = true;
            }
            else
            {
                if (playerCards[fightNum] != null && playerCards[fightNum].GetComponent<CardMove>().Health > 0)
                {
                    isPlayerBout = true;
                    playerCards[fightNum].GetComponent<CardMove>().EnemyObj = FindAnalogue(fightNum);
                    playerCards[fightNum].GetComponent<CardMove>().EnemyIndex = (selectEnemy != -1) ? selectEnemy : fightNum;
                    //获取当前是否有特殊攻击状态
                    playerCards[fightNum].GetComponent<CardMove>().IsHadSpecialState = playerCards[fightNum].GetComponent<CardMove>().Fight_State.GetHadSpState();
                    playerCards[fightNum].GetComponent<CardMove>().IsAttack = StateOfAttack.FightNow;
                    isFightNow = true;
                    ChangeParent(playerCards[fightNum].transform);
                }
                enemyCards[fightNum++].GetComponent<CardMove>().isFightInThisBout = false;
            }
            return;
        }
        fightNum++;
    }

    /// <summary>
    /// 找到要攻击的对手
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    private GameObject FindAnalogue(int i)
    {
        if (isPlayerBout)
        {
            //前排位置可以首选同位置对手牌
            if (fightNum < 3 && enemyCards[fightNum] != null && enemyCards[fightNum].GetComponent<CardMove>().Health > 0)
            {
                return enemyCards[fightNum];
            }
            else
            {
                if (fightNum >= 3 && enemyCards[fightNum % 3] != null && enemyCards[fightNum % 3].GetComponent<CardMove>().Health > 0)
                {
                    selectEnemy = fightNum % 3;
                    return enemyCards[fightNum % 3];
                }
                else
                {
                    selectEnemy = 0;
                    while (enemyCards[selectEnemy] == null || enemyCards[selectEnemy].GetComponent<CardMove>().Health <= 0)
                    {
                        selectEnemy++;
                        if (selectEnemy >= enemyCards.Length)
                        {

                            Debug.Log("玩家获胜");
                            winBattles++;   //胜利场数加一
                                            //结算战斗信息
                                            //血量
                            int remainingHP = 0;    //总血量
                            int fullHP = 0;         //剩余血量
                            for (int j = 0; j < 9; j++)
                            {
                                if (playerCards[j] != null)
                                {
                                    fullHP += playerCards[j].GetComponent<CardMove>().Fullhealth;
                                    remainingHP += playerCards[j].GetComponent<CardMove>().Health;
                                }
                            }
                            float remainScale = (float)remainingHP / fullHP;    //玩家剩余血量比例

                            int cutHp = 0;
                            cutHp = (int)(remainScale * fightCtl.maxCutHp);                                                    //敌方扣血
                            cutHp = cutHp > fightCtl.defCutHp ? cutHp : fightCtl.defCutHp;
                            fightCtl.npcPlayerHps[enemyForceId] -= cutHp;

                            if (fightCtl.npcPlayerHps[enemyForceId] <= 0)
                            {
                                string str1 = string.Format("玩家消灭 {0} 的{1}势力", LoadJsonFile.forcesTableDatas[UIControl.enemy_forces[enemyForceId] - 1][1], LoadJsonFile.forcesTableDatas[UIControl.enemy_forces[enemyForceId] - 1][4]);
                                ControllMonthEvent.instance.AddShowMonthEvent(str1);
                                //Debug.Log(string.Format("玩家消灭 {0} 势力", LoadJsonFile.forcesTableDatas[UIControl.enemy_forces[enemyForceId] - 1][1]));
                                changeAndGet.GetEnemyStartHero(enemyForceId);   //消灭势力，得到其初始武将
                            }

                            int addMoney = 0;
                            if (fightCtl.selectForce != -1)
                            {
                                addMoney = (int)(remainScale * fightCtl.maxAddGold);
                                addMoney = addMoney > fightCtl.defAddGold ? addMoney : fightCtl.defAddGold;
                            }
                            else
                            {
                                addMoney = fightCtl.defAddGold;
                            }
                            CreateAndUpdate.money += addMoney;   //玩家加金币
                            createAndUpdate.ShowDyCutOrAddGold(true, addMoney);

                            player_hp2.transform.GetChild(3).GetChild(0).GetComponent<cutHpTextMove>().content_text = "";  //设置城池播放扣血文字内容

                            string str = "";
                            //展示战斗胜负信息
                            if (remainScale >= 0.8f)
                            {
                                SettlementPic.transform.GetChild(3).GetComponent<Image>().sprite = Resources.Load("Image/calligraphy/BattleEnding/完", typeof(Sprite)) as Sprite;
                                SettlementPic.transform.GetChild(4).GetComponent<Image>().sprite = Resources.Load("Image/calligraphy/BattleEnding/胜", typeof(Sprite)) as Sprite;
                                str = "完胜";
                            }
                            else
                            {
                                if (remainScale >= 0.5f)
                                {
                                    SettlementPic.transform.GetChild(3).GetComponent<Image>().sprite = Resources.Load("Image/calligraphy/BattleEnding/大", typeof(Sprite)) as Sprite;
                                    SettlementPic.transform.GetChild(4).GetComponent<Image>().sprite = Resources.Load("Image/calligraphy/BattleEnding/胜", typeof(Sprite)) as Sprite;
                                    str = "大胜";
                                }
                                else
                                {
                                    if (remainScale >= 0.2f)
                                    {
                                        SettlementPic.transform.GetChild(3).GetComponent<Image>().sprite = Resources.Load("Image/calligraphy/BattleEnding/胜", typeof(Sprite)) as Sprite;
                                        SettlementPic.transform.GetChild(4).GetComponent<Image>().sprite = Resources.Load("Image/calligraphy/BattleEnding/利", typeof(Sprite)) as Sprite;
                                        str = "小胜";
                                    }
                                    else
                                    {
                                        SettlementPic.transform.GetChild(3).GetComponent<Image>().sprite = Resources.Load("Image/calligraphy/BattleEnding/险", typeof(Sprite)) as Sprite;
                                        SettlementPic.transform.GetChild(4).GetComponent<Image>().sprite = Resources.Load("Image/calligraphy/BattleEnding/胜", typeof(Sprite)) as Sprite;
                                        str = "险胜";
                                    }
                                }
                            }
                            getOrloseText.GetComponent<Text>().text = string.Format("{0}敌方，获得{1}金币！", str, addMoney);
                            string str2 = "玩家" + str + LoadJsonFile.forcesTableDatas[UIControl.enemy_forces[enemyForceId] - 1][1] + "的"+ LoadJsonFile.forcesTableDatas[UIControl.enemy_forces[enemyForceId] - 1][4] + "势力";
                            ControllMonthEvent.instance.AddShowMonthEvent(str2);

                            //展示战况
                            if (isSpecialLevel)
                            {
                                //SettlementPic.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>().text = string.Format("<color=#CDCDCD>{0}</color>        <color=#57A65F>{1}</color>        <color=#FFF0F5>{2}</color>", LoadJsonFile.forcesTableDatas[UIControl.playerForceId - 1][1], "胜", LoadJsonFile.NPCTableDates[specialLevelId][15]);
                            }
                            else
                            {
                                //SettlementPic.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>().text = string.Format("<color=#CDCDCD>{0}</color>        <color=#57A65F>{1}</color>        <color=#332D2D>{2}</color>", LoadJsonFile.forcesTableDatas[UIControl.playerForceId - 1][1], "胜", LoadJsonFile.forcesTableDatas[UIControl.enemy_forces[enemyForceId] - 1][1]);
                            }
                            //fightControll.GetComponent<FightControll>().BattleSettlement();

                            //延时显示结算界面
                            Invoke("ShowSettlementPic", 1f);
                            //RecoverCardData();  //战斗结算结束恢复卡牌数值
                            return null;
                        }
                    }
                    return enemyCards[selectEnemy];
                }
            }
        }
        else
        {
            if (fightNum < 3 && playerCards[fightNum] != null && playerCards[fightNum].GetComponent<CardMove>().Health > 0)
            {
                return playerCards[fightNum];
            }
            else
            {
                if (fightNum >= 3 && playerCards[fightNum % 3] != null && playerCards[fightNum % 3].GetComponent<CardMove>().Health > 0)
                {
                    selectEnemy = fightNum % 3;
                    return playerCards[fightNum % 3];
                }
                else
                {
                    selectEnemy = 0;

                    if (isMianZhan)
                    {
                        selectEnemy = playerCards.Length;
                        Debug.Log("玩家免战，电脑获胜");
                    }
                    else
                    {
                        while ((selectEnemy < playerCards.Length) && (playerCards[selectEnemy] == null || playerCards[selectEnemy].GetComponent<CardMove>().Health <= 0) )
                        {
                            selectEnemy++;
                        }
                    }
                    if (selectEnemy >= playerCards.Length)
                    {
                        Debug.Log("电脑获胜");
                        //结算战斗信息
                        //血量
                        int remainingHP = 0;    //总血量
                        int fullHP = 0;         //剩余血量
                        for (int j = 0; j < 9; j++)
                        {
                            if (enemyCards[j] != null)
                            {
                                fullHP += enemyCards[j].GetComponent<CardMove>().Fullhealth;
                                remainingHP += enemyCards[j].GetComponent<CardMove>().Health;
                            }
                        }
                        int cutHp = 0;
                        if (fightCtl.selectForce != -1)
                        {
                            float remainScale = (float)remainingHP / fullHP;    //玩家剩余血量比例
                            cutHp = (int)(remainScale * fightCtl.maxCutHp);    //玩家扣血
                            cutHp = cutHp > fightCtl.defCutHp ? cutHp : fightCtl.defCutHp;
                        }
                        else  //防守
                        {
                            cutHp = fightCtl.defCutHp;
                        }
                        CreateAndUpdate.playerHp -= cutHp;
                        getOrloseText.GetComponent<Text>().text = string.Format("惜败敌方，损失{0}势力值！", cutHp);
                        player_hp2.transform.GetChild(3).GetChild(0).GetComponent<cutHpTextMove>().content_text = "-" + cutHp;  //设置城池播放扣血文字内容

                        string str2 = "玩家惜败给了" + LoadJsonFile.forcesTableDatas[UIControl.enemy_forces[enemyForceId] - 1][1] + "的"+ LoadJsonFile.forcesTableDatas[UIControl.enemy_forces[enemyForceId] - 1][4] + "势力";
                        ControllMonthEvent.instance.AddShowMonthEvent(str2);

                        SettlementPic.transform.GetChild(3).GetComponent<Image>().sprite = Resources.Load("Image/calligraphy/BattleEnding/惜", typeof(Sprite)) as Sprite;
                        SettlementPic.transform.GetChild(4).GetComponent<Image>().sprite = Resources.Load("Image/calligraphy/BattleEnding/败", typeof(Sprite)) as Sprite;

                        if (isSpecialLevel)
                        {
                            //SettlementPic.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>().text = string.Format("<color=#CDCDCD>{0}</color>        <color=#E04638>{1}</color>        <color=#FFF0F5>{2}</color>", LoadJsonFile.forcesTableDatas[UIControl.playerForceId - 1][1], "败", LoadJsonFile.NPCTableDates[specialLevelId][15]);
                        }
                        else
                        {
                            //SettlementPic.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>().text = string.Format("<color=#CDCDCD>{0}</color>        <color=#E04638>{1}</color>        <color=#332D2D>{2}</color>", LoadJsonFile.forcesTableDatas[UIControl.playerForceId - 1][1], "败", LoadJsonFile.forcesTableDatas[UIControl.enemy_forces[enemyForceId] - 1][1]);
                        }
                        //fightControll.GetComponent<FightControll>().BattleSettlement();

                        Invoke("ShowSettlementPic", 1f);    //延时打开结算界面

                        return null;
                    }
                    return playerCards[selectEnemy];
                }
            }
        }
    }

    /// <summary>
    /// 改变玩家卡牌血量的百分比
    /// </summary>
    private void ChangePlayerCardHpPercentage()
    {
        float indexFlo = 1f;
        float addFlo = 1f + (uiControl.cityAttValuePy[1] + uiControl.bigEvent[3]) / 100f;   //军粮加成
        for (int n = 0; n < 9; n++)
        {
            if (playerCards[n] != null)
            {
                indexFlo = float.Parse(jiugongge_BrforeFight.GetChild(n).GetChild(0).GetComponent<HeroDataControll>().HeroData[23]) * addFlo;
                indexFlo += playerCards[n].GetComponent<CardMove>().Health / (float)playerCards[n].GetComponent<CardMove>().Fullhealth;
                playerCardHpPercentage[n] = Mathf.Clamp01(indexFlo);
                Debug.Log("下次战斗血量： " + playerCardHpPercentage[n]);
            }
        }
    }

    /// <summary>
    /// 选择对战类型
    /// </summary>
    public void ChooseFightStateToBigMap(int index) //1迎战-1防守0免战
    {
        isMianZhan = false;
        if (index == 0)
        {
            if (avoidWarNums > 0)
            {
                //免战
                isMianZhan = true;
                avoidWarNums--;
                ControllMonthEvent.instance.AddShowMonthEvent("免战");
            }
            else
            {
                Debug.Log("免战次数不足");
                createAndUpdate.GoldNotEnough("免战次数不足");
            }
        }
        else
        {
            if (index != -1)
            {
                //攻
            }
            else
            {
                //守
            }
            fightCtl.selectForce = index;
        }
        chooseFightStateObj.gameObject.SetActive(false);
        bigMapObj.gameObject.SetActive(false);
        vsTitleObj.gameObject.SetActive(false);
        isFightNow = false;
        EndOfInitToFight();
        roundTextObj.text = "回合 " + roundNum;
        roundTextObj.gameObject.SetActive(true);
    }

    int indexNpcFightOver = 0;
    /// <summary>
    /// 显示每轮战斗的结算界面
    /// </summary>
    private void ShowSettlementPic()
    {
        //还有势力要和玩家势力战斗
        if(indexNpcFightOver < npcForceFightPy.Count)
        {
            isEndOFInit = false;

            ChangePlayerCardHpPercentage();

            RecoverCardData();
            
            array_str = fightCtl.enemyHeroDatas[indexNpcFightOver];
            enemyForceId = indexNpcFightOver;
            //敌人初始化
            InitializeBattleCard();
            //上阵初始化
            OtherInitialization();

            InitForceFlag();

            indexNpcFightOver++;
            fightNum = 0;

            chooseFightStateObj.gameObject.SetActive(true);
            bigMapObj.gameObject.SetActive(true);
            //isFightNow = false;
        }
        else
        {
            OpenOrCloseObj(true);
            Debug.Log("此年战斗结束！");
            indexNpcFightOver = 0;
            //设置首显战况
            Transform tran = SettlementPic.transform.parent;
            SettlementPic.transform.SetParent(fightControll);
            SettlementPic.transform.SetParent(tran);

            SettlementPic.gameObject.SetActive(true);
            ShowGameOver();
            getOrloseText.SetActive(true);
        }
    }

    /// <summary>
    /// 重置卡牌数值，玩家加经验加金币
    /// </summary>
    public void RecoverCardData()
    {
        createAndUpdate.AddMoneyAndExp();

        for (int n = 0; n < 9; n++)
        {
            if (playerCards[n] != null)
            {
                Destroy(playerCards[n].gameObject);
            }
            if (enemyCards[n] != null)
            {
                Destroy(enemyCards[n].gameObject);
            }
        }
        //玩家血条刷新
        player_hp.value = CreateAndUpdate.playerHp / float.Parse(LoadJsonFile.difficultyChooseDatas[CreateAndUpdate.difficultNum - 1][2]);
        player_hp.transform.GetChild(3).GetComponent<Text>().text = CreateAndUpdate.playerHp.ToString();
        player_hp2.value = CreateAndUpdate.playerHp / float.Parse(LoadJsonFile.difficultyChooseDatas[CreateAndUpdate.difficultNum - 1][2]);
        player_hp2.transform.GetChild(3).GetComponent<Text>().text = CreateAndUpdate.playerHp.ToString();
        if (fightCtl.hpFloatToFire >= player_hp.value)
        {
            playerForceCity.GetChild(3).gameObject.SetActive(true); //开启冒火特效
        }
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    private void ShowGameOver()
    {
        if (CreateAndUpdate.playerHp <= 0)    //失败结算
        {
            SettlementPic.transform.GetChild(5).gameObject.SetActive(false);
            Invoke("ClearingTheGame", 2f);    //延时打开本次游戏结束界面
            //ClearingTheGame();
            return;
        }
        for (int i = 0; i < fightCtl.npcPlayerHps.Length; i++)
        {
            if (fightCtl.npcPlayerHps[i] > 0) //有npc还活着
                return;
        }
        //结算胜利
        SettlementPic.transform.GetChild(5).gameObject.SetActive(false);
        Invoke("ClearingTheGame", 2f);    //延时打开本次游戏结束界面
        //ClearingTheGame();
    }

    /// <summary>
    /// 游戏结算
    /// </summary>
    private void ClearingTheGame()
    {
        int forceCount = UIControl.enemy_forces.Count + 1;  //势力总数
        SettlementPic.transform.parent.gameObject.SetActive(false);
        int[] arrRank = new int[forceCount];  //结束势力排名id
        int[] arrRankWinTimes = new int[forceCount];  //结束势力胜场排名
        for (int i = 0; i < forceCount; i++)
        {
            arrRank[i] = -1;
            arrRankWinTimes[i] = -1;
        }
        for (int i = 0; i < forceCount - 1; i++)
        {
            arrRank[i] = UIControl.enemy_forces[i];
            arrRankWinTimes[i] = fightCtl.allWinTimes[i];
        }
        int tempWintimes, idIndex = -1;
        for (int i = 0; i < forceCount - 1; i++)     //对npc进行排名
        {
            for (int j = forceCount - 2; j > i; j--)
            {
                if (arrRankWinTimes[j] > arrRankWinTimes[j - 1])
                {
                    tempWintimes = arrRankWinTimes[j - 1];
                    idIndex = arrRank[j - 1];
                    arrRankWinTimes[j - 1] = arrRankWinTimes[j];
                    arrRank[j - 1] = arrRank[j];
                    arrRankWinTimes[j] = tempWintimes;
                    arrRank[j] = idIndex;
                }
            }
        }
        for (int i = 0; i < forceCount; i++)     //对玩家进行排名
        {
            if (i == forceCount - 1)   //最后一名
            {
                idIndex = i;    //记录玩家的名次
                arrRank[i] = UIControl.playerForceId;
                arrRankWinTimes[i] = winBattles;
            }
            else
            {
                if (winBattles > arrRankWinTimes[i])
                {
                    idIndex = i;
                    for (int j = forceCount - 2; j >= i; j--)
                    {
                        arrRank[j + 1] = arrRank[j];
                        arrRankWinTimes[j + 1] = arrRankWinTimes[j];
                    }
                    arrRank[i] = UIControl.playerForceId;
                    arrRankWinTimes[i] = winBattles;
                    break;
                }
            }
        }
        //结算展示
        int addPrestige = 0;    //增加声望值数量
        if (idIndex < 3)  //前三名
        {
            addPrestige = int.Parse(LoadJsonFile.difficultyChooseDatas[fightCtl.difnum - 1][15 + idIndex]);
            if (idIndex == 0) //第一名
            {
                gameOverBg.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = Resources.Load("Image/calligraphy/GameOver/" + "总结算_威震", typeof(Sprite)) as Sprite;
                gameOverBg.transform.GetChild(0).GetChild(2).GetComponent<Image>().sprite = Resources.Load("Image/calligraphy/GameOver/" + "总结算_华夏", typeof(Sprite)) as Sprite;
            }
            else
            {
                if (idIndex == 1) //第二名
                {
                    gameOverBg.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = Resources.Load("Image/calligraphy/GameOver/" + "总结算_统领", typeof(Sprite)) as Sprite;
                    gameOverBg.transform.GetChild(0).GetChild(2).GetComponent<Image>().sprite = Resources.Load("Image/calligraphy/GameOver/" + "总结算_九州", typeof(Sprite)) as Sprite;
                }
                else    //第三名
                {
                    gameOverBg.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = Resources.Load("Image/calligraphy/GameOver/" + "总结算_称霸", typeof(Sprite)) as Sprite;
                    gameOverBg.transform.GetChild(0).GetChild(2).GetComponent<Image>().sprite = Resources.Load("Image/calligraphy/GameOver/" + "总结算_一方", typeof(Sprite)) as Sprite;
                }
            }
        }
        else
        {
            addPrestige = int.Parse(LoadJsonFile.difficultyChooseDatas[fightCtl.difnum - 1][18]);
            gameOverBg.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = Resources.Load("Image/calligraphy/GameOver/" + "总结算_遗恨", typeof(Sprite)) as Sprite;
            gameOverBg.transform.GetChild(0).GetChild(2).GetComponent<Image>().sprite = Resources.Load("Image/calligraphy/GameOver/" + "总结算_千古", typeof(Sprite)) as Sprite;
        }
        gameOverBg.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "声望 +" + addPrestige;
        PlayerPrefs.SetInt("prestigeNum", PlayerPrefs.GetInt("prestigeNum") + addPrestige);
        for (int i = 0; i < forceCount; i++)
        {
            //势力
            gameOverBg.transform.GetChild(0).GetChild(3).GetChild(1).GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetComponent<Text>().text = LoadJsonFile.forcesTableDatas[arrRank[i] - 1][1] + "";
            //胜场
            gameOverBg.transform.GetChild(0).GetChild(3).GetChild(1).GetChild(0).GetChild(0).GetChild(i).GetChild(1).GetComponent<Text>().text = arrRankWinTimes[i] + "";
            //平局
            //gameOverBg.transform.GetChild(0).GetChild(3).GetChild(1).GetChild(0).GetChild(0).GetChild(i).GetChild(2).GetComponent<Text>().text = 0 + "";
            //败场
            gameOverBg.transform.GetChild(0).GetChild(3).GetChild(1).GetChild(0).GetChild(0).GetChild(i).GetChild(3).GetComponent<Text>().text = (battles - 1 - arrRankWinTimes[i]) + "";
        }
        for (int i = 0; i < 4; i++) //玩家势力红字
        {
            gameOverBg.transform.GetChild(0).GetChild(3).GetChild(1).GetChild(0).GetChild(0).GetChild(idIndex).GetChild(i).GetComponent<Text>().color = Color.white;
        }
        gameOverBg.transform.GetChild(0).GetChild(3).GetChild(1).GetComponent<ScrollRect>().verticalNormalizedPosition = 1; //回到顶部
        gameOverBg.SetActive(true);
    }

    /// <summary>
    /// 重置攻击卡牌的parent
    /// </summary>
    /// <param name="tran">攻击卡牌的Transform</param>
    private void ChangeParent(Transform tran)
    {
        Transform par = tran.parent;
        tran.SetParent(fightControll);
        tran.SetParent(par);
    }

    /// <summary>
    /// 用于延时开始战斗
    /// </summary>
    private void LiteTimeStart()
    {
        roundTextObj.gameObject.SetActive(false);
        isStart = true;
    }

    int[] enemyArmsActive = new int[3] { 0, 0, 0 };
    int enemyNums = 0;
    /// <summary>
    /// 初始化战斗卡牌
    /// </summary>
    public void InitializeBattleCard()
    {
        Debug.Log("战斗势力： " + LoadJsonFile.forcesTableDatas[UIControl.enemy_forces[enemyForceId] - 1][4]);

        enemyNums = 0;

        ChooseIsLoadSpecialNPC();

        //特殊关卡敌人初始化
        if (isSpecialLevel)
        {
            Debug.Log("加载特殊敌人关卡: " + LoadJsonFile.NPCTableDates[specialLevelId][2]);

            string[] strArr;
            int heroId_NPC = 0;
            string hero_Grade = string.Empty;
            for (int i = 0; i < arrayNPC_str.Length; i++)
            {
                arrayNPC_str[i] = null;
                if (LoadJsonFile.NPCTableDates[specialLevelId][6 + i] != "")
                {
                    strArr = LoadJsonFile.NPCTableDates[specialLevelId][6 + i].Split(',');
                    heroId_NPC = int.Parse(strArr[0]);
                    hero_Grade = strArr[1];
                    List<string> newData = LoadJsonFile.DeepClone<string>(LoadJsonFile.RoleTableDatas[heroId_NPC - 1]);
                    arrayNPC_str[i] = newData;
                    arrayNPC_str[i].Add(hero_Grade);
                }
            }

            //设置敌方上阵激活兵种
            for (int i = 0; i < 9; i++)
            {
                if (arrayNPC_str[i] != null)
                {
                    enemyNums++;
                }
            }
            //默认最高开启一兵种技能
            for (int i = 1; i < 4; i++)
            {
                if (enemyNums >= i * 3)
                {
                    if (arrayNPC_str[i * 3 - 3][0] == arrayNPC_str[i * 3 - 2][0] || 
                        arrayNPC_str[i * 3 - 3][0] == arrayNPC_str[i * 3 - 1][0] || 
                        arrayNPC_str[i * 3 - 2][0] == arrayNPC_str[i * 3 - 1][0])
                    {
                        enemyArmsActive[i - 1] = 0;
                    }
                    else
                    {
                        enemyArmsActive[i - 1] = 1;
                    }
                }
            }
            //敌方卡牌武将初始化
            for (int i = 0; i < 9; i++) //玩家和敌方战斗卡牌，空卡牌就位
            {
                playerCards[i] = null;
                if (jiugongge_BrforeFight.GetChild(i).childCount > 0)
                {
                    playerCards[i] = Instantiate(heroFightCard, OwnJiuGonggePos[i]);
                    playerCards[i].transform.position = OwnJiuGonggePos[i].position;
                    playerCards[i].transform.SetParent(OwnJiuGonggePos[i].parent.parent);
                }
                if (arrayNPC_str[i] != null) //敌方卡牌此位置有武将数据
                {
                    List<string> datas = arrayNPC_str[i];  //记录单个武将卡牌的数据，倒数第一个是品阶
                    enemyCards[i] = Instantiate(heroFightCard, enemyJiuGonggePos[i]);
                    enemyCards[i].transform.position = enemyJiuGonggePos[i].position;
                    enemyCards[i].transform.SetParent(enemyJiuGonggePos[i].parent.parent);
                    CardMove enemyCards_CM = enemyCards[i].GetComponent<CardMove>();
                    enemyCards_CM.IsPlayerOrEnemy = 1;
                    enemyCards_CM.IsAttack_first = ((i + 2) % 2 == 0) ? false : true;
                    enemyCards_CM.HeroId = int.Parse(datas[0]);
                    enemyCards_CM.Grade = int.Parse(datas[datas.Count - 1]);
                    enemyCards_CM.ArmsId = datas[3];
                    enemyCards_CM.ArmsSkillStatus = enemyArmsActive[i / 3];
                    enemyCards_CM.Health = enemyCards_CM.Fullhealth = int.Parse(datas[8]);
                    enemyCards_CM.HpCityAdd = 0;
                    enemyCards_CM.Force = int.Parse(datas[6]);
                    enemyCards_CM.ForceCityAdd = 0;
                    enemyCards_CM.Defence = int.Parse(datas[7]);
                    enemyCards_CM.DefenceCityAdd = 0;
                    enemyCards_CM.DodgeRate = float.Parse(datas[9]);
                    enemyCards_CM.ThumpRate = float.Parse(datas[12]);
                    enemyCards_CM.ThumpDamage = float.Parse(datas[13]);
                    enemyCards_CM.CritRate = float.Parse(datas[10]);
                    enemyCards_CM.CritRateCityAdd = 0;
                    enemyCards_CM.CritDamage = float.Parse(datas[11]);
                    enemyCards_CM.ArmorPenetrationRate = float.Parse(datas[14]);
                    enemyCards[i].transform.GetChild(3).GetComponent<Text>().text = ""; //datas[1];
                    CardRarityShow(int.Parse(datas[4]), enemyCards[i].transform.GetChild(1).GetChild(1));   //稀有度颜色表现

                    //enemyCards_CM.OtherDataSet();
                    InitFightState(enemyCards[i]);
                }
            }
        }
        else
        {
            //设置敌方上阵激活兵种
            for (int i = 0; i < 9; i++)
            {
                if (array_str[i] != null)
                {
                    enemyNums++;
                }
            }
            //默认最高开启一兵种技能
            for (int i = 1; i < 4; i++)
            {
                if (enemyNums >= i * 3)
                {
                    if (array_str[i * 3 - 3][0] == array_str[i * 3 - 2][0] || 
                        array_str[i * 3 - 3][0] == array_str[i * 3 - 1][0] || 
                        array_str[i * 3 - 2][0] == array_str[i * 3 - 1][0])
                    {
                        enemyArmsActive[i - 1] = 0;
                    }
                    else
                    {
                        enemyArmsActive[i - 1] = 1;
                    }
                }
            }
            //敌方卡牌武将初始化
            for (int i = 0; i < 9; i++) //玩家和敌方战斗卡牌，空卡牌就位
            {
                playerCards[i] = null;
                if (jiugongge_BrforeFight.GetChild(i).childCount > 0)
                {
                    playerCards[i] = Instantiate(heroFightCard, OwnJiuGonggePos[i]);
                    playerCards[i].transform.position = OwnJiuGonggePos[i].position;
                    playerCards[i].transform.SetParent(OwnJiuGonggePos[i].parent.parent);
                }
                if (array_str[i] != null) //敌方卡牌此位置有武将数据
                {
                    List<string> datas = array_str[i];  //记录单个武将卡牌的数据，倒数第二个是品阶，倒数第一个是参与战斗周目数
                    enemyCards[i] = Instantiate(heroFightCard, enemyJiuGonggePos[i]);
                    enemyCards[i].transform.position = enemyJiuGonggePos[i].position;
                    enemyCards[i].transform.SetParent(enemyJiuGonggePos[i].parent.parent);
                    CardMove enemyCards_CM = enemyCards[i].GetComponent<CardMove>();
                    //设置卡牌为敌方卡牌
                    enemyCards_CM.IsPlayerOrEnemy = 1;
                    //设置敌方卡牌的默认先后手情况
                    enemyCards_CM.IsAttack_first = ((i + 2) % 2 == 0) ? false : true;
                    //武将ID
                    enemyCards_CM.HeroId = int.Parse(datas[0]);
                    //品阶
                    enemyCards_CM.Grade = int.Parse(datas[datas.Count - 2]);
                    //兵种
                    enemyCards_CM.ArmsId = datas[3];
                    //兵种技能激活状态
                    enemyCards_CM.ArmsSkillStatus = enemyArmsActive[i / 3];
                    //enemyCards[i].GetComponent<CardMove>().ArmsSkillStatus = 0;
                    //血量
                    enemyCards_CM.Health = enemyCards_CM.Fullhealth = int.Parse(datas[8]);
                    //城市血量加成
                    enemyCards_CM.HpCityAdd = 0;
                    //攻击力 （受战意数值影响）
                    enemyCards_CM.Force = int.Parse(datas[6]);
                    //城市攻击力加成
                    enemyCards_CM.ForceCityAdd =(uiControl.cityAttValueNpc[enemyForceId][0] + uiControl.bigEventNpc[enemyForceId][2] - 50) / 50f;
                    //防御力
                    enemyCards_CM.Defence = int.Parse(datas[7]);
                    //城市防御力加成
                    enemyCards_CM.DefenceCityAdd = 0;
                    //闪避率
                    enemyCards_CM.DodgeRate = float.Parse(datas[9]);
                    //重击率
                    enemyCards_CM.ThumpRate = float.Parse(datas[12]);
                    //重击伤害百分比
                    enemyCards_CM.ThumpDamage = float.Parse(datas[13]);
                    //暴击率
                    enemyCards_CM.CritRate = float.Parse(datas[10]);
                    //城市暴力率加成
                    enemyCards_CM.CritRateCityAdd = (uiControl.cityAttValueNpc[enemyForceId][3] + uiControl.bigEventNpc[enemyForceId][5] - 50) / 50f * 0.2f;
                    //暴击伤害百分比
                    enemyCards_CM.CritDamage = float.Parse(datas[11]);
                    //破甲百分比
                    enemyCards_CM.ArmorPenetrationRate = float.Parse(datas[14]);
                    //显示武将名
                    enemyCards[i].transform.GetChild(3).GetComponent<Text>().text = "";//datas[1];
                    CardRarityShow(int.Parse(datas[4]), enemyCards[i].transform.GetChild(1).GetChild(1));   //稀有度颜色表现

                    //enemyCards_CM.OtherDataSet();
                    InitFightState(enemyCards[i]);
                }
            }
        }
    }

    private void OtherInitialization()
    {
        FightControll.playerHeroHps = 0;    //玩家英雄总血量

        //玩家卡牌初始化
        for (int i = 0; i < playerCards.Length; i++)
        {
            if (playerCards[i] != null)
            {
                //临时存储玩家数据，传递给战斗卡牌
                List<string> datas = jiugongge_BrforeFight.GetChild(i).GetChild(0).GetComponent<HeroDataControll>().HeroData;
                FightControll.playerHeroHps += int.Parse(datas[8]);
                CardMove playerCards_CM = playerCards[i].GetComponent<CardMove>();
                //设置卡牌为玩家卡牌
                playerCards_CM.IsPlayerOrEnemy = 0;
                //设置玩家卡牌的默认先后手情况
                playerCards_CM.IsAttack_first = ((i + 2) % 2 == 0) ? true : false;
                //武将ID
                playerCards_CM.HeroId = int.Parse(datas[0]);
                //品阶
                playerCards_CM.Grade = jiugongge_BrforeFight.GetChild(i).GetChild(0).GetComponent<HeroDataControll>().Grade_hero;
                //兵种
                playerCards_CM.ArmsId = datas[3];
                //兵种技能激活状态
                playerCards_CM.ArmsSkillStatus = armsSkillStatus[int.Parse(datas[3]) - 1];
                //血量
                playerCards_CM.Fullhealth = int.Parse(datas[8]);
                playerCards_CM.Health = (int)(playerCards_CM.Fullhealth * playerCardHpPercentage[i]);
                playerCards[i].GetComponent<Slider>().value = 1 - playerCards_CM.Health / (float)playerCards_CM.Fullhealth;
                //城市血量加成
                playerCards_CM.HpCityAdd = 0;
                //攻击力
                playerCards_CM.Force = int.Parse(datas[6]);
                //城市攻击力加成
                playerCards_CM.ForceCityAdd =(uiControl.cityAttValuePy[0] + uiControl.bigEvent[2] - 50) / 50f;
                //防御力
                playerCards_CM.Defence = int.Parse(datas[7]);
                //城市防御力加成
                playerCards_CM.DefenceCityAdd = fightCtl.selectForce != -1 ? 0 : (uiControl.cityAttValuePy[2] + uiControl.bigEvent[4] - 50) / 50f;
                //闪避率
                playerCards_CM.DodgeRate = float.Parse(datas[9]);
                //重击率
                playerCards_CM.ThumpRate = float.Parse(datas[12]);
                //重击伤害百分比
                playerCards_CM.ThumpDamage = float.Parse(datas[13]);
                //暴击率
                playerCards_CM.CritRate = float.Parse(datas[10]);
                //城市暴力率加成
                playerCards_CM.CritRateCityAdd = (uiControl.cityAttValuePy[3] + uiControl.bigEvent[5] - 50) / 50f * 0.2f;
                //暴击伤害百分比
                playerCards_CM.CritDamage = float.Parse(datas[11]);
                //破甲百分比
                playerCards_CM.ArmorPenetrationRate = float.Parse(datas[14]);
                //显示武将名
                playerCards[i].transform.GetChild(3).GetComponent<Text>().text = "";//datas[1];
                CardRarityShow(int.Parse(datas[4]), playerCards[i].transform.GetChild(1).GetChild(1));   //稀有度颜色表现

                //playerCards_CM.OtherDataSet();     //其他初始化
                InitFightState(playerCards[i]);     //战斗状态控制初始化
            }
        }
    }

    /// <summary>
    /// 战斗特殊状态初始化
    /// </summary>
    /// <param name="obj"></param>
    private void InitFightState(GameObject obj)
    {
        CardMove cardMove = obj.GetComponent<CardMove>();
        cardMove.Fight_State = new FightState();
        cardMove.Fight_State.isDizzy = false;
        cardMove.Fight_State.isBatter = false;
        cardMove.Fight_State.batterNums = 0;
        cardMove.Fight_State.isWithStand = false;
        cardMove.Fight_State.withStandNums = 0;
        cardMove.Fight_State.isFireAttack = false;
        cardMove.Fight_State.isFightMean = false;
        cardMove.Fight_State.isPopular = false;
    }

    /// <summary>
    /// 初始化兵种技能激活状态
    /// </summary>
    private void InitArmsSkillStatus()
    {
        for (int i = 0; i < 9; i++)
        {
            armsSkillStatus[i] = 0;
        }
        int count = HeroIdChangeAndSave.activationSkillId_soldiers.Count;
        int armsid, nums = 0;
        for (int i = 0; i < count; i++)
        {
            armsid = HeroIdChangeAndSave.activationSkillId_soldiers[i] / 10 - 1;    //兵种编号
            if (nums < HeroIdChangeAndSave.activationSkillId_soldiers[i] % 10)
                nums = HeroIdChangeAndSave.activationSkillId_soldiers[i] % 10;
            if (nums < 6)
                armsSkillStatus[armsid] = 1;
            else
                armsSkillStatus[armsid] = 2;
        }
    }

    /// <summary>
    /// 判断并记录是否进入特殊关卡
    /// </summary>
    private void ChooseIsLoadSpecialNPC()
    {
        string nums = (battles - 1).ToString();
        specialLevelId = 0;    //记录npc特殊阵容ID
        while (LoadJsonFile.NPCTableDates[specialLevelId][1] == "2")
        {
            specialLevelId++;
        }
        for (; specialLevelId < LoadJsonFile.NPCTableDates.Count; specialLevelId++)
        {
            if (nums == LoadJsonFile.NPCTableDates[specialLevelId][5])
            {
                isSpecialLevel = true;
                return;
            }
        }
        isSpecialLevel = false;
    }

    /// <summary>
    /// 对阵战旗战役文字初始化
    /// </summary>
    private void InitForceFlag()
    {
        int npcForceId = -1;    //记录对手势力id
        playerForceFlag.sprite = Resources.Load("Image/calligraphy/Forces/" + LoadJsonFile.forcesTableDatas[UIControl.playerForceId - 1][4], typeof(Sprite)) as Sprite;           //设置玩家势力的头像
        SettlementPic.transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = Resources.Load("Image/calligraphy/Forces/" + LoadJsonFile.forcesTableDatas[UIControl.playerForceId - 1][4], typeof(Sprite)) as Sprite; //结算面板的头像
        if (isSpecialLevel)
        {
            specialFightText.text = LoadJsonFile.NPCTableDates[specialLevelId][2];
            npcForceId = int.Parse(LoadJsonFile.NPCTableDates[specialLevelId][16]);
        } 
        else
        {
            specialFightText.text = "";
            npcForceId = UIControl.enemy_forces[enemyForceId];
        }
        rivalForceFlag.sprite = Resources.Load("Image/calligraphy/Forces/" + LoadJsonFile.forcesTableDatas[npcForceId - 1][4], typeof(Sprite)) as Sprite;    //设置特殊对手势力的头像
        SettlementPic.transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite = Resources.Load("Image/calligraphy/Forces/" + LoadJsonFile.forcesTableDatas[npcForceId - 1][4], typeof(Sprite)) as Sprite;
        SettlementPic.transform.GetChild(1).GetChild(3).GetComponent<Text>().text = ((fightCtl.selectForce != -1) ? "进攻" : "抵御") + LoadJsonFile.forcesTableDatas[npcForceId - 1][1];

        vsTitleObj.GetChild(0).GetComponent<Image>().sprite = playerForceFlag.sprite;
        vsTitleObj.GetChild(1).GetComponent<Image>().sprite = rivalForceFlag.sprite;
        vsTitleObj.gameObject.SetActive(true);

        string playerFightNpcStr = "玩家"+ ((fightCtl.selectForce != -1) ? "进攻" : "抵御") + LoadJsonFile.forcesTableDatas[npcForceId - 1][1]+"的"+ LoadJsonFile.forcesTableDatas[npcForceId - 1][4] + "势力";
        ControllMonthEvent.instance.AddShowMonthEvent(playerFightNpcStr);
    }

    private void CardRarityShow(int rarity, Transform imageObj)
    {
        switch (rarity)
        {
            case 1:
                //enemyCards[i].transform.GetChild(3).GetComponent<Text>().color = ColorData.green_Color;  //绿色
                imageObj.GetComponent<Image>().color = ColorData.green_Color;  //绿色
                break;
            case 2:
                //enemyCards[i].transform.GetChild(3).GetComponent<Text>().color = ColorData.blue_Color_hero; //蓝色
                imageObj.GetComponent<Image>().color = ColorData.blue_Color_hero; //蓝色
                break;
            case 3:
                //enemyCards[i].transform.GetChild(3).GetComponent<Text>().color = ColorData.purple_Color; //紫色
                imageObj.GetComponent<Image>().color = ColorData.purple_Color; //紫色
                break;
            case 4:
                //enemyCards[i].transform.GetChild(3).GetComponent<Text>().color = ColorData.red_Color_hero;  //红色
                imageObj.GetComponent<Image>().color = ColorData.red_Color_hero;  //红色
                break;
        }
    }

    //结束初始化
    public void EndOfInitToFight()
    {
        Invoke("LiteTimeToChooseInit", 2f);
    }

    private void LiteTimeToChooseInit()
    {
        isEndOFInit = true;
    }

    private void OpenOrCloseObj(bool boo)
    {
        for (int i = 0; i < needHideObj.Length; i++)
        {
            needHideObj[i].SetActive(boo);
        }
    }
}