using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

//敌方武将卡牌数据初始化和控制
public class EmFightControll : MonoBehaviour
{
    private static int hardNum;    //难度值

    //可随机的英雄id
    List<int> canGetHeroId = new List<int>();
    //存放最后拿到的英雄Id
    List<int> getHeroId = new List<int>();

    //上阵位置和开启周目                   [0,0]                                 [1,1]     
    int[,] arrayBattles = new int[2, 10] { { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };

    List<string>[] array_str = new List<string>[9]; //存储最终传递的英雄数据

    private void Awake()
    {
        hardNum = PlayerPrefs.GetInt("DifficultyType");  //难度值获取
    }

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            arrayBattles[1, i] = int.Parse(LoadJsonFile.DifficultyTableDates[i][hardNum+1]);
        }

        for (int i = 0; i < 10; i++)
        {
            print(arrayBattles[1,i]);
        }
    }

    /// <summary>
    /// 接收敌方武将现有的上阵卡牌，进行卡牌变化和传递
    /// </summary>
    /// <param name="arrHeroData">武将数据</param>
    /// <param name="enemyUnits">阵容兵种定位[前排，中排，后排]</param>
    /// <param name="battles">总周目数</param>
    /// <returns></returns>
    public List<string>[] SendHeroData(List<string>[] arrHeroData, int[] enemyUnits, int battles)
    {
        int heroCount = 0;  //记录英雄数
        for (int i = 0; i < arrHeroData.Length; i++)    //依次处理传过来的九个位置
        {
            if (arrHeroData[i] != null)   //若该位置有英雄
            {
                heroCount++;
                switch (arrHeroData[i][1])  //判断英雄的品阶       
                {
                    case "1":
                        switch (LoadJsonFile.RoleTableDatas[int.Parse(arrHeroData[i][0])-1][4])   //判断稀有度
                        {
                            case "1":   //绿1
                                if (int.Parse(arrHeroData[i][2]) >= int.Parse(LoadJsonFile.difficultyChooseDatas[hardNum-1][7]))  //判断此卡牌参与的战斗周目
                                {
                                    if (GradeOrColor()) //升阶
                                    {
                                        array_str[i] = UpGrade(2, int.Parse(arrHeroData[i][0]) + 1);  //执行升阶方法传递所需阶值和英雄id
                                    }
                                    else                //升色
                                    {
                                        array_str[i] = UpColor(LoadJsonFile.RoleTableDatas[int.Parse(arrHeroData[i][0]) - 1][3], "2");  //执行升色方法传递所需兵种和升色值（稀有度1234）
                                        if (array_str[i] == null) HoldHeroData(arrHeroData, i);   //保持英雄数据
                                    }
                                }
                                else
                                {
                                    HoldHeroData(arrHeroData, i);   //保持英雄数据
                                }
                                break;
                            case "2":   //蓝1
                                if (int.Parse(arrHeroData[i][2]) >= int.Parse(LoadJsonFile.difficultyChooseDatas[hardNum - 1][8]))
                                {
                                    if (GradeOrColor()) //升阶
                                    {
                                        array_str[i] = UpGrade(2, int.Parse(arrHeroData[i][0]) + 1);  //执行升阶方法传递所需阶值和英雄id
                                    }
                                    else                //升色
                                    {
                                        array_str[i] = UpColor(LoadJsonFile.RoleTableDatas[int.Parse(arrHeroData[i][0]) - 1][3], "3");  //执行升色方法传递所需兵种和升色值（稀有度1234）
                                        if (array_str[i] == null) HoldHeroData(arrHeroData, i);   //保持英雄数据
                                    }
                                }
                                else
                                {
                                    HoldHeroData(arrHeroData, i);   //保持英雄数据
                                }
                                break;
                            case "3":   //紫1
                                if (int.Parse(arrHeroData[i][2]) >= int.Parse(LoadJsonFile.difficultyChooseDatas[hardNum - 1][9]))
                                {
                                    if (GradeOrColor()) //升阶
                                    {
                                        array_str[i] = UpGrade(2, int.Parse(arrHeroData[i][0]) + 1);  //执行升阶方法传递所需阶值和英雄id
                                    }
                                    else                //升色
                                    {
                                        array_str[i] = UpColor(LoadJsonFile.RoleTableDatas[int.Parse(arrHeroData[i][0]) - 1][3], "4");  //执行升色方法传递所需兵种和升色值（稀有度1234）
                                        if (array_str[i] == null) HoldHeroData(arrHeroData, i);   //保持英雄数据
                                    }
                                }
                                else
                                {
                                    HoldHeroData(arrHeroData, i);   //保持英雄数据
                                }
                                break;
                            case "4":   //橙1
                                if (int.Parse(arrHeroData[i][2]) >= int.Parse(LoadJsonFile.difficultyChooseDatas[hardNum - 1][10]))
                                {
                                    if (GradeOrColor()) //升阶
                                    {
                                        array_str[i] = UpGrade(2, int.Parse(arrHeroData[i][0]) + 1);  //执行升阶方法传递所需阶值和英雄id
                                    }
                                }
                                else
                                {
                                    HoldHeroData(arrHeroData, i);   //保持英雄数据
                                }
                                break;
                        }
                        break;
                    case "2":
                        switch (LoadJsonFile.RoleTableDatas[int.Parse(arrHeroData[i][0]) - 1][4])   //判断稀有度
                        {
                            case "1":   //绿2
                                if (int.Parse(arrHeroData[i][2]) >= int.Parse(LoadJsonFile.difficultyChooseDatas[hardNum - 1][11]) && GradeOrColor())  //判断此卡牌参与的战斗周目
                                {
                                    //二阶英雄升阶，失败后不用升色
                                    {
                                        array_str[i] = UpGrade(3, int.Parse(arrHeroData[i][0]) + 1);  //执行升阶方法传递所需阶值和英雄id
                                    }
                                }
                                else
                                {
                                    HoldHeroData(arrHeroData, i);   //保持英雄数据
                                }
                                break;
                            case "2":   //蓝2
                                if (int.Parse(arrHeroData[i][2]) >= int.Parse(LoadJsonFile.difficultyChooseDatas[hardNum - 1][12]) && GradeOrColor())
                                {
                                    //升阶
                                    {
                                        array_str[i] = UpGrade(3, int.Parse(arrHeroData[i][0]) + 1);  //执行升阶方法传递所需阶值和英雄id
                                    }
                                }
                                else
                                {
                                    HoldHeroData(arrHeroData, i);   //保持英雄数据
                                }
                                break;
                            case "3":   //紫2
                                if (int.Parse(arrHeroData[i][2]) >= int.Parse(LoadJsonFile.difficultyChooseDatas[hardNum - 1][13]) && GradeOrColor())
                                {
                                    //升阶
                                    {
                                        array_str[i] = UpGrade(3, int.Parse(arrHeroData[i][0]) + 1);  //执行升阶方法传递所需阶值和英雄id
                                    }
                                }
                                else
                                {
                                    HoldHeroData(arrHeroData, i);   //保持英雄数据
                                }
                                break;
                            case "4":   //橙2
                                if (int.Parse(arrHeroData[i][2]) >= int.Parse(LoadJsonFile.difficultyChooseDatas[hardNum - 1][14]) && GradeOrColor())
                                {
                                    //升阶
                                    {
                                        array_str[i] = UpGrade(3, int.Parse(arrHeroData[i][0]) + 1);  //执行升阶方法传递所需阶值和英雄id
                                    }
                                }
                                else
                                {
                                    HoldHeroData(arrHeroData, i);   //保持英雄数据
                                }
                                break;
                        }
                        break;
                    case "3":   //三阶英雄保持数据
                        HoldHeroData(arrHeroData, i);   //保持英雄数据
                        break;
                }
            }
        }

        //更新英雄上阵位置和个数
        if (battles < arrayBattles[1, 1])
        {
            if (heroCount < arrayBattles[0, 0])
            {
                //添加英雄 
                //ClearDate();
                AddHeros(enemyUnits, arrayBattles[0, 0] - heroCount);
            }
        }
        else
        {
            if (battles < arrayBattles[1, 2])
            {
                if (heroCount < arrayBattles[0, 1])
                {
                    //ClearDate(); 
                    AddHeros(enemyUnits, arrayBattles[0, 1] - heroCount);
                }
            }
            else
            {
                if (battles < arrayBattles[1, 3])
                {
                    if (heroCount < arrayBattles[0, 2])
                    {
                        //ClearDate();
                        AddHeros(enemyUnits, arrayBattles[0, 2] - heroCount);
                    }
                }
                else
                {
                    if (battles < arrayBattles[1, 4])
                    {
                        if (heroCount < arrayBattles[0, 3])
                        {
                            //ClearDate();
                            AddHeros(enemyUnits, arrayBattles[0, 3] - heroCount);
                        }
                    }
                    else
                    {
                        if (battles < arrayBattles[1, 5])
                        {
                            if (heroCount < arrayBattles[0, 4])
                            {
                                //ClearDate();
                                AddHeros(enemyUnits, arrayBattles[0, 4] - heroCount);
                            }
                        }
                        else
                        {
                            if (battles < arrayBattles[1, 6])
                            {
                                if (heroCount < arrayBattles[0, 5])
                                {
                                    //ClearDate();
                                    AddHeros(enemyUnits, arrayBattles[0, 5] - heroCount);
                                }
                            }
                            else
                            {
                                if (battles < arrayBattles[1, 7])
                                {
                                    if (heroCount < arrayBattles[0, 6])
                                    {
                                        //ClearDate();
                                        AddHeros(enemyUnits, arrayBattles[0, 6] - heroCount);
                                    }
                                }
                                else
                                {
                                    if (battles < arrayBattles[1, 8])
                                    {
                                        if (heroCount < arrayBattles[0, 7])
                                        {
                                            //ClearDate();
                                            AddHeros(enemyUnits, arrayBattles[0, 7] - heroCount);
                                        }
                                    }
                                    else
                                    {
                                        if (battles < arrayBattles[1, 9])
                                        {
                                            if (heroCount < arrayBattles[0, 8])
                                            {
                                                //ClearDate();
                                                AddHeros(enemyUnits, arrayBattles[0, 8] - heroCount);
                                            }
                                        }
                                        else
                                        {
                                            if (heroCount < arrayBattles[0, 9])
                                            {
                                                //ClearDate();
                                                AddHeros(enemyUnits, arrayBattles[0, 9] - heroCount);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        return array_str;
    }
    
    //enemyUnits[0] = Random.Range(1, 7);     //前排
    //enemyUnits[1] = Random.Range(1, 10);    //中排
    //enemyUnits[2] = Random.Range(4, 10);    //后排
    //array_str   [0]heroid [3]kind兵种 [4]稀有度
    /// <summary>
    /// 添加敌方英雄卡牌
    /// </summary>
    /// <param name="enemyUnits">兵种类型</param>
    /// <param name="v">添加个数</param>
    private void AddHeros(int[] enemyUnits, int v)
    {
        //记录现有英雄的种类个数
        int[] hero_kindNum = new int[3] { 0, 0, 0 };
        //记录所有绿色稀有度英雄id
        List<int> heroIds = new List<int>();
        //记录所有新加英雄的id
        List<int> newheroIds = new List<int>();

        for (int i = 0; i < 9; i++)
        {
            if (array_str[i]!=null)
            {
                if (array_str[i][4]=="1")   //判断稀有度是否为1
                {
                    heroIds.Add(int.Parse(array_str[i][0]));
                }
                int bingzhong = int.Parse(array_str[i][3]);
                if (bingzhong==enemyUnits[0])
                {
                    hero_kindNum[0]++;
                }
                else
                {
                    if (bingzhong == enemyUnits[1])
                    {
                        hero_kindNum[1]++;
                    }
                    else
                    {
                        if (bingzhong == enemyUnits[2])
                        {
                            hero_kindNum[2]++;
                        }
                    }
                }
            }
        }
        //添加v个英雄
        for (int i = 0; i < v; i++)
        {
            int id_newHero=0;
            if (hero_kindNum[0]<3)
            {
                int index = 0;
                while (true)
                {
                    if (LoadJsonFile.RoleTableDatas[index++][3] == enemyUnits[0].ToString() && LoadJsonFile.RoleTableDatas[index-1][4] == "1")
                        break;
                }
                int id = index;
                do
                {
                    id_newHero = Random.Range(0, 3) + id;
                } while (IsHadThisHero(heroIds, id_newHero));
                newheroIds.Add(id_newHero);
                hero_kindNum[0]++;
                continue;
            }
            if (hero_kindNum[1]<3)
            {
                int index = 0;
                while (true)
                {
                    if (LoadJsonFile.RoleTableDatas[index++][3] == enemyUnits[1].ToString() && LoadJsonFile.RoleTableDatas[index-1][4] == "1")
                        break;
                }
                int id = index;
                do
                {
                    id_newHero = Random.Range(0, 3) + id;
                } while (IsHadThisHero(heroIds, id_newHero));
                newheroIds.Add(id_newHero);
                hero_kindNum[1]++;
                continue;
            }
            if (hero_kindNum[2]<3)
            {
                int index = 0;
                while (true)
                {
                    if (LoadJsonFile.RoleTableDatas[index++][3] == enemyUnits[2].ToString() && LoadJsonFile.RoleTableDatas[index-1][4] == "1")
                        break;
                }
                int id = index;
                do
                {
                    id_newHero = Random.Range(0, 3) + id;
                } while (IsHadThisHero(heroIds, id_newHero));
                newheroIds.Add(id_newHero);
                hero_kindNum[2]++;
                continue;
            }
        }

        int num = 0;
        //添加新英雄到总英雄数据中
        for (int i = 0; i < 9; i++)
        {
            if (num>=v)
            {
                break;
            }
            if (array_str[i]==null)
            {
                //添加新英雄的数据
                array_str[i] = LoadJsonFile.DeepClone<string>(LoadJsonFile.RoleTableDatas[newheroIds[num] - 1]);
                array_str[i].Add("1");  //品阶
                array_str[i].Add("1");  //战斗周目数
                num++;
            }
        }
    }



    /// <summary>
    /// 判断是否已经有此英雄
    /// </summary>
    /// <param name="heroIds"></param>
    /// <param name="id_newHero"></param>
    /// <returns></returns>
    private bool IsHadThisHero(List<int> heroIds, int id_newHero)
    {
        int num = heroIds.Count;
        //若没有英雄则直接返回
        if (num < 1) return false;     
        for (int i = 0; i < num; i++)
        {
            if (heroIds[i] == id_newHero)   //有该英雄
            {
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// 升色方法
    /// </summary>
    /// <param name="v1">兵种</param>
    /// <param name="v2">颜色值</param>
    /// <returns></returns>
    private List<string> UpColor(string v1, string v2)
    {
        List<string> str_list = new List<string>();
        //随机拿到符合要求的武将数据，给品阶和战斗周目数为1
        List<int> heroids=new List<int>();
        for (int i = 0; i < LoadJsonFile.RoleTableDatas.Count; i++) //遍历所有武将
        {
            //判断兵种和稀有度（颜色）
            if (LoadJsonFile.RoleTableDatas[i][3] == v1 && LoadJsonFile.RoleTableDatas[i][4] == v2)
            {
                heroids.Add(i+1);   //存储符合的英雄id
            }
        }
        int num = heroids.Count;    //符合的英雄总数
        if (num <= 0)
        {
            Debug.Log("///没有符合的升色英雄///");
            return null;
        }
        str_list = LoadJsonFile.DeepClone<string>(LoadJsonFile.RoleTableDatas[heroids[Random.Range(0, num)]-1]);
        str_list.Add("1");  //品阶
        str_list.Add("1");  //战斗周目数
        return str_list;
    }

    /// <summary>
    /// 升阶方法
    /// </summary>
    /// <param name="v1">所升阶数</param>
    /// <param name="v2">英雄id</param>
    /// <returns></returns>
    private List<string> UpGrade(int v1, int v2)
    {
        //传递武将数据链表尾部依次添加品阶和战斗周目数
        List<string> str_list = new List<string>();
        str_list = LoadJsonFile.DeepClone<string>(LoadJsonFile.RoleTableDatas[v2 - 1]);
        str_list.Add(v1.ToString());
        str_list.Add("1");
        return str_list;
    }

    /// <summary>
    /// 保持英雄数据
    /// </summary>
    /// <param name="arrHeroData"></param>
    /// <param name="i"></param>
    private void HoldHeroData(List<string>[] arrHeroData, int i)   
    {
        List<string> newHeroData = new List<string>();
        newHeroData = LoadJsonFile.DeepClone<string>(LoadJsonFile.RoleTableDatas[int.Parse(arrHeroData[i][0]) - 1]);  //记录该英雄所有数据
        array_str[i] = newHeroData;
        array_str[i].Add(arrHeroData[i][1]);    //记录他的品阶
        array_str[i].Add((int.Parse(arrHeroData[i][2])+1).ToString());    //记录他的参与战斗周目数+1
    }

    //判断是升阶（true）还是升色（false）
    private bool GradeOrColor()
    {
        int num = Random.Range(1, 101);
        if (num <= 30)  
            return true;
        else
            return false;
    }

}
