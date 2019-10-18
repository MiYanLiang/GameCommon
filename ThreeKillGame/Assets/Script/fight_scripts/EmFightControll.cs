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

    /// <summary>
    /// 接收敌方武将现有的上阵卡牌，进行卡牌变化和传递
    /// </summary>
    /// <param name="arrHeroData">武将数据</param>
    /// <param name="enemyUnits">阵容发展方向</param>
    /// <param name="battles">总周目数</param>
    /// <returns></returns>
    public static List<string>[] SendHeroData(List<string>[] arrHeroData, int[] enemyUnits,int battles)
    {
        List<string>[] array_str = new List<string>[9]; //存储最终传递的英雄数据

        for (int i = 0; i < arrHeroData.Length; i++)    //依次处理传过来的九个位置
        {
            if (arrHeroData[i]!=null)   //若该位置有英雄
            {
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
                                        array_str[i] = UpColor(int.Parse(useepplusfun.GetRowAndColumnData(worksheet_Role, int.Parse(arrHeroData[i][0]) + 1, 4)), 2);  //执行升色方法传递所需兵种和升色值（稀有度1234）
                                    }
                                }
                                else
                                {
                                    array_str[i] = useepplusfun.GetRowDatas(worksheet_Role, int.Parse(arrHeroData[i][0]) + 1);  //记录该英雄所有数据
                                    array_str[i].Add(arrHeroData[i][1]);    //记录他的品阶
                                    array_str[i].Add(arrHeroData[i][2]);    //记录他的参与战斗周目数
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
                                        array_str[i] = UpColor(int.Parse(useepplusfun.GetRowAndColumnData(worksheet_Role, int.Parse(arrHeroData[i][0]) + 1, 4)), 3);  //执行升色方法传递所需兵种和升色值（稀有度1234）
                                    }
                                }
                                else
                                {
                                    array_str[i] = useepplusfun.GetRowDatas(worksheet_Role, int.Parse(arrHeroData[i][0]) + 1);  //记录该英雄所有数据
                                    array_str[i].Add(arrHeroData[i][1]);    //记录他的品阶
                                    array_str[i].Add(arrHeroData[i][2]);    //记录他的参与战斗周目数
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
                                        array_str[i] = UpColor(int.Parse(useepplusfun.GetRowAndColumnData(worksheet_Role, int.Parse(arrHeroData[i][0]) + 1, 4)), 4);  //执行升色方法传递所需兵种和升色值（稀有度1234）
                                    }
                                }
                                else
                                {
                                    array_str[i] = useepplusfun.GetRowDatas(worksheet_Role, int.Parse(arrHeroData[i][0]) + 1);  //记录该英雄所有数据
                                    array_str[i].Add(arrHeroData[i][1]);    //记录他的品阶
                                    array_str[i].Add(arrHeroData[i][2]);    //记录他的参与战斗周目数
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
                                    array_str[i] = useepplusfun.GetRowDatas(worksheet_Role, int.Parse(arrHeroData[i][0]) + 1);  //记录该英雄所有数据
                                    array_str[i].Add(arrHeroData[i][1]);    //记录他的品阶
                                    array_str[i].Add(arrHeroData[i][2]);    //记录他的参与战斗周目数
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
                                    array_str[i] = useepplusfun.GetRowDatas(worksheet_Role, int.Parse(arrHeroData[i][0]) + 1);  //记录该英雄所有数据
                                    array_str[i].Add(arrHeroData[i][1]);    //记录他的品阶
                                    array_str[i].Add(arrHeroData[i][2]);    //记录他的参与战斗周目数
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
                                    array_str[i] = useepplusfun.GetRowDatas(worksheet_Role, int.Parse(arrHeroData[i][0]) + 1);  //记录该英雄所有数据
                                    array_str[i].Add(arrHeroData[i][1]);    //记录他的品阶
                                    array_str[i].Add(arrHeroData[i][2]);    //记录他的参与战斗周目数
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
                                    array_str[i] = useepplusfun.GetRowDatas(worksheet_Role, int.Parse(arrHeroData[i][0]) + 1);  //记录该英雄所有数据
                                    array_str[i].Add(arrHeroData[i][1]);    //记录他的品阶
                                    array_str[i].Add(arrHeroData[i][2]);    //记录他的参与战斗周目数
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
                                    array_str[i] = useepplusfun.GetRowDatas(worksheet_Role, int.Parse(arrHeroData[i][0]) + 1);  //记录该英雄所有数据
                                    array_str[i].Add(arrHeroData[i][1]);    //记录他的品阶
                                    array_str[i].Add(arrHeroData[i][2]);    //记录他的参与战斗周目数
                                }
                                break;
                        }
                        break;
                    case "3":   //三阶英雄保持数据
                        array_str[i] = useepplusfun.GetRowDatas(worksheet_Role, int.Parse(arrHeroData[i][0]) + 1);  //记录该英雄所有数据
                        array_str[i].Add(arrHeroData[i][1]);    //记录他的品阶
                        array_str[i].Add(arrHeroData[i][2]);    //记录他的参与战斗周目数
                        break;
                }
            }
        }



        return null;
    }

    /// <summary>
    /// 升色方法
    /// </summary>
    /// <param name="v1">兵种</param>
    /// <param name="v2">颜色值</param>
    /// <returns></returns>
    private static List<string> UpColor(int v1, int v2)
    {
        //随机拿到符合要求的武将数据，给品阶和战斗周目数为1
        throw new NotImplementedException();
    }

    /// <summary>
    /// 升阶方法
    /// </summary>
    /// <param name="v1">所升阶数</param>
    /// <param name="v2">英雄id</param>
    /// <returns></returns>
    private static List<string> UpGrade(int v1, int v2)
    {
        throw new NotImplementedException();
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
