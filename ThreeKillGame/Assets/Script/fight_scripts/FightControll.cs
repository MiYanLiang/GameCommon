using System.Collections;
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

    //public Text[] forceNames;   //游戏内势力单个字显示

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

    private void Awake()
    {
        //playerForceId= PlayerPrefs.GetInt("forcesId"); //玩家自身的势力id
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
        CreateEnemyUnits(); //初始化所有NPC势力发展阵容
        ChangeAllEnemyCards();
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
                    //设置品阶颜色表现和属性
                    switch (int.Parse(enemyHeroDatas[i][m][enemyHeroDatas[i][m].Count - 2]))
                    {
                        case 1:
                            newheroCard.GetComponent<Image>().color = Color.white;
                            break;
                        case 2:
                            newheroCard.GetComponent<Image>().color = Color.blue;
                            break;
                        case 3:
                            newheroCard.transform.GetComponent<Image>().color = Color.red;
                            break;
                        default:
                            newheroCard.transform.GetComponent<Image>().color = Color.white;
                            break;
                    }
                    //显示英雄名等信息
                    newheroCard.transform.GetChild(0).GetComponent<Text>().text = enemyHeroDatas[i][m][0] + ":" + enemyHeroDatas[i][m][1];
                    newheroCard.transform.GetChild(1).GetComponent<Text>().text = enemyHeroDatas[i][m][6];
                    newheroCard.transform.GetChild(2).GetComponent<Text>().text = enemyHeroDatas[i][m][7];
                }
            }

        }
    }

    /// <summary>
    /// 开始战斗，传递数据到战斗界面
    /// </summary>
    public void StartFightSendHeroData()
    {
        FightCardSps[0].GetComponent<FightCardSP>().array_str = enemyHeroDatas[1];
        FightCardSps[0].gameObject.SetActive(true);
    }


    //打开或者关闭战斗界面
    public void OpenOrCloseFightTV(bool boo)
    {
        for (int i = 0; i < 6; i++)
        {
            FightTVs[i].gameObject.SetActive(boo);
        }
    }



    //初始化创建敌方势力和后期要发展的兵种类型
    private void CreateEnemyUnits()
    {
        for (int i = 0; i < 5; i++)
        {
            enemyUnits[i] = new int[3];
            enemyUnits[i][0] = Random.Range(1, 7);     //前排
            enemyUnits[i][1] = Random.Range(1, 10);    //中排
            enemyUnits[i][2] = Random.Range(4, 10);    //后排
            //Debug.Log("///" + enemyUnits[i][0] + "///" + enemyUnits[i][1] + "////" + enemyUnits[i][2]);
            
        }
    }
}
