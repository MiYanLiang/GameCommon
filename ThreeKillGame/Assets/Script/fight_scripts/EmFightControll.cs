using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//敌方武将卡牌数据初始化和控制
public class EmFightControll : MonoBehaviour
{
    private int hardNum;    //难度值

    static UseEPPlusFun useepplusfun = new UseEPPlusFun();
    static TableDatas worksheet_Role;  //存储武将表
    static TableDatas worksheet_DFC;   //存储难度选择表

    //存放相关种类可以使用的士兵id
    List<int> soldiersKind1 = new List<int>();
    List<int> soldiersKind2 = new List<int>();
    List<int> soldiersKind3 = new List<int>();
    //记录三种兵种的个数
    int soldiersKindNum1;
    int soldiersKindNum2;
    int soldiersKindNum3;
    //可随机的英雄id
    List<int> canGetHeroId = new List<int>();
    //存放最后拿到的英雄Id
    List<int> getHeroId = new List<int>();


    private void Awake()
    {
        worksheet_Role = useepplusfun.FindExcelFiles("RoleTable1");
        worksheet_DFC = useepplusfun.FindExcelFiles("DifficultyChoose");
        hardNum = PlayerPrefs.GetInt("DifficultyType");  //难度值获取
    }

    //上阵位置和开启周目                   [0,0]                                 [1,1]     
    int[,] arrayBattles = new int[2, 10] { { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, { 1, 3, 4, 5, 6, 7, 11, 16, 21, 25 } };

    List<string>[] array_str = new List<string>[9]; //存储最终传递的英雄数据

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
                        switch (useepplusfun.GetRowAndColumnData(worksheet_Role, int.Parse(arrHeroData[i][0]), 5))   //判断稀有度
                        {
                            case "1":   //绿1
                                if (int.Parse(arrHeroData[i][2]) >= int.Parse(useepplusfun.GetRowAndColumnData(worksheet_DFC, hardNum + 1, 8)))  //判断此卡牌参与的战斗周目
                                {
                                    if (GradeOrColor()) //升阶
                                    {
                                        array_str[i] = UpGrade(2, int.Parse(arrHeroData[i][0]) + 1);  //执行升阶方法传递所需阶值和英雄id
                                    }
                                    else                //升色
                                    {
                                        array_str[i] = UpColor(useepplusfun.GetRowAndColumnData(worksheet_Role, int.Parse(arrHeroData[i][0]) + 1, 4), "2");  //执行升色方法传递所需兵种和升色值（稀有度1234）
                                        if (array_str[i] == null) HoldHeroData(arrHeroData, i);   //保持英雄数据
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
                                        if (array_str[i] == null) HoldHeroData(arrHeroData, i);   //保持英雄数据
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
                                        if (array_str[i] == null) HoldHeroData(arrHeroData, i);   //保持英雄数据
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
        if (battles < arrayBattles[1, 1])
        {
            if (heroCount < arrayBattles[0, 0])
            {
                //添加英雄 
                ClearDate();
                AddHeros(enemyUnits, arrayBattles[0, 0] - heroCount);
            }
        }
        else
        {
            if (battles < arrayBattles[1, 2])
            {
                if (heroCount < arrayBattles[0, 1])
                {
                    ClearDate(); 
                    AddHeros(enemyUnits, arrayBattles[0, 1] - heroCount);
                }
            }
            else
            {
                if (battles < arrayBattles[1, 3])
                {
                    if (heroCount < arrayBattles[0, 2])
                    {
                        ClearDate();
                        AddHeros(enemyUnits, arrayBattles[0, 2] - heroCount);
                    }
                }
                else
                {
                    if (battles < arrayBattles[1, 4])
                    {
                        if (heroCount < arrayBattles[0, 3])
                        {
                            ClearDate();
                            AddHeros(enemyUnits, arrayBattles[0, 3] - heroCount);
                        }
                    }
                    else
                    {
                        if (battles < arrayBattles[1, 5])
                        {
                            if (heroCount < arrayBattles[0, 4])
                            {
                                ClearDate();
                                AddHeros(enemyUnits, arrayBattles[0, 4] - heroCount);
                            }
                        }
                        else
                        {
                            if (battles < arrayBattles[1, 6])
                            {
                                if (heroCount < arrayBattles[0, 5])
                                {
                                    ClearDate();
                                    AddHeros(enemyUnits, arrayBattles[0, 5] - heroCount);
                                }
                            }
                            else
                            {
                                if (battles < arrayBattles[1, 7])
                                {
                                    if (heroCount < arrayBattles[0, 6])
                                    {
                                        ClearDate();
                                        AddHeros(enemyUnits, arrayBattles[0, 6] - heroCount);
                                    }
                                }
                                else
                                {
                                    if (battles < arrayBattles[1, 8])
                                    {
                                        if (heroCount < arrayBattles[0, 7])
                                        {
                                            ClearDate();
                                            AddHeros(enemyUnits, arrayBattles[0, 7] - heroCount);
                                        }
                                    }
                                    else
                                    {
                                        if (battles < arrayBattles[1, 9])
                                        {
                                            if (heroCount < arrayBattles[0, 8])
                                            {
                                                ClearDate();
                                                AddHeros(enemyUnits, arrayBattles[0, 8] - heroCount);
                                            }
                                        }
                                        else
                                        {
                                            if (heroCount < arrayBattles[0, 9])
                                            {
                                                ClearDate();
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
        //List<string>[] array_strr = new List<string>[9];
        //array_strr = array_str;
        //for (int pp = 0; pp < 9; pp++)
        //{
        //    if (array_str[pp]!=null)
        //    {
        //        array_str[pp].Clear();
        //    }
        //}
        return array_str;
    }

    /// <summary>
    /// 添加敌方英雄卡牌
    /// </summary>
    /// <param name="enemyUnits">兵种类型</param>
    /// <param name="v">添加个数</param>
    //调用AddHeros方法时，所需调用清空方法
    void ClearDate()
    {
        soldiersKind1.Clear();
        soldiersKind2.Clear();
        soldiersKind3.Clear();
        soldiersKindNum1 = 0;
        soldiersKindNum2 = 0;
        soldiersKindNum3 = 0;
        canGetHeroId.Clear();
        getHeroId.Clear();
    }
    private void AddHeros(int[] enemyUnits, int v)
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
        for (int i = 0; i < array_str.Length; i++)
        {
            if (array_str[i] != null)
            {
                if (int.Parse(array_str[i][3]) == enemyUnits[0])
                {
                    soldiersKindNum1++;
                    for (int j = 0; j < soldiersKind1.Count; j++)
                    {
                        if (soldiersKind1[j] == int.Parse(array_str[i][0]))
                        {
                            soldiersKind1.RemoveAt(j);
                        }
                    }
                }
                else if (int.Parse(array_str[i][3]) == enemyUnits[1])
                {
                    soldiersKindNum2++;
                    for (int j = 0; j < soldiersKind2.Count; j++)
                    {
                        if (soldiersKind2[j] == int.Parse(array_str[i][0]))
                        {
                            soldiersKind2.RemoveAt(j);
                        }
                    }
                }
                else if (int.Parse(array_str[i][3]) == enemyUnits[2])
                {
                    soldiersKindNum1++;
                    for (int j = 0; j < soldiersKind3.Count; j++)
                    {
                        if (soldiersKind3[j] == int.Parse(array_str[i][0]))
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
        //通过获取到的英雄id将整行数据存放
        for (int i = 0; i < getHeroId.Count; i++)
        {
            List<string> herodate = new List<string>();
            for (int j = 1; j < 15 + 1; j++)
            {
                herodate.Add(worksheet_Role.worksheet.Cells[getHeroId[i] + 1, j].Value.ToString());
            }
            for (int j = 0; j < array_str.Length; j++)
            {
                if (array_str[j] == null)
                {
                    array_str[j] = herodate;
                    break;
                }
            }
        }
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
    private List<string> UpGrade(int v1, int v2)
    {
        //传递武将数据链表尾部依次添加品阶和战斗周目数
        List<string> str_list = new List<string>();
        str_list = useepplusfun.GetRowDatas(worksheet_Role, v2 + 1);
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
        array_str[i] = useepplusfun.GetRowDatas(worksheet_Role, int.Parse(arrHeroData[i][0]) + 1);  //记录该英雄所有数据
        array_str[i].Add(arrHeroData[i][1]);    //记录他的品阶
        array_str[i].Add(arrHeroData[i][2]);    //记录他的参与战斗周目数
    }

    //判断是升阶（true）还是升色（false）
    private bool GradeOrColor()
    {
        int num = UnityEngine.Random.Range(1, 101);
        if (num <= 30)  
            return true;
        else
            return false;
    }

}
