﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using OfficeOpenXml;    //引入使用EPPlus类库


public class TableDatas     //数据表类
{
    public ExcelWorksheet worksheet;   //存储表的数据

    public int rows, columns;  //存储表的总行数和列数
}

public class UseEPPlusFun : MonoBehaviour {

	private void Start ()
    {
        //RoleTable

        //TableDatas tableData = FindExcelFiles("RoleTable");
        //int i = 1;
        //while (tableData.worksheet.Cells[2, i].Value != null)
        //{ i++; }
        //Debug.Log("Excel测试:" + i);
        
    }

    /// <summary>
    /// 根据表名获取到表数据存放到TableDatas类中
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <returns></returns>
    public TableDatas FindExcelFiles(string tableName)
    {
        string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //StreamingAssets文件夹的相对路径
        FileInfo fileinfo = new FileInfo(filePath);
        Debug.Log("打开文件路径："+fileinfo);
        ExcelPackage excelPackage = new ExcelPackage(fileinfo);
        TableDatas tableData = new TableDatas();
        tableData.worksheet = excelPackage.Workbook.Worksheets[tableName];

        if (tableData.worksheet == null || excelPackage == null)
        {
            Debug.Log(tableName + "表数据或许为null");
            return null;
        }
        else
        {
            Debug.Log("获取 " + tableName + " 表数据 " + excelPackage);
            int num = 1;
            //计算表的总行数和列数
            while (tableData.worksheet.Cells[num++, 1].Value != null){ }
            tableData.rows = num - 2;
            num = 1;
            while (tableData.worksheet.Cells[1, num++].Value != null) { }
            tableData.columns = num - 2;
            return tableData;
        }
    }

    /// <summary>
    /// 根据行列值获取表数据类中的某个数据
    /// </summary>
    /// <param name="tabledata"></param>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    public string GetRowAndColumnData(TableDatas tabledata, int row, int column)
    {
        if (row > tabledata.rows || column > tabledata.columns)
        {
            Debug.Log("索引超出表格范围！");
            return null;
        }
        else
        {
            if (tabledata.worksheet.Cells[row,column].Value==null)
            {
                Debug.Log("该表格数据为空");
                return "";
            }
            else
            {
                return tabledata.worksheet.Cells[row, column].Value.ToString();
            }
        }
    }

    /// <summary>
    /// 获取到相应行的所有数据
    /// </summary>
    /// <param name="tabledata"></param>
    /// <param name="row"></param>
    /// <returns></returns>
    public List<string> GetRowDatas(TableDatas tabledata, int row)
    {
        List<string> datas = new List<string>();
        int i = 1;
        while (i <= tabledata.columns)
        {
            datas.Add((tabledata.worksheet.Cells[row, i].Value != null) ? tabledata.worksheet.Cells[row, i].Value.ToString() : "");
        }
        return datas;
    }

    /// <summary>
    /// 获取到相应列的所有数据
    /// </summary>
    /// <param name="tabledata"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    public List<string> GetColumnDatas(TableDatas tabledata, int column)
    {
        List<string> datas = new List<string>();
        int i = 1;
        while (i <= tabledata.rows)
        {
            datas.Add((tabledata.worksheet.Cells[i, column].Value != null) ? tabledata.worksheet.Cells[i, column].Value.ToString() : "");
        }
        return datas;
    }







