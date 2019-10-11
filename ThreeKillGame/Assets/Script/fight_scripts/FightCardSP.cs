using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCardSP : MonoBehaviour
{
    private bool isEndOFInit;   //记录是否初始化结束
    private int roundNum;   //记录当前回合数量
    private int fightNum;   //记录当前攻击武将的位置
    private bool isPlayerBout;  //记录是否是玩家的武将攻击回合
    [HideInInspector]
    public static bool isFightNow;  //记录现在是否正在攻击
    
    public Transform jiugongge_BrforeFight;  //上阵布阵的九宫格
    public GameObject heroFightCard;    //英雄战斗卡片预制件
    public Transform[] OwnJiuGonggePos=new Transform[9];  //战斗槽位置_玩家
    
    [SerializeField]
    GameObject[] enemyCards = new GameObject[9];//存储敌人卡牌
    GameObject[] playerCards = new GameObject[9];//存储己方卡牌


    private void Start()
    {
        isEndOFInit = false;
        fightNum = 0;
        roundNum = 1;
        isPlayerBout = true;
        isFightNow = false;
    }

    private void OnEnable()
    {
        InitializeBattleCard();
        isEndOFInit = true;
    }

    /// <summary>
    /// 初始化战斗卡牌
    /// </summary>
    public void InitializeBattleCard()
    {
        for (int i = 0; i < 9; i++)
        {
            playerCards[i] = null;
            if (jiugongge_BrforeFight.GetChild(i).childCount>0)
            {
                playerCards[i]=Instantiate(heroFightCard,OwnJiuGonggePos[i]);
                playerCards[i].transform.position = new Vector3(0,0,0);
                playerCards[i].transform.SetParent(OwnJiuGonggePos[i].parent.parent);
            }
        }
        OtherInitialization();
    }

    private void OtherInitialization()
    {
        //敌方卡牌初始化
        for (int i = 0; i < enemyCards.Length; i++)
        {
            if (enemyCards[i] != null)
            {
                //enemyCards[i].AddComponent<CardMove>();
                
                enemyCards[i].GetComponent<CardMove>().IsAttack_first = ((i + 2) % 2 == 0) ? false : true;
                enemyCards[i].GetComponent<CardMove>().Health = enemyCards[i].GetComponent<CardMove>().Fullhealth = 100;
                enemyCards[i].GetComponent<CardMove>().Force = 60;
                enemyCards[i].GetComponent<CardMove>().Defence = 5;
            }
        }
        //玩家卡牌初始化
        for (int i = 0; i < playerCards.Length; i++)
        {
            if (playerCards[i] != null)
            {
                Debug.Log("///////+"+jiugongge_BrforeFight.GetChild(i).GetChild(0).GetComponent<HeroDataControll>().heroData.Count);//
                //playerCards[i].AddComponent<CardMove>();
                //设置地方卡牌的默认先后手情况/血量/攻击力/防御力
                playerCards[i].GetComponent<CardMove>().IsAttack_first = ((i + 2) % 2 == 0) ? true : false;
                playerCards[i].GetComponent<CardMove>().Health = playerCards[i].GetComponent<CardMove>().Fullhealth = int.Parse(jiugongge_BrforeFight.GetChild(i).GetChild(0).GetComponent<HeroDataControll>().heroData[8]);
                playerCards[i].GetComponent<CardMove>().Force = int.Parse(jiugongge_BrforeFight.GetChild(i).GetChild(0).GetComponent<HeroDataControll>().heroData[6]);
                playerCards[i].GetComponent<CardMove>().Defence = int.Parse(jiugongge_BrforeFight.GetChild(i).GetChild(0).GetComponent<HeroDataControll>().heroData[7]);
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
            roundNum++;
            fightNum = 0;
        }
        //若有武将正在攻击
        if (isFightNow)
            return;

        if (playerCards[fightNum] != null && playerCards[fightNum].GetComponent<CardMove>().IsAttack_first && playerCards[fightNum].GetComponent<CardMove>().Health > 0)
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

        if (enemyCards[fightNum] != null && enemyCards[fightNum].GetComponent<CardMove>().IsAttack_first && enemyCards[fightNum].GetComponent<CardMove>().Health > 0)
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
                    if (selectEnemy > enemyCards.Length)
                    {
                        Debug.Log("玩家获胜");
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
                    if (selectEnemy > playerCards.Length)
                    {
                        Debug.Log("电脑获胜");
                        return null;
                    }
                }
                return playerCards[selectEnemy];
            }
        }
    }
}
