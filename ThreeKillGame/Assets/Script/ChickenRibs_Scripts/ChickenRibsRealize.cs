using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using OfficeOpenXml;    //引入使用EPPlus类库

public class ChickenRibsRealize : MonoBehaviour
{
    // Start is called before the first frame update
    int peopleHeart = 0;       //民心
    int morale = 0;           //士气
    int money = 0;            //金钱
    List<int> weightId_All = new List<int>();    //可遇事件id
    List<int> weight_All = new List<int>();      //存储所有的可遇事件权重
    List<int> type_money = new List<int>();
    List<int> type_morale = new List<int>();
    List<int> type_peopleHeart = new List<int>();
    List<int> type_hero = new List<int>();
    List<int> type_null = new List<int>();
    List<int> equip_heroId = new List<int>();//拿到装备对应的所有Id
    List<int> hasHeroId = new List<int>();//拥有的英雄中有装备有关的英雄Id
    int weight_total;  //权重总和
    int index;  //当前鸡肋事件下标
    int CurrenId; //当前鸡肋事件Id
    void Start()
    {
        //GetInEventId();
        GetExcelFile();
        GetCurrentChickenRibsId();
    }

    // Update is called once per frame
    void Update()
    {

    }
    //读表
    void GetExcelFile()
    {
        //string filePath = "F:/dev/GameCommon/111.xlsx";   //绝对路径
        string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        FileInfo fileinfo = new FileInfo(filePath);
        using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))   //using用来强行做资源释放
        {
            ExcelWorksheet worksheet1 = excelpackge.Workbook.Worksheets[1];
            ExcelWorksheet worksheet2 = excelpackge.Workbook.Worksheets[2];
            ExcelWorksheet worksheet3 = excelpackge.Workbook.Worksheets[3];
            GetChickenRibsId(worksheet3);
            GetInEventId();
            for (int i = 0; i < hasHeroId.Count; i++)
            {
                GetChickenRibsIdFromHeroId(hasHeroId[i], worksheet3);
            }
            for (int i = 0; i < weightId_All.Count; i++)
            {
                GetWeightFromId(worksheet3, weightId_All[i]);
            }
            //for (int i = 0; i < weightId_All.Count; i++)
            //{
            //    print(weightId_All[i]);
            //}

        }
    }
    //通过触发条件存储相应的事件Id
    //记得更换行列数据
    void GetChickenRibsId(ExcelWorksheet worksheet)
    {
        int num = 6;     //给num赋值为触发条件的列
        for (int i = 1; i < 92 + 1; i++)
        {
            if (worksheet.Cells[i, num].Value.ToString() == "金币")
            {
                type_money.Add(int.Parse(worksheet.Cells[i, 1].Value.ToString()));
            }
            else if (worksheet.Cells[i, num].Value.ToString() == "士气")
            {
                type_morale.Add(int.Parse(worksheet.Cells[i, 1].Value.ToString()));
            }
            else if (worksheet.Cells[i, num].Value.ToString() == "民心")
            {
                type_peopleHeart.Add(int.Parse(worksheet.Cells[i, 1].Value.ToString()));
            }
            else if (worksheet.Cells[i, num].Value.ToString() == "英雄")
            {
                type_hero.Add(int.Parse(worksheet.Cells[i, 1].Value.ToString()));
                equip_heroId.Add(int.Parse(worksheet.Cells[i, 11].Value.ToString()));//x代表对应英雄id的列值
            }
            else if (worksheet.Cells[i, num].Value.ToString() == "无")
            {
                type_null.Add(int.Parse(worksheet.Cells[i, 1].Value.ToString()));
            }
        }
    }
    //通过英雄Id拿到事件Id
    void GetChickenRibsIdFromHeroId(int id, ExcelWorksheet worksheet)
    {
        int num = 11;     //给num赋值为指向的列
        for (int i = 1; i < 92 + 1; i++)
        {
            if (int.Parse(worksheet.Cells[i, num].Value.ToString()) == id)
            {
                weightId_All.Add(int.Parse(worksheet.Cells[i, 1].Value.ToString()));
            }
        }
    }
    //根据条件存储可遇事件的Id
    void GetInEventId()
    {
        for (int i = 0; i < type_null.Count; i++)
        {
            weightId_All.Add(type_null[i]);
        }
        if (peopleHeart > 80)
        {
            for (int i = 0; i < type_peopleHeart.Count; i++)
            {
                weightId_All.Add(type_peopleHeart[i]);
            }
        }
        if (morale > 80)
        {
            for (int i = 0; i < type_morale.Count; i++)
            {
                weightId_All.Add(type_morale[i]);
            }
        }
        if (money > 5)
        {
            for (int i = 0; i < type_money.Count; i++)
            {
                weightId_All.Add(type_money[i]);
            }
            //for (int i = 0; i < getCardId.Count; i++)
            //{
            //    for (int j = 0; j < equip_heroId.Count; j++)
            //    {
            //        if (getCardId[i] == equip_heroId[j])
            //        {
            //            //调用读表，通过英雄Id获取事件Id
            //            //weightId_All.Add(type_hero[i]);
            //            //GetExcelFile1(equip_heroId[j]);
            //            hasHeroId.Add(equip_heroId[j]);
            //        }
            //    }
            //}
        }
    }
    //通过拿到的事件Id，获取权重
    void GetWeightFromId(ExcelWorksheet worksheet, int incidentId)
    {
        int num = 3;   //存权重列
        for (int i = 2; i < 92 + 1; i++)
        {
            if (int.Parse(worksheet.Cells[i, 1].Value.ToString()) == incidentId)
            {
                weight_All.Add(int.Parse(worksheet.Cells[i, num].Value.ToString()));
            }
        }
    }
    //获取当前鸡肋事件id
    void GetCurrentChickenRibsId()
    {
        for (int i = 0; i < weight_All.Count; i++)
        {
            weight_total += weight_All[i];
        }
        index = rand(weight_All, weight_total);
        CurrenId = weightId_All[index];
        print("CurrenId：" + CurrenId);
    }
    //随机概率
    int rand(List<int> rate, int total)
    {
        int r = Random.Range(1, total + 1);
        int t = 0;
        for (int i = 0; i < rate.Count; i++)
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
