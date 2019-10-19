using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战斗控制脚本
/// </summary>
public class FightControll : MonoBehaviour
{
    //记录敌方五个势力的卡牌
    //public List<GameObject[]> enemyCards_Controll=new List<GameObject[]>();
    //各个战斗控制控件0为玩家
    public Transform[] FightCardSps=new Transform[6];
    //各个战斗画面
    public GameObject[] FightTVs = new GameObject[6];

    private void Awake()
    {
        //添加五个敌方势力的初始卡牌
        //for (int i = 0; i < 5; i++)
        //{
        //    enemyCards_Controll.Add(new GameObject[9]);
        //}
        CreateEnemyUnits(); //初始化敌方发展阵容
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
    List<string>[] sendDatas = new List<string>[9];     //存储需要传递的数据
    /// <summary>
    /// 更新所有敌方阵容
    /// </summary>
    public void ChangeAllEnemyCards()
    {
        //玩家的敌方上阵英雄数据获取和传递
        for (int i = 0; i < 9; i++)
        {
            if (enemyHreoData[i] != null)
            {
                //传递ID,品阶,战斗周目数
                sendDatas[i].Add(enemyHreoData[i][0]);
                sendDatas[i].Add(enemyHreoData[i][enemyHreoData[i].Count - 2]);
                sendDatas[i].Add(enemyHreoData[i][enemyHreoData[i].Count - 1]);
            }
            else
            {
                sendDatas[i] = null;
            }
        }
        FightCardSps[0].GetComponent<FightCardSP>().array_str= EmFightControll.SendHeroData(sendDatas, enemyUnits, FightCardSps[0].GetComponent<FightCardSP>().battles- 1);    //npc武将数据更新
        for (int i = 0; i < sendDatas.Length; i++)
        {
            if (sendDatas[i]!=null)
            {
                sendDatas[i].Clear();
            }
        }

        FightCardSps[0].gameObject.SetActive(true);

    }

    public void OpenOrCloseFightTV(bool boo)
    {
        for (int i = 0; i < 6; i++)
        {
            FightTVs[i].gameObject.SetActive(boo);
        }
    }

}
