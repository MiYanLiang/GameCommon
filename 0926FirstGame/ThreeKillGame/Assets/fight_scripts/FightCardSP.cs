using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCardSP : MonoBehaviour {

    private int roundNum;   //记录当前回合数量
    private int fightNum;   //记录当前攻击武将的位置
    private bool isPlayerBout;  //记录是否是玩家的武将攻击回合

    public GameObject[] enemyCards = new GameObject[9];//存储敌人卡牌
    public GameObject[] playerCards = new GameObject[9];//存储己方卡牌

    private void Awake()
    {
        fightNum = 0;
        roundNum = 1;
        isPlayerBout = true;
        //敌方卡牌初始化
        for (int i = 0; i < enemyCards.Length; i++)
        {
            if (enemyCards[i]!=null)
            {
                enemyCards[i].AddComponent<CardMove>();
                //设置地方卡牌的默认先后手情况
                enemyCards[i].GetComponent<CardMove>().IsAttack_first = ((i + 2) % 2 == 0) ? false : true;
            }
        }
        //玩家卡牌初始化
        for (int i = 0; i < enemyCards.Length; i++)
        {
            if (playerCards[i]!=null)
            {
                playerCards[i].AddComponent<CardMove>();
                playerCards[i].GetComponent<CardMove>().IsAttack_first = ((i + 2) % 2 == 0) ? true : false;
            }
        }

    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        ////  该位置有武将牌                 该武将的攻击状态为备战                                                                  该武将血量不为零                                             现在是该玩家武将攻击时机
        //if (playerCards[fightNum]!=null && playerCards[fightNum].GetComponent<CardMove>().IsAttack==StateOfAttack.ReadyForFight && playerCards[fightNum].GetComponent<CardMove>().Health > 0 && isPlayerBout)
        
        //回合增加
        if (fightNum >= playerCards.Length)
        {
            roundNum++;
            fightNum = 0;
        }
        //若有武将正在攻击
        if (playerCards[fightNum].GetComponent<CardMove>().IsAttack != StateOfAttack.ReadyForFight || enemyCards[fightNum].GetComponent<CardMove>().IsAttack != StateOfAttack.ReadyForFight)
            return;
        else
        {
            if (isPlayerBout)
            {
                isPlayerBout = false;
                if (playerCards[fightNum] != null && playerCards[fightNum].GetComponent<CardMove>().Health > 0)
                {
                    //先找到需要攻击的敌人
                    playerCards[fightNum].GetComponent<CardMove>().Enemyindex = FindAnalogue(fightNum);
                    //切换武将状态为正在攻击
                    playerCards[fightNum].GetComponent<CardMove>().IsAttack = StateOfAttack.FightNow;
                }
            }
            else
            {
                isPlayerBout = true;
                if (enemyCards[fightNum] != null && enemyCards[fightNum].GetComponent<CardMove>().Health > 0)
                {
                    enemyCards[fightNum].GetComponent<CardMove>().Enemyindex = FindAnalogue(fightNum);
                    enemyCards[fightNum].GetComponent<CardMove>().IsAttack = StateOfAttack.FightNow;
                }
                fightNum++;
            }
            
        }
        

	}

    //找到要攻击的对手
    private GameObject FindAnalogue(int i)
    {
        if (isPlayerBout)
        {
            if (enemyCards[fightNum] != null && enemyCards[fightNum].GetComponent<CardMove>().Health>0)
            {
                return enemyCards[fightNum];
            }
            else
            {
                fightNum = 0;
                while (enemyCards[fightNum].GetComponent<CardMove>().Health < 0 || enemyCards[fightNum] == null)
                {
                    fightNum++;
                    if (fightNum > enemyCards.Length)
                    {
                        Debug.Log("玩家获胜");
                        return null;
                    }
                }
                return enemyCards[fightNum];
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
                fightNum = 0;
                while (playerCards[fightNum].GetComponent<CardMove>().Health < 0 || playerCards[fightNum] == null)
                {
                    fightNum++;
                    if (fightNum > playerCards.Length)
                    {
                        Debug.Log("电脑获胜");
                        return null;
                    }
                }
                return playerCards[fightNum];
            }
        }
    }
}