    /// <summary>
    /// 读取Excel表格文件
    /// </summary>
    private static void GetExcelFile()
    {
        string filePath = "F:/dev/111.xlsx";   //绝对路径
        //string filePath = Application.streamingAssetsPath + "\\TablFiles\\表格测试数据.xlsx"; //相对路径，文件从StreamingAssets文件夹中获取（file://）

        //根据文件路径，获取excel文件信息
        FileInfo fileinfo = new FileInfo(filePath);

        //通过文件信息，打开excel文件
        using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))   //using用来强行做资源释放
        {
            ExcelWorksheet worksheet = excelpackge.Workbook.Worksheets[1];  //读取excel表文件中的第一张表，注意集合从1开始

            //取得第一行第一列的数值
            string str = worksheet.Cells[1, 1].Value.ToString();
            for (int i = 1; i < worksheet.Cells.Rows + 1; i++)
            {
                Debug.Log(worksheet.Cells[i, 1].Value.ToString() + "--" + worksheet.Cells[i, 2].Value.ToString() + "--" + worksheet.Cells[i, 3].Value.ToString());
            }
            //GetSpecificValue(5, worksheet, "roleName");


        }//完成一些列操作后，关闭excel文件
    }



    /// <summary>
    /// 通过第一行的列名拿到数据
    /// </summary>
    /// <param name="name"></param>
    /// <param name="worksheet"></param>
    void GetValueFromColumn(string name,ExcelWorksheet worksheet)
    {
        for (int i = 1; i < 3 + 1; i++)
        {
            for (int j = 1; j < 4 + 1; j++)
            {
                if (worksheet.Cells[i, j].Value.ToString() == name)
                {
                    string n = worksheet.Cells[i, j].GetEnumerator().ToString();
                    
                }
            }
        }
    }

    //static int num;
    /// <summary>
    /// 通过id拿到整行数据
    /// </summary>
    /// <param name="id"></param>
    /// <param name="worksheet"></param>
    static void GetValueFromId(int id, ExcelWorksheet worksheet)
    {
        int num = 0;
        for (int i = 1; i < 3 + 1; i++)
        {
            for (int j = 1; j < 4 + 1; j++)
            {
                if (j == 1)
                {
                    if (int.Parse(worksheet.Cells[i, j].Value.ToString()) == id)
                    {
                        string n = worksheet.Cells[i, j].GetEnumerator().ToString();
                        num = int.Parse(n[1].ToString());
                    }
                }
            }
        }
        for (int y = 1; y < 4 + 1; y++)
        {
            print(worksheet.Cells[num,y].Value.ToString());
        }
    }

    /// <summary>
    /// 根据id和列名拿到具体的值
    /// </summary>
    /// <param name="id"></param>
    /// <param name="worksheet"></param>
    /// <param name="name"></param>
    static void GetSpecificValue(int id, ExcelWorksheet worksheet, string name)
    {
        int num = 0;
        string numy = "";
        for (int i = 1; i < 3 + 1; i++)
        {
            for (int j = 1; j < 4 + 1; j++)
            {
                if (j == 1)
                {
                    if (int.Parse(worksheet.Cells[i, j].Value.ToString()) == id)
                    {
                        string n = worksheet.Cells[i, j].GetEnumerator().ToString();
                        num = int.Parse(n[1].ToString());
                    }
                }
                if (i == 1)
                {
                    if (worksheet.Cells[i, j].Value.ToString() == name)
                    {
                        string n = worksheet.Cells[i, j].GetEnumerator().ToString();
                        numy =n[0].ToString();
                    }
                }
            }
        }
        for (int y = 1; y < 4 + 1; y++)
        {
            if (worksheet.Cells[num, y].GetEnumerator().ToString() == numy + num.ToString())
            {
                print(worksheet.Cells[num, y].Value.ToString());
            }
        } 
    }

    /// <summary>
    /// 创建和删除excel文件
    /// </summary>
    private static void CreatAndDeleteExcel()
    {
        string filePath = "D:/测试实例文件/ExcelFiles/TestExcel.xlsx";

        FileInfo fileinfo = new FileInfo(filePath);

        using (ExcelPackage excelpackage = new ExcelPackage(fileinfo))
        {
            //如果打开的excel存在的话，直接给其中添加表格，若不存在，先创建文件再添加表格
            ExcelWorksheet worksheet = excelpackage.Workbook.Worksheets.Add("Sheet1(表1)");
            excelpackage.Workbook.Worksheets.Add("Sheet2(表2)");
            excelpackage.Workbook.Worksheets.Add("Sheet3(表3)");
            //删除excel的一个表
            excelpackage.Workbook.Worksheets.Delete("Sheet2(表2)");

            excelpackage.Save();
        }
    }

    /// <summary>
    /// 修改excel文件数据
    /// </summary>
    private static void UpdateExcelFile()
    {
        string filePath = "D:/测试实例文件/ExcelFiles/游戏数据.xlsx";

        FileInfo fileinfo = new FileInfo(filePath);

        using (ExcelPackage excelpackage = new ExcelPackage(fileinfo))
        {
            ExcelWorksheet worksheet = excelpackage.Workbook.Worksheets[1];

            worksheet.Cells[8, 1].Value = "1007";   //往第4行，第1列写入数据
            worksheet.Cells[8, 2].Value = "拳击手套";
            worksheet.Cells[8, 3].Value = "300";

            worksheet.Cells[7, 1].Value = 1006;   //往第4行，第1列写入数据
            worksheet.Cells[7, 2].Value = "小刀";
            worksheet.Cells[7, 3].Value = 188;

            excelpackage.Save();    //保存数据
        }
    }
   
}
