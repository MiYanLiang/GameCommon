using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCardSP : MonoBehaviour
{

    private int roundNum;   //记录当前回合数量
    private int fightNum;   //记录当前攻击武将的位置
    private bool isPlayerBout;  //记录是否是玩家的武将攻击回合
    public static bool isFightNow;  //记录现在是否正在攻击


    public GameObject[] enemyCards = new GameObject[9];//存储敌人卡牌
    public GameObject[] playerCards = new GameObject[9];//存储己方卡牌

    private void Awake()
    {
        fightNum = 0;
        roundNum = 1;
        isPlayerBout = true;
        isFightNow = false;
        //敌方卡牌初始化
        for (int i = 0; i < enemyCards.Length; i++)
        {
            if (enemyCards[i] != null)
            {
                enemyCards[i].AddComponent<CardMove>();
                //设置地方卡牌的默认先后手情况
                enemyCards[i].GetComponent<CardMove>().IsAttack_first = ((i + 2) % 2 == 0) ? false : true;
            }
        }
        //玩家卡牌初始化
        for (int i = 0; i < enemyCards.Length; i++)
        {
            if (playerCards[i] != null)
            {
                playerCards[i].AddComponent<CardMove>();
                playerCards[i].GetComponent<CardMove>().IsAttack_first = ((i + 2) % 2 == 0) ? true : false;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
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
                while (enemyCards[selectEnemy].GetComponent<CardMove>().Health <= 0 || enemyCards[selectEnemy] == null)
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
                while (playerCards[selectEnemy].GetComponent<CardMove>().Health <= 0 || playerCards[selectEnemy] == null)
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
