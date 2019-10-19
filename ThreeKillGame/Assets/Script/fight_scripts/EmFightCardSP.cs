using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmFightCardSP : MonoBehaviour
{
    [SerializeField]
    private Slider player_hp;//玩家血条
    [SerializeField]
    private GameObject updateBtn;   //招募刷新控件，用来获取脚本
    [SerializeField]
    private Text victoryOrFailureText;  //战斗胜负显示

    private bool isEndOFInit = false;   //记录是否初始化结束
    private int battles;       //记录对战次数
    private int roundNum;   //记录当前回合数量
    private int fightNum;   //记录当前攻击武将的位置
    private bool isPlayerBout;  //记录是否是玩家的武将攻击回合
    [HideInInspector]
    public static bool isFightNow;  //记录现在是否正在攻击

    public Transform jiugongge_BrforeFight;  //上阵布阵的九宫格
    public GameObject heroFightCard;    //英雄战斗卡片预制件
    public Transform[] OwnJiuGonggePos = new Transform[9];  //战斗槽位置_玩家

    [SerializeField]
    GameObject[] enemyCards = new GameObject[9];//存储敌人卡牌
    [SerializeField]
    GameObject[] playerCards = new GameObject[9];//存储己方卡牌

    UseEPPlusFun useepplusfun = new UseEPPlusFun();
    TableDatas worksheet_NPC;   //存储npc表
    TableDatas worksheet_Role;  //存储武将表

    private void Awake()
    {
        worksheet_NPC = useepplusfun.FindExcelFiles("NPCTable");  //获取NPC数据表
        worksheet_Role = useepplusfun.FindExcelFiles("RoleTable1");   //武将表数据
        CreateEnemyUnits(); //初始化敌方发展阵容
    }

    private void Start()
    {
        battles = 1;
        //isFightNow = false;
    }

    private void OnEnable()
    {
        fightNum = 0;
        roundNum = 1;
        isPlayerBout = true;
        battles += 1;   //战斗次数增加
        //Debug.Log("OnEnable()执行");
        InitializeBattleCard();
        isFightNow = false;
        isEndOFInit = true;
    }

    int[] enemyUnits = new int[3];  //记录敌方势力要发展的兵种类型
    //初始化创建敌方后期要发展的兵种类型
    private void CreateEnemyUnits()
    {
        enemyUnits[0] = UnityEngine.Random.Range(1, 7);      //前排
        enemyUnits[1] = UnityEngine.Random.Range(1, 10);    //中排
        enemyUnits[2] = UnityEngine.Random.Range(4, 10);    //后排
    }

    List<string>[] enemyHreoData = new List<string>[9];  //记录敌方上阵英雄的数据
    /// <summary>
    /// 初始化战斗卡牌
    /// </summary>
    public void InitializeBattleCard()
    {
        for (int i = 0; i < 9; i++) //玩家战斗卡牌，空卡牌就位
        {
            playerCards[i] = null;
            if (jiugongge_BrforeFight.GetChild(i).childCount > 0)
            {
                playerCards[i] = Instantiate(heroFightCard, OwnJiuGonggePos[i]);
                playerCards[i].transform.position = OwnJiuGonggePos[i].position;
                playerCards[i].transform.SetParent(OwnJiuGonggePos[i].parent.parent);
            }
        }
        //敌方上阵英雄数据获取和传递
        //for (int i = 0; i < 9; i++)
        //{
        //    if (enemyCards[i] != null)
        //    {
        //        //传递ID,品阶,战斗周目数
        //        enemyHreoData[i].Add(enemyCards[i].GetComponent<CardMove>().HeroId.ToString());
        //        enemyHreoData[i].Add(enemyCards[i].GetComponent<CardMove>().Grade.ToString());
        //        enemyHreoData[i].Add((enemyCards[i].GetComponent<CardMove>().FightNums += 1).ToString());
        //    }
        //    else
        //    {
        //        enemyHreoData[i] = null;
        //    }
        //}
        //enemyHreoData = EmFightControll.SendHeroData(enemyHreoData, enemyUnits, battles-1);    //npc武将数据更新
        //上阵初始化
        OtherInitialization();
    }



    private void OtherInitialization()
    {

        //敌方卡牌初始化
        for (int i = 0; i < enemyCards.Length; i++)
        {
            if (enemyCards[i] != null)
            {
                //获得武将id和品阶
                string[] heroIdandclass = worksheet_NPC.worksheet.Cells[battles + 1, 6 + i].Value.ToString().Split(',');
                int grade_Addition = (int)Mathf.Pow(2, (heroIdandclass[1] != null ? int.Parse(heroIdandclass[1]) : 1) - 1);  //获取武将品阶加成
                //根据品阶，设置敌方武将卡牌外观
                switch (heroIdandclass[1])
                {
                    case "1":
                        enemyCards[i].transform.GetComponent<Image>().color = Color.white;
                        break;
                    case "2":
                        enemyCards[i].transform.GetComponent<Image>().color = Color.blue;
                        break;
                    case "3":
                        enemyCards[i].transform.GetComponent<Image>().color = Color.red;
                        break;
                    default:
                        enemyCards[i].transform.GetComponent<Image>().color = Color.white;
                        break;
                }

                //在武将表中获取武将数据
                enemyCards[i].GetComponent<CardMove>().IsAttack_first = ((i + 2) % 2 == 0) ? false : true;
                Debug.Log("///NPC第" + (i + 1) + "位武将ID：" + heroIdandclass[0]);
                enemyCards[i].transform.GetChild(3).GetComponent<Text>().text = worksheet_Role.worksheet.Cells[int.Parse(heroIdandclass[0]) + 1, 2].Value.ToString();
                enemyCards[i].GetComponent<CardMove>().Force = grade_Addition * int.Parse(worksheet_Role.worksheet.Cells[int.Parse(heroIdandclass[0]) + 1, 7].Value.ToString());
                enemyCards[i].GetComponent<CardMove>().Defence = int.Parse(worksheet_Role.worksheet.Cells[int.Parse(heroIdandclass[0]) + 1, 8].Value.ToString());
                enemyCards[i].GetComponent<CardMove>().Health = enemyCards[i].GetComponent<CardMove>().Fullhealth = grade_Addition * int.Parse(worksheet_Role.worksheet.Cells[int.Parse(heroIdandclass[0]) + 1, 9].Value.ToString());
                enemyCards[i].GetComponent<CardMove>().OtherDataSet();
            }
        }

        //玩家卡牌初始化
        for (int i = 0; i < playerCards.Length; i++)
        {
            if (playerCards[i] != null)
            {
                //临时存储玩家数据，传递给战斗卡牌
                List<string> datas = jiugongge_BrforeFight.GetChild(i).GetChild(0).GetComponent<HeroDataControll>().HeroData;
                //设置地方卡牌的默认先后手情况
                playerCards[i].GetComponent<CardMove>().IsAttack_first = ((i + 2) % 2 == 0) ? true : false;
                //武将ID
                playerCards[i].GetComponent<CardMove>().HeroId = int.Parse(datas[0]);
                //品阶
                playerCards[i].GetComponent<CardMove>().Grade = jiugongge_BrforeFight.GetChild(i).GetChild(0).GetComponent<HeroDataControll>().Grade_hero;
                //血量
                playerCards[i].GetComponent<CardMove>().Health = playerCards[i].GetComponent<CardMove>().Fullhealth = int.Parse(datas[8]);
                //攻击力
                playerCards[i].GetComponent<CardMove>().Force = int.Parse(datas[6]);
                //防御力
                playerCards[i].GetComponent<CardMove>().Defence = int.Parse(datas[7]);
                //闪避率
                playerCards[i].GetComponent<CardMove>().DodgeRate = float.Parse(datas[9]);
                //重击率
                playerCards[i].GetComponent<CardMove>().ThumpRate = float.Parse(datas[12]);
                //重击伤害百分比
                playerCards[i].GetComponent<CardMove>().ThumpDamage = float.Parse(datas[13]);
                //暴击率
                playerCards[i].GetComponent<CardMove>().CritRate = float.Parse(datas[10]);
                //暴击伤害百分比
                playerCards[i].GetComponent<CardMove>().CritDamage = float.Parse(datas[11]);
                //破甲百分比
                playerCards[i].GetComponent<CardMove>().ArmorPenetrationRate = float.Parse(datas[14]);
                //显示武将名
                playerCards[i].transform.GetChild(3).GetComponent<Text>().text = datas[1];
                playerCards[i].GetComponent<CardMove>().OtherDataSet();
                datas.Clear();
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        //若还在初始化
        if (!isEndOFInit)
            return;

        //回合增加
        if (fightNum >= playerCards.Length)
        {
            Debug.Log("///回合" + roundNum + "结束///");
            roundNum++;
            fightNum = 0;
        }
        //若有武将正在攻击
        if (isFightNow)
            return;
        //  玩家卡牌槽此位置有卡牌            此卡牌位为先手攻击                                                 此卡牌为后手                                                     敌方此卡牌位无卡牌                玩家此卡牌位卡牌武将血量不为零
        if (playerCards[fightNum] != null && (playerCards[fightNum].GetComponent<CardMove>().IsAttack_first || (!playerCards[fightNum].GetComponent<CardMove>().IsAttack_first && enemyCards[fightNum] == null)) && playerCards[fightNum].GetComponent<CardMove>().Health > 0)
        {
            if (!playerCards[fightNum].GetComponent<CardMove>().isFightInThisBout)
            {
                isPlayerBout = true;
                //先找到需要攻击的敌人
                playerCards[fightNum].GetComponent<CardMove>().Enemyindex = FindAnalogue(fightNum);
                //切换武将状态为正在攻击
                playerCards[fightNum].GetComponent<CardMove>().IsAttack = StateOfAttack.FightNow;
                isFightNow = true;
                //记录当前武将在该回合已进行过攻击
                playerCards[fightNum].GetComponent<CardMove>().isFightInThisBout = true;
            }
            else
            {
                if (enemyCards[fightNum] != null && enemyCards[fightNum].GetComponent<CardMove>().Health > 0)
                {
                    isPlayerBout = false;
                    enemyCards[fightNum].GetComponent<CardMove>().Enemyindex = FindAnalogue(fightNum);
                    //切换武将状态为正在攻击
                    enemyCards[fightNum].GetComponent<CardMove>().IsAttack = StateOfAttack.FightNow;
                    isFightNow = true;
                }
                playerCards[fightNum++].GetComponent<CardMove>().isFightInThisBout = false;
            }
            return;
        }

        if (enemyCards[fightNum] != null && (enemyCards[fightNum].GetComponent<CardMove>().IsAttack_first || (!enemyCards[fightNum].GetComponent<CardMove>().IsAttack_first && playerCards[fightNum] == null)) && enemyCards[fightNum].GetComponent<CardMove>().Health > 0)
        {
            if (!enemyCards[fightNum].GetComponent<CardMove>().isFightInThisBout)
            {
                isPlayerBout = false;
                enemyCards[fightNum].GetComponent<CardMove>().Enemyindex = FindAnalogue(fightNum);
                enemyCards[fightNum].GetComponent<CardMove>().IsAttack = StateOfAttack.FightNow;
                isFightNow = true;
                enemyCards[fightNum].GetComponent<CardMove>().isFightInThisBout = true;
            }
            else
            {
                if (playerCards[fightNum] != null && playerCards[fightNum].GetComponent<CardMove>().Health > 0)
                {
                    isPlayerBout = true;
                    playerCards[fightNum].GetComponent<CardMove>().Enemyindex = FindAnalogue(fightNum);
                    playerCards[fightNum].GetComponent<CardMove>().IsAttack = StateOfAttack.FightNow;
                    isFightNow = true;
                }
                enemyCards[fightNum++].GetComponent<CardMove>().isFightInThisBout = false;
            }
            return;
        }
        fightNum++;
    }


    int selectEnemy;
    //找到要攻击的对手
    private GameObject FindAnalogue(int i)
    {
        if (isPlayerBout)
        {
            if (enemyCards[fightNum] != null && enemyCards[fightNum].GetComponent<CardMove>().Health > 0)
            {
                return enemyCards[fightNum];
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
                        //敌方扣血

                        //金币
                        updateBtn.GetComponent<CreateAndUpdate>().money += 1;   //玩家加金币

                        //展示战斗胜负信息
                        if (remainScale >= 0.75f)
                        {
                            victoryOrFailureText.text = "完胜";
                        }
                        else
                        {
                            if (remainScale >= 0.5f)
                            {
                                victoryOrFailureText.text = "大胜";
                            }
                            else
                            {
                                if (remainScale >= 0.25f)
                                {
                                    victoryOrFailureText.text = "小胜";
                                }
                                else
                                {
                                    victoryOrFailureText.text = "险胜";
                                }
                            }
                        }
                        victoryOrFailureText.gameObject.SetActive(true);
                        //RecoverCardData();  //战斗结算结束恢复卡牌数值
                        return null;
                    }
                }
                return enemyCards[selectEnemy];
            }
        }
        else
        {
            if (playerCards[fightNum] != null && playerCards[fightNum].GetComponent<CardMove>().Health > 0)
            {
                return playerCards[fightNum];
            }
            else
            {
                selectEnemy = 0;
                while (playerCards[selectEnemy] == null || playerCards[selectEnemy].GetComponent<CardMove>().Health <= 0)
                {
                    selectEnemy++;
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
                        float remainScale = (float)remainingHP / fullHP;    //玩家剩余血量比例
                        updateBtn.GetComponent<CreateAndUpdate>().hp -= (int)(remainScale * 10);    //玩家扣血
                        //金币
                        updateBtn.GetComponent<CreateAndUpdate>().money += 0;   //玩家不加金币

                        //展示战斗胜负信息
                        if (remainScale >= 0.75f)
                        {
                            victoryOrFailureText.text = "惨败";
                        }
                        else
                        {
                            if (remainScale >= 0.5f)
                            {
                                victoryOrFailureText.text = "大败";
                            }
                            else
                            {
                                if (remainScale >= 0.25f)
                                {
                                    victoryOrFailureText.text = "小败";
                                }
                                else
                                {
                                    victoryOrFailureText.text = "惜败";
                                }
                            }
                        }
                        victoryOrFailureText.gameObject.SetActive(true);
                        //RecoverCardData();  
                        return null;
                    }
                }
                return playerCards[selectEnemy];
            }
        }
    }

    /// <summary>
    /// 重置卡牌数值
    /// </summary>
    public void RecoverCardData()
    {
        for (int n = 0; n < 9; n++)
        {
            if (playerCards[n] != null)
            {
                Destroy(playerCards[n].gameObject);
            }
            if (enemyCards[n] != null)
            {
                enemyCards[n].GetComponent<Slider>().value = 1;
                enemyCards[n].GetComponent<CardMove>().Health = enemyCards[n].GetComponent<CardMove>().Fullhealth;
                //Destroy(enemyCards[n].gameObject);
            }
        }
        //玩家血条刷新
        player_hp.value = updateBtn.GetComponent<CreateAndUpdate>().hp / 100f;
        player_hp.transform.GetChild(3).GetComponent<Text>().text = updateBtn.GetComponent<CreateAndUpdate>().hp.ToString();
    }
}
