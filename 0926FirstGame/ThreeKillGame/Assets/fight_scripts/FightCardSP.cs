using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCardSP : MonoBehaviour {

    public GameObject[] enemyCards;
    public GameObject[] playerCards;

    private void Awake()
    {
        //敌方卡牌初始化
        for (int i = 0; i < enemyCards.Length; i++)
        {
            enemyCards[i].AddComponent<CardMove>();
            //设置地方卡牌的默认先后手情况
            enemyCards[i].GetComponent<CardMove>().IsAttack_first = ((i + 2) % 2 == 0) ? false : true;
        }
        //玩家卡牌初始化
        for (int i = 0; i < enemyCards.Length; i++)
        {
            playerCards[i].AddComponent<CardMove>();
            playerCards[i].GetComponent<CardMove>().IsAttack_first = ((i + 2) % 2 == 0) ? true : false;
        }

    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < playerCards.Length; i++)
        {
            playerCards[i].GetComponent<CardMove>().Enemyindex = FindAnalogue(i);
            playerCards[i].GetComponent<CardMove>().ChangeAttackState();
        }
	}

    //找到要攻击的对手
    private GameObject FindAnalogue(int i)
    {
        if (enemyCards[i].GetComponent<CardMove>().Health>0)
        {
            return enemyCards[i];
        }
        else
        {
            i = 0;
            while (enemyCards[i].GetComponent<CardMove>().Health < 0)
            {
                i++;
            }
            return enemyCards[i];
        }
    }
}
