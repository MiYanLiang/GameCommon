using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

        //测试
        array_str_like_init();
        AddHeros_Like(enemyUnits_like, 3);
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
                AddHeros(enemyUnits, arrayBattles[0, 0] - heroCount);
            }
        }
        else
        {
            if (battles<arrayBattles[1,2])
            {
                if (heroCount<arrayBattles[0,1])
                {
                    AddHeros(enemyUnits, arrayBattles[0, 1] - heroCount);
                }
            }
            else
            {
                if (battles < arrayBattles[1, 3])
                {
                    if (heroCount < arrayBattles[0, 2])
                    {
                        AddHeros(enemyUnits, arrayBattles[0, 2] - heroCount);
                    }
                }
                else
                {
                    if (battles < arrayBattles[1, 4])
                    {
                        if (heroCount < arrayBattles[0, 3])
                        {
                            AddHeros(enemyUnits, arrayBattles[0, 3] - heroCount);
                        }
                    }
                    else
                    {
                        if (battles < arrayBattles[1, 5])
                        {
                            if (heroCount < arrayBattles[0, 4])
                            {
                                AddHeros(enemyUnits, arrayBattles[0, 4] - heroCount);
                            }
                        }
                        else
                        {
                            if (battles < arrayBattles[1, 6])
                            {
                                if (heroCount < arrayBattles[0, 5])
                                {
                                    AddHeros(enemyUnits, arrayBattles[0, 5] - heroCount);
                                }
                            }
                            else
                            {
                                if (battles < arrayBattles[1, 7])
                                {
                                    if (heroCount < arrayBattles[0, 6])
                                    {
                                        AddHeros(enemyUnits, arrayBattles[0, 6] - heroCount);
                                    }
                                }
                                else
                                {
                                    if (battles < arrayBattles[1, 8])
                                    {
                                        if (heroCount < arrayBattles[0, 7])
                                        {
                                            AddHeros(enemyUnits, arrayBattles[0, 7] - heroCount);
                                        }
                                    }
                                    else
                                    {
                                        if (battles < arrayBattles[1, 9])
                                        {
                                            if (heroCount < arrayBattles[0, 8])
                                            {
                                                AddHeros(enemyUnits, arrayBattles[0, 8] - heroCount);
                                            }
                                        }
                                        else
                                        {
                                            if (heroCount < arrayBattles[0, 9])
                                            {
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
        return null;
    }

    /// <summary>
    /// 添加敌方英雄卡牌
    /// </summary>
    /// <param name="enemyUnits">兵种类型</param>
    /// <param name="v">添加个数</param>
    private static void AddHeros(int[] enemyUnits, int v)
    {
       
    }
    //用来测试
    static int[] enemyUnits_like = {1,4,6 };
    static List<string>[] array_str_like = new List<string>[9];
    static List<string> arrDate = new List<string>() { "1", "谛听", "9", "1", "1" };
    static List<string> arrDate1 = new List<string>() { "2", "狴犴", "9", "1", "1" };  //第四个数据是兵种
    //存放相关种类可以使用的士兵id
    static List<int> soldiersKind1 = new List<int>();
    static List<int> soldiersKind2 = new List<int>();
    static List<int> soldiersKind3 = new List<int>();
    //记录三种兵种的个数
    static int soldiersKindNum1;
    static int soldiersKindNum2;
    static int soldiersKindNum3;
    //可随机的英雄id
    static List<int> canGetHeroId = new List<int>();
    //存放最后拿到的英雄Id
    static List<int> getHeroId = new List<int>();

    void array_str_like_init()
    {
        array_str_like[4] = arrDate;
        array_str_like[8] = arrDate1;
    }
    private static void AddHeros_Like(int[] enemyUnits, int v)
    {
        //将可以添加的英雄id存储
        for (int j = 2; j < 88 + 1; j++)
        {
            if (int.Parse(worksheet_Role.worksheet.Cells[j, 5].Value.ToString()) == 1)
            {
                if (int.Parse(worksheet_Role.worksheet.Cells[j, 4].Value.ToString()) == enemyUnits[0])
                {
                    soldiersKind1.Add(int.Parse(worksheet_Role.worksheet.Cells[j, 1].Value.ToString()));
                }
                else if (int.Parse(worksheet_Role.worksheet.Cells[j, 4].Value.ToString()) == enemyUnits[1])
                {
                    soldiersKind2.Add(int.Parse(worksheet_Role.worksheet.Cells[j, 1].Value.ToString()));
                }
                else if (int.Parse(worksheet_Role.worksheet.Cells[j, 4].Value.ToString()) == enemyUnits[2])
                {
                    soldiersKind3.Add(int.Parse(worksheet_Role.worksheet.Cells[j, 1].Value.ToString()));
                }
            }
        }
        //拿到已有英雄所属兵种与三种一样的个数,及去重
        for (int i = 0; i < array_str_like.Length; i++)
        {
            if (array_str_like[i] != null)
            {
                if (int.Parse(array_str_like[i][3]) == enemyUnits[0])
                {
                    soldiersKindNum1++;
                    for (int j = 0; j < soldiersKind1.Count; j++)
                    {
                        if (soldiersKind1[j] == int.Parse(array_str_like[i][0]))
                        {
                            soldiersKind1.RemoveAt(j);
                        }
                    }
                }
                else if (int.Parse(array_str_like[i][3]) == enemyUnits[1])
                {
                    soldiersKindNum2++;
                    for (int j = 0; j < soldiersKind2.Count; j++)
                    {
                        if (soldiersKind2[j] == int.Parse(array_str_like[i][0]))
                        {
                            soldiersKind2.RemoveAt(j);
                        }
                    }
                }
                else if (int.Parse(array_str_like[i][3]) == enemyUnits[2])
                {
                    soldiersKindNum1++;
                    for (int j = 0; j < soldiersKind3.Count; j++)
                    {
                        if (soldiersKind3[j] == int.Parse(array_str_like[i][0]))
                        {
                            soldiersKind3.RemoveAt(j);
                        }
                    }
                }
            }
        }
        //将可以随机的英雄id存放
        for (int i = 0; i < soldiersKind1.Count; i++)
        {
            canGetHeroId.Add(soldiersKind1[i]);
        }
        for (int i = 0; i < soldiersKind2.Count; i++)
        {
            canGetHeroId.Add(soldiersKind2[i]);
        }
        for (int i = 0; i < soldiersKind3.Count; i++)
        {
            canGetHeroId.Add(soldiersKind3[i]);
        }
        //要几个随机几个英雄Id
        for (int i = 0; i < v; i++)
        {
            int temp_num = Random.Range(0, canGetHeroId.Count);
            if (int.Parse(worksheet_Role.worksheet.Cells[canGetHeroId[temp_num] + 1, 4].Value.ToString()) == enemyUnits[0])
            {
                soldiersKindNum1++;
                if (soldiersKindNum1 < 3)
                {
                    if (!getHeroId.Contains(canGetHeroId[temp_num]))
                    {
                        getHeroId.Add(canGetHeroId[temp_num]);
                    }
                    else
                    {
                        i--;
                        soldiersKindNum1--;
                    }
                }
                else
                {
                    i--;
                    soldiersKindNum1--;
                }
            }
            if (int.Parse(worksheet_Role.worksheet.Cells[canGetHeroId[temp_num] + 1, 4].Value.ToString()) == enemyUnits[1])
            {
                soldiersKindNum2++;
                if (soldiersKindNum2 < 3)
                {
                    if (!getHeroId.Contains(canGetHeroId[temp_num]))
                    {
                        getHeroId.Add(canGetHeroId[temp_num]);
                    }
                    else
                    {
                        i--;
                        soldiersKindNum2--;
                    }
                }
                else
                {
                    i--;
                    soldiersKindNum2--;
                }
            }
            if (int.Parse(worksheet_Role.worksheet.Cells[canGetHeroId[temp_num] + 1, 4].Value.ToString()) == enemyUnits[2])
            {
                soldiersKindNum3++;
                if (soldiersKindNum3 < 3)
                {
                    if (!getHeroId.Contains(canGetHeroId[temp_num]))
                    {
                        getHeroId.Add(canGetHeroId[temp_num]);
                    }
                    else
                    {
                        i--;
                        soldiersKindNum3--;
                    }
                }
                else
                {
                    i--;
                    soldiersKindNum3--;
                }
            }
        }
        print("NUM:"+getHeroId.Count);
        //通过获取到的英雄id将整行数据存放
        for (int i = 0; i < getHeroId.Count; i++)
        {
            List<string> herodate = new List<string>();
            //herodate = useepplusfun.GetRowDatas(worksheet_Role, getHeroId[i] + 1);
            for (int j = 1; j < 15 + 1; j++)
            {
                herodate.Add(worksheet_Role.worksheet.Cells[getHeroId[i]+1,j].Value.ToString());
            }
            for (int j = 0; j < array_str_like.Length; j++)
            {
                if (array_str_like[j] == null)
                {
                    array_str_like[j] = herodate;
                    break;
                }
            }
        }

        //输出测试
        for (int i = 0; i < array_str_like.Length; i++)
        {
            if (array_str_like[i] != null)
            {
                print("i:" + i + ".." + "array_str_like[i][0]:" + array_str_like[i][0]);
            }
        }
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
