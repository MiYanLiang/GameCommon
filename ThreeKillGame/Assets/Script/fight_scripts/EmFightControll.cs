using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敌方武将卡牌数据初始化和控制
public class EmFightControll : MonoBehaviour
{
    public static int hardNum;    //难度值

    static UseEPPlusFun useepplusfun = new UseEPPlusFun();
    static TableDatas worksheet_Role;  //存储武将表
    static TableDatas worksheet_DFC;   //存储难度选择表

    private void Awake()
    {
        worksheet_Role = useepplusfun.FindExcelFiles("RoleTable1");
        worksheet_DFC = useepplusfun.FindExcelFiles("DifficultyChoose");
    }

    //上阵位置和开启周目                          [0,0]                                 [1,1]     
    static int[,] arrayBattles = new int[2, 10] { { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, { 1, 3, 4, 5, 6, 7, 11, 16, 21, 25 } };

    static List<string>[] array_str = new List<string>[9]; //存储最终传递的英雄数据

    /// <summary>
    /// 接收敌方武将现有的上阵卡牌，进行卡牌变化和传递
    /// </summary>
    /// <param name="arrHeroData">武将数据</param>
    /// <param name="enemyUnits">阵容兵种定位[前排，中排，后排]</param>
    /// <param name="battles">总周目数</param>
    /// <returns></returns>
    public static List<string>[] SendHeroData(List<string>[] arrHeroData, int[] enemyUnits,int battles)
    {
        int heroCount = 0;  //记录英雄数
        for (int i = 0; i < arrHeroData.Length; i++)    //依次处理传过来的九个位置
        {
            if (arrHeroData[i]!=null)   //若该位置有英雄
            {
                heroCount++;
                switch (arrHeroData[i][1])  //判断英雄的品阶       
                {
                    case "1":
                        switch (useepplusfun.GetRowAndColumnData(worksheet_Role, int.Parse(arrHeroData[i][0]), 5))   //判断稀有度
                        {
                            case "1":   //绿1
                                if (int.Parse(arrHeroData[i][2]) >= int.Parse(useepplusfun.GetRowAndColumnData(worksheet_DFC, hardNum+1, 8)))  //判断此卡牌参与的战斗周目
                                {
                                    if (GradeOrColor()) //升阶
                                    {
                                        array_str[i] = UpGrade(2, int.Parse(arrHeroData[i][0]) + 1);  //执行升阶方法传递所需阶值和英雄id
                                    }
                                    else                //升色
                                    {
                                        array_str[i] = UpColor(useepplusfun.GetRowAndColumnData(worksheet_Role, int.Parse(arrHeroData[i][0]) + 1, 4), "2");  //执行升色方法传递所需兵种和升色值（稀有度1234）
                                        if (array_str[i]==null) HoldHeroData(arrHeroData, i);   //保持英雄数据
                                    }
                                }
                                else
                                {
                                    HoldHeroData(arrHeroData, i);   //保持英雄数据
                                }
                                break;
                            case "2":   //蓝1
                                if (int.Parse(arrHeroData[i][2]) >= int.Parse(useepplusfun.GetRowAndColumnData(worksheet_DFC, hardNum + 1, 9)))  
                                {
                                    if (GradeOrColor()) //升阶
                                    {
                                        array_str[i] = UpGrade(2, int.Parse(arrHeroData[i][0]) + 1);  //执行升阶方法传递所需阶值和英雄id
                                    }
                                    else                //升色
                                    {
                                        array_str[i] = UpColor(useepplusfun.GetRowAndColumnData(worksheet_Role, int.Parse(arrHeroData[i][0]) + 1, 4), "3");  //执行升色方法传递所需兵种和升色值（稀有度1234）
                                        if (array_str[i]==null) HoldHeroData(arrHeroData, i);   //保持英雄数据
                                    }
                                }
                                else
                                {
                                    HoldHeroData(arrHeroData, i);   //保持英雄数据
                                }
                                break;
                            case "3":   //紫1
                                if (int.Parse(arrHeroData[i][2]) >= int.Parse(useepplusfun.GetRowAndColumnData(worksheet_DFC, hardNum + 1, 10)))  
                                {
                                    if (GradeOrColor()) //升阶
                                    {
                                        array_str[i] = UpGrade(2, int.Parse(arrHeroData[i][0]) + 1);  //执行升阶方法传递所需阶值和英雄id
                                    }
                                    else                //升色
                                    {
                                        array_str[i] = UpColor(useepplusfun.GetRowAndColumnData(worksheet_Role, int.Parse(arrHeroData[i][0]) + 1, 4), "4");  //执行升色方法传递所需兵种和升色值（稀有度1234）
                                        if (array_str[i]==null) HoldHeroData(arrHeroData, i);   //保持英雄数据
                                    }
                                }
                                else
                                {
                                    HoldHeroData(arrHeroData, i);   //保持英雄数据
                                }
                                break;
                            case "4":   //橙1
                                if (int.Parse(arrHeroData[i][2]) >= int.Parse(useepplusfun.GetRowAndColumnData(worksheet_DFC, hardNum + 1, 11))) 
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
                        switch (useepplusfun.GetRowAndColumnData(worksheet_Role, int.Parse(arrHeroData[i][0]), 5))   //判断稀有度
                        {
                            case "1":   //绿2
                                if (int.Parse(arrHeroData[i][2]) >= int.Parse(useepplusfun.GetRowAndColumnData(worksheet_DFC, hardNum + 1, 12)) && GradeOrColor())  //判断此卡牌参与的战斗周目
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
                                if (int.Parse(arrHeroData[i][2]) >= int.Parse(useepplusfun.GetRowAndColumnData(worksheet_DFC, hardNum + 1, 13)) && GradeOrColor())
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
                                if (int.Parse(arrHeroData[i][2]) >= int.Parse(useepplusfun.GetRowAndColumnData(worksheet_DFC, hardNum + 1, 14)) && GradeOrColor())
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
                                if (int.Parse(arrHeroData[i][2]) >= int.Parse(useepplusfun.GetRowAndColumnData(worksheet_DFC, hardNum + 1, 15)) && GradeOrColor())
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
        if (battles<arrayBattles[1,1])
        {
            if (heroCount<arrayBattles[0,0])
            {
                //添加英雄 
                AddHeros(array_str,enemyUnits, arrayBattles[0, 0]-heroCount);
            }
        }
        else
        {
            if (battles<arrayBattles[1,2])
            {
                if (heroCount<arrayBattles[0,1])
                {
                    AddHeros(array_str, enemyUnits, arrayBattles[0, 1] - heroCount);
                }
            }
            else
            {
                if (battles < arrayBattles[1, 3])
                {
                    if (heroCount < arrayBattles[0, 2])
                    {
                        AddHeros(array_str, enemyUnits, arrayBattles[0, 2] - heroCount);
                    }
                }
                else
                {
                    if (battles < arrayBattles[1, 4])
                    {
                        if (heroCount < arrayBattles[0, 3])
                        {
                            AddHeros(array_str, enemyUnits, arrayBattles[0, 3] - heroCount);
                        }
                    }
                    else
                    {
                        if (battles < arrayBattles[1, 5])
                        {
                            if (heroCount < arrayBattles[0, 4])
                            {
                                AddHeros(array_str, enemyUnits, arrayBattles[0, 4] - heroCount);
                            }
                        }
                        else
                        {
                            if (battles < arrayBattles[1, 6])
                            {
                                if (heroCount < arrayBattles[0, 5])
                                {
                                    AddHeros(array_str, enemyUnits, arrayBattles[0, 5] - heroCount);
                                }
                            }
                            else
                            {
                                if (battles < arrayBattles[1, 7])
                                {
                                    if (heroCount < arrayBattles[0, 6])
                                    {
                                        AddHeros(array_str, enemyUnits, arrayBattles[0, 6] - heroCount);
                                    }
                                }
                                else
                                {
                                    if (battles < arrayBattles[1, 8])
                                    {
                                        if (heroCount < arrayBattles[0, 7])
                                        {
                                            AddHeros(array_str, enemyUnits, arrayBattles[0, 7] - heroCount);
                                        }
                                    }
                                    else
                                    {
                                        if (battles < arrayBattles[1, 9])
                                        {
                                            if (heroCount < arrayBattles[0, 8])
                                            {
                                                AddHeros(array_str, enemyUnits, arrayBattles[0, 8] - heroCount);
                                            }
                                        }
                                        else
                                        {
                                            if (heroCount < arrayBattles[0, 9])
                                            {
                                                AddHeros(array_str, enemyUnits, arrayBattles[0, 9] - heroCount);
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

        return null;
    }

    /// <summary>
    /// 添加敌方英雄卡牌
    /// </summary>
    /// <param name="array_str">现有英雄</param>
    /// <param name="enemyUnits">兵种类型</param>
    /// <param name="v">添加个数</param>
    private static void AddHeros(List<string>[] array_str, int[] enemyUnits, int v)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 升色方法
    /// </summary>
    /// <param name="v1">兵种</param>
    /// <param name="v2">颜色值</param>
    /// <returns></returns>
    private static List<string> UpColor(string v1, string v2)
    {
        List<string> str_list = new List<string>();
        //随机拿到符合要求的武将数据，给品阶和战斗周目数为1
        List<int> heroids=new List<int>();
        for (int i = 2; i < worksheet_Role.rows + 1; i++) //遍历所有武将
        {
            //判断兵种和稀有度（颜色）
            if (worksheet_Role.worksheet.Cells[i, 4].Value.ToString() == v1 && worksheet_Role.worksheet.Cells[i, 5].Value.ToString() == v2)
            {
                heroids.Add(i-1);   //存储符合的英雄id
            }
        }
        int num = heroids.Count;    //符合的英雄总数
        if (num <= 0)
        {
            Debug.Log("///没有符合的升色英雄///");
            return null;
        }
        str_list = useepplusfun.GetRowDatas(worksheet_Role, heroids[UnityEngine.Random.Range(0, num)] + 1);
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
    private static List<string> UpGrade(int v1, int v2)
    {
        //传递武将数据链表尾部依次添加品阶和战斗周目数
        List<string> str_list = new List<string>();
        str_list = useepplusfun.GetRowDatas(worksheet_Role, v2 + 1);
        str_list.Add(v1.ToString());
        str_list.Add("1");
        return str_list;
    }

    private static void HoldHeroData(List<string>[] arrHeroData, int i)   //保持英雄数据
    {
        array_str[i] = useepplusfun.GetRowDatas(worksheet_Role, int.Parse(arrHeroData[i][0]) + 1);  //记录该英雄所有数据
        array_str[i].Add(arrHeroData[i][1]);    //记录他的品阶
        array_str[i].Add(arrHeroData[i][2]);    //记录他的参与战斗周目数
    }

    //判断是升阶（true）还是升色（false）
    private static bool GradeOrColor()
    {
        int num = UnityEngine.Random.Range(1, 101);
        if (num <= 30)  
            return true;
        else
            return false;
    }

}
