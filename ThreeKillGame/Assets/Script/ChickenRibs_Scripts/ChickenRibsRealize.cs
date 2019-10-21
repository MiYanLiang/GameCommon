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
    int hp;               //血量
    int attack;           //攻击
    int defense;          //防御
    int soldiersNum;      //士兵数量
    List<int> weightId_All = new List<int>();    //可遇事件id
    List<int> weight_All = new List<int>();      //存储所有的可遇事件权重
    List<int> type_money = new List<int>();
    List<int> type_morale = new List<int>();
    List<int> type_peopleHeart = new List<int>();
    List<int> type_hero = new List<int>();
    List<int> type_null = new List<int>();
    List<int> equip_heroId = new List<int>();//拿到装备对应的所有Id
    List<int> hasHeroId = new List<int>();//拥有的英雄中有装备有关的英雄Id
    List<string> currenEventDate = new List<string>();
    List<string> eventDate = new List<string>();
    int weight_total;  //权重总和
    int index;  //当前鸡肋事件下标
    int CurrenId; //当前鸡肋事件Id
    [HideInInspector]
    public int eventType; //事件显示类型
    public CreateAndUpdate cau;
    public GameObject type1;
    public GameObject type2;
    string pointId;//指向Id列的值
    string pointId_one;
    string pointId_A;
    string pointId_B;
    string currenPointId;//当前指向id

    List<string> buyHeroDate = new List<string>();     //购买英雄后存入
    List<string> buyEquipmentDate = new List<string>();//购买武器后存入

    public GameObject textSee;
    int days;

    void Start()
    {
        days = 1;
        hasHeroId = cau.ChickenRibsHeroId;  ///将CreateAndUpdate中的玩家拥有的所有英雄id传过来
        money = cau.money;
        peopleHeart = cau.peopleHearts;
        morale = cau.moraleNum;
        GetExcelFile();
        GetCurrentChickenRibsId();
        init();
        //print(buyEquipmentDate[1]);
        //textSee.GetComponent<Text>().text = eventDate[1];
    }
    //点击跳过
    public void Jump()
    {
        print(hasHeroId.Count);
        days++;
        if (days < 6)
        {
            weight_total = 0;
            weightId_All.Clear();
            weight_All.Clear();
            type_money.Clear();
            type_morale.Clear();
            type_peopleHeart.Clear();
            type_hero.Clear();
            type_null.Clear();
            equip_heroId.Clear();
            hasHeroId.Clear();
            hasHeroId = cau.ChickenRibsHeroId;  ///将CreateAndUpdate中的玩家拥有的所有英雄id传过来
            money = cau.money;
            peopleHeart = cau.peopleHearts;
            morale = cau.moraleNum;
            GetExcelFile();
            GetCurrentChickenRibsId();
            init();
        }
        else
        {
            print("您吃饱了");
        }

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
                //print("hasHeroID:" + hasHeroId[i]);
            }
            for (int i = 0; i < weightId_All.Count; i++)
            {
                GetWeightFromId(worksheet3, weightId_All[i]);
            }

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
            if (worksheet.Cells[i, num].Value.ToString() == id.ToString())
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
        print("index:" + index);
        CurrenId = weightId_All[index];
        print("CurrenId：" + CurrenId);
    }
    //随机概率
    int rand(List<int> rate, int total)
    {
        print(rate + ".." + total);
        int r = Random.Range(1, total + 1);
        int t = 0;
        for (int i = 0; i < rate.Count; i++)
        {
            t += rate[i];
            if (r < t)
            {
                print("i:" + i);
                return i;
            }
        }
        return 0;
    }
    /////////////////////////////////////上面是拿到每一次的鸡肋事件id
    /////////////////////////////////////下面对事件进行判断及操作
    void init()
    {
        currenEventDate.Clear();
        GetExcelFile1();
        print("eventType:" + eventType);
        ShowType();
        GetExcelFile2();
        RealizeFunctionFromType();
        textSee.GetComponent<Text>().text = currenEventDate[1];
    }
    void GetExcelFile1()
    {
        //string filePath = "F:/dev/GameCommon/111.xlsx";   //绝对路径
        string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        FileInfo fileinfo = new FileInfo(filePath);
        using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))   //using用来强行做资源释放
        {
            ExcelWorksheet worksheet1 = excelpackge.Workbook.Worksheets[1];
            ExcelWorksheet worksheet2 = excelpackge.Workbook.Worksheets[2];
            ExcelWorksheet worksheet3 = excelpackge.Workbook.Worksheets[3];
            GetTypeFromCurrenId(worksheet3, CurrenId);
            GetAllDateFromCurrenId(worksheet3,CurrenId);
        }
    }
    //根据CurrenId当前鸡肋事件，拿到事件类型
    void GetTypeFromCurrenId(ExcelWorksheet worksheet, int CurrenId)
    {
        int num = 8;   //交互类型
        for (int i = 2; i < 92 + 1; i++)
        {
            if (int.Parse(worksheet.Cells[i, 1].Value.ToString()) == CurrenId)
            {
                if (worksheet.Cells[i, num].Value.ToString() == "是否购买（购买/跳过）")
                {
                    eventType = 2;
                }
                if (worksheet.Cells[i, num].Value.ToString() == "（跳过）")
                {
                    eventType = 3;
                }
                if (worksheet.Cells[i, num].Value.ToString() == "A/B/跳过")
                {
                    eventType = 1;
                }
            }
        }
    }
    //通过当前事件id拿到当前事件所有数据
    void GetAllDateFromCurrenId(ExcelWorksheet worksheet, int CurrenId)
    {
        for (int i = 2; i < 92 + 1; i++)
        {
            if (int.Parse(worksheet.Cells[i, 1].Value.ToString()) == CurrenId)
            {
                for (int j = 1; j < 3 + 1; j++)
                {
                    currenEventDate.Add(worksheet.Cells[i, j].Value.ToString());
                }
            }
        }
    }
    //根据类型，在前端显示type
    void ShowType()
    {
        if (eventType == 1)  //A/B类
        {
            type1.SetActive(true);
            type2.SetActive(false);
        }
        else if (eventType == 2)  //购买类
        {
            type1.SetActive(false);
            type2.SetActive(true);
        }
        else if (eventType == 3)  //跳过类
        {
            type1.SetActive(false);
            type2.SetActive(false);
        }
    }
    //通过类型，实现功能
    void RealizeFunctionFromType()
    {
        if (eventType == 1)
        {
            string[] a = pointId.Split('/');
            pointId_A = a[0];
            pointId_B = a[1];
            //print("pointId_A:"+ pointId_A);
            //print("pointId_B:"+ pointId_B);
        }
        if (eventType == 2)
        {
            pointId_one = pointId;
        }
        if (eventType == 3)
        {
            pointId_one = pointId;
        }
    }
    //读表获取指向Id
    void GetExcelFile2()
    {
        //string filePath = "F:/dev/GameCommon/111.xlsx";   //绝对路径
        string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        FileInfo fileinfo = new FileInfo(filePath);
        using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))   //using用来强行做资源释放
        {
            ExcelWorksheet worksheet1 = excelpackge.Workbook.Worksheets[1];
            ExcelWorksheet worksheet2 = excelpackge.Workbook.Worksheets[2];
            ExcelWorksheet worksheet3 = excelpackge.Workbook.Worksheets[3];
            GetPointidFronEventid(CurrenId, worksheet3);
        }
    }

    //通过当前事件id获取指向id列值
    void GetPointidFronEventid(int eventid, ExcelWorksheet worksheet)
    {
        int num = 11;   //指向Id
        for (int i = 2; i < 92 + 1; i++)
        {
            if (int.Parse(worksheet.Cells[i, 1].Value.ToString()) == eventid)
            {
                pointId = worksheet.Cells[i, num].Value.ToString();
            }
        }
    }
    //////////////////////对各类事件进行操作
    //点击了A
    public void ClickA()
    {
        eventDate.Clear();
        currenPointId = pointId_A;
        print("currenPointId:" + currenPointId);
        GetExcelFile3();
        Jump();
    }
    //点击了B
    public void ClickB()
    {
        eventDate.Clear();
        currenPointId = pointId_B;
        print("currenPointId:" + currenPointId);
        GetExcelFile3();
        Jump();
    }
    //点击了购买
    public void ClickBuy()
    {
        if (CurrenId > 100 && CurrenId < 116)                     //购买英雄
        {
            eventDate.Clear();
            currenPointId = pointId_one;
            print("currenPointId:" + currenPointId);
            GetExcelFile3();
            if (money > int.Parse(eventDate[5]))
            {
                money -= int.Parse(eventDate[5]);
                cau.money = money;
                for (int i = 0; i < eventDate.Count; i++)
                {
                    buyHeroDate.Add(eventDate[i]);
                }
                hasHeroId.Add(int.Parse(eventDate[0]));
                cau.ChickenRibsHeroId = hasHeroId;
                Jump();
            }
            else
            {
                print("啊，呸，穷鬼");
            }
        }
        if (CurrenId > 215 && CurrenId < 299)                      //购买装备
        {
            eventDate.Clear();
            currenPointId = pointId_one;
            print("currenPointId:" + currenPointId);
            GetExcelFile3();
            if (money > int.Parse(eventDate[2]))
            {
                money -= int.Parse(eventDate[2]);
                cau.money = money;
                for (int i = 0; i < eventDate.Count; i++)
                {
                    buyEquipmentDate.Add(eventDate[i]);
                }
                Jump();
            }
            else
            {
                print("啊，呸，穷鬼");
            }
        }
    }
    //点击了跳过
    public void ClickJump()
    {
        if (CurrenId > 600 && CurrenId < 611)
        {
            eventDate.Clear();
            currenPointId = CurrenId.ToString();
            print("currenPointId:" + currenPointId);
            GetExcelFile3();
        }
        if (CurrenId == 1 || CurrenId == 2)                      //典故
        {
            eventDate.Clear();
            currenPointId = CurrenId.ToString();
            print("currenPointId:" + currenPointId);
            GetExcelFile3();
        }
    }
    void GetExcelFile3()
    {
        //string filePath = "F:/dev/GameCommon/111.xlsx";   //绝对路径
        string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        FileInfo fileinfo = new FileInfo(filePath);
        using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))   //using用来强行做资源释放
        {
            ExcelWorksheet worksheet1 = excelpackge.Workbook.Worksheets[1];
            ExcelWorksheet worksheet2 = excelpackge.Workbook.Worksheets[2];
            ExcelWorksheet worksheet3 = excelpackge.Workbook.Worksheets[3];
            ExcelWorksheet worksheet4 = excelpackge.Workbook.Worksheets[4];
            ExcelWorksheet worksheet5 = excelpackge.Workbook.Worksheets[5];
            ExcelWorksheet worksheet6 = excelpackge.Workbook.Worksheets[6];
            ExcelWorksheet worksheet7 = excelpackge.Workbook.Worksheets[7];
            ExcelWorksheet worksheet8 = excelpackge.Workbook.Worksheets[8];
            if (int.Parse(currenPointId[0].ToString()) == 6)
            {
                AllFromEventId_4(worksheet4, currenPointId);
                money += int.Parse(eventDate[2]);
                peopleHeart += int.Parse(eventDate[3]);
                morale += int.Parse(eventDate[4]);
                hp += int.Parse(eventDate[5]);
                cau.money = money;
                cau.peopleHearts = peopleHeart;
                cau.moraleNum = money;
                cau.hp = hp;
            }
            if (int.Parse(currenPointId[0].ToString()) == 7)
            {
                AllFromEventId_5(worksheet5, currenPointId);
                attack += int.Parse(eventDate[2]);
                defense += int.Parse(eventDate[3]);
                soldiersNum += int.Parse(eventDate[4]);
            }
            if (CurrenId > 100 && CurrenId < 116)                     //购买英雄
            {
                AllFromEventId_1(worksheet1, currenPointId);
            }
            if (CurrenId > 215 && CurrenId < 299)                      //购买装备
            {
                AllFromEventId_6(worksheet6, currenPointId);
            }
            if (CurrenId == 1 || CurrenId == 2)                      //典故
            {
                AllFromEventId_8(worksheet8, currenPointId);
                attack += int.Parse(eventDate[2]);
                defense += int.Parse(eventDate[3]);
                soldiersNum += int.Parse(eventDate[4]);
            }
        }
    }
    //根据指向id读取整行数据
    void AllFromEventId_4(ExcelWorksheet worksheet, string eventId)
    {
        for (int i = 2; i < 51 + 1; i++)
        {
            if (worksheet.Cells[i, 1].Value.ToString().ToString() == eventId)
            {
                for (int j = 1; j < 6 + 1; j++)
                {
                    eventDate.Add(worksheet.Cells[i, j].Value.ToString());
                }
            }
        }
    }
    void AllFromEventId_5(ExcelWorksheet worksheet, string eventId)
    {
        for (int i = 2; i < 21 + 1; i++)
        {
            if (worksheet.Cells[i, 1].Value.ToString().ToString() == eventId)
            {
                for (int j = 1; j < 5 + 1; j++)
                {
                    eventDate.Add(worksheet.Cells[i, j].Value.ToString());
                }
            }
        }
    }
    void AllFromEventId_1(ExcelWorksheet worksheet, string eventId)
    {
        for (int i = 2; i < 89 + 1; i++)
        {
            if (worksheet.Cells[i, 1].Value.ToString().ToString() == eventId)
            {
                for (int j = 1; j < 15 + 1; j++)
                {
                    eventDate.Add(worksheet.Cells[i, j].Value.ToString());
                }
            }
        }
    }
    void AllFromEventId_6(ExcelWorksheet worksheet, string eventId)
    {
        for (int i = 2; i < 29 + 1; i++)
        {
            if (worksheet.Cells[i, 1].Value.ToString().ToString() == eventId)
            {
                for (int j = 1; j < 6 + 1; j++)
                {
                    eventDate.Add(worksheet.Cells[i, j].Value.ToString());
                }
            }
        }
    }
    void AllFromEventId_8(ExcelWorksheet worksheet, string eventId)
    {
        for (int i = 2; i < 3 + 1; i++)
        {
            if (worksheet.Cells[i, 1].Value.ToString().ToString() == eventId)
            {
                for (int j = 1; j < 5 + 1; j++)
                {
                    eventDate.Add(worksheet.Cells[i, j].Value.ToString());
                }
            }
        }
    }
}
