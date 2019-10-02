using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using OfficeOpenXml;    //引入使用EPPlus类库

public class UseEPPlusFun : MonoBehaviour {

	private void Start ()
    {
        GetExcelFile(); //读取Excel表格文件

        //UpdateExcelFile();

        //CreatAndDeleteExcel();
    }

    /// <summary>
    /// 读取Excel表格文件
    /// </summary>
    private static void GetExcelFile()
    {
        //string filePath = "D:/测试实例文件/ExcelFiles/游戏数据.xlsx";   //绝对路径
        string filePath = Application.streamingAssetsPath + "\\TablFiles\\表格测试数据.xlsx"; //相对路径，文件从StreamingAssets文件夹中获取（file://）

        //根据文件路径，获取excel文件信息
        FileInfo fileinfo = new FileInfo(filePath);

        //通过文件信息，打开excel文件
        using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))   //using用来强行做资源释放
        {
            ExcelWorksheet worksheet = excelpackge.Workbook.Worksheets[1];  //读取excel表文件中的第一张表，注意集合从1开始

            //取得第一行第一列的数值
            //string str = worksheet.Cells[1, 1].Value.ToString();

            for (int i = 1; i < worksheet.Cells.Rows + 1; i++)
            {
                Debug.Log(worksheet.Cells[i, 1].Value.ToString() + "--" + worksheet.Cells[i, 2].Value.ToString() + "--" + worksheet.Cells[i, 3].Value.ToString());
            }

        }//完成一些列操作后，关闭excel文件
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
