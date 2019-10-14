using OfficeOpenXml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroDataControll : MonoBehaviour {

    //[HideInInspector]
    public List<string> heroData=new List<string>(); //存储当前武将的数据
    //index	    roleName	所属势力    soldierKind	  rarity  recruitingMoney  attack    defense	soldierNum	    闪避率	
    //0         1           2           3             4       5                6         7          8               9       
    //暴击率	暴击伤害	重击率	    重击伤害	  破甲    装备id           典故id	 武器技id	强化兵种技id	羁绊技id
    //10        11          12          13            14      15               16        17         18              19
    public int grade_hero;  //记录卡牌品阶

    // Use this for initialization
    void Start () {
        
    }

    /// <summary>
    /// 显示英雄名等数据信息
    /// </summary>
    public void ShowThisHeroData()
    {
        if (heroData != null)
        {
            //Debug.Log("//传递成功--"+heroData[1]);
            //显示英雄名等信息
            gameObject.transform.GetChild(0).GetComponent<Text>().text = heroData[1];
            gameObject.transform.GetChild(1).GetComponent<Text>().text = heroData[6];
            gameObject.transform.GetChild(2).GetComponent<Text>().text = heroData[7];
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
