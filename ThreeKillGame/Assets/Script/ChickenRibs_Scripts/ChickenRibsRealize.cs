using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using OfficeOpenXml;    //引入使用EPPlus类库

public class ChickenRibsRealize : MonoBehaviour
{
    // Start is called before the first frame update
    int peopleHeart;       //民心
    int morale;           //士气
    int money;            //金钱
    List<int> weightId_All = new List<int>();
    List<int> weight_All = new List<int>();
    List<int> type_money = new List<int>();
    List<int> type_morale = new List<int>();
    List<int> type_peopleHeart = new List<int>();
    List<int> type_hero = new List<int>();
    List<int> type_null = new List<int>();
    List<int> equip_heroId = new List<int>();//拿到装备对应的所有Id
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //读表
    void GetExcelFile1()
    {
        //string filePath = "F:/dev/GameCommon/111.xlsx";   //绝对路径
        string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        FileInfo fileinfo = new FileInfo(filePath);
        using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))   //using用来强行做资源释放
        {
            ExcelWorksheet worksheet1 = excelpackge.Workbook.Worksheets[1];
            ExcelWorksheet worksheet2 = excelpackge.Workbook.Worksheets[2];
            ExcelWorksheet worksheet3 = excelpackge.Workbook.Worksheets["RoleTable"];
        }
    }
    //随机概率
    int rand(int[] rate, int total)
    {
        int r = Random.Range(1, total + 1);
        int t = 0;
        for (int i = 0; i < rate.Length; i++)
        {
            t += rate[i];
            if (r < t)
            {
                return i;
            }
        }
        return 0;
    }
}
