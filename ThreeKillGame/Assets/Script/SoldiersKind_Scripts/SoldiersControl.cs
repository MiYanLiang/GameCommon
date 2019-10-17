using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using OfficeOpenXml;    //引入使用EPPlus类库
using System.Linq;  //去除重复

public class SoldiersControl : MonoBehaviour
{
    // Start is called before the first frame update
    List<int> myHeroId = new List<int>() { 1, 1, 6, 16, 26 }; //我拥有的英雄id数列，，，，每次购买的时候给我传过来
    List<int> battleHeroId = new List<int>() { 4, 6, 3, 21, 5, 5, 3,72,71,71,72,70 };  //传过来的上阵英雄Id数列
    List<int> heroTypeAll = new List<int>();//存放我的所有兵种id
    List<int> battleHeroType = new List<int>();//存放上阵所有兵种id
    List<string> heroTypeName = new List<string>();//存放我的所有兵种名字，无重复
    int shieldSoldierNum = 0;              //盾兵数量
    int mahoutNum = 0;                     //象兵数量
    int halberdierNum = 0;                //戟兵数量
    int lifeguardNum = 0;                 //禁卫数量
    int spearmanNum = 0;                   //枪兵数量
    int sowarNum = 0;                      //骑兵数量
    int counsellorNum = 0;                 //军师数量
    int sapperNum = 0;                      //工兵数量
    int necromancerNum = 0;                //方士数量
    ////////////////////////////////////////////////////////////下面是分母
    int shieldSoldierNum_All = 3;              //盾兵数量
    int mahoutNum_All = 3;                    //象兵数量
    int halberdierNum_All = 3;              //戟兵数量
    int lifeguardNum_All = 3;                //禁卫数量
    int spearmanNum_All = 3;                 //枪兵数量
    int sowarNum_All = 3;                     //骑兵数量
    int counsellorNum_All = 3;                //军师数量
    int sapperNum_All = 3;                   //工兵数量
    int necromancerNum_All = 3;               //方士数量
    ////////////////////////////////////////////////////////////技能id
    int skillIndex= 0;
    List<int> skillArray = new List<int>(); //开通技能列表
    List<string> skillInformation = new List<string>();//技能信息，暂时只存技能名字
    ///////////////////////////////////////////////////////////存放某类型兵种的所有id
    List<int> shieldSoldierId = new List<int>();  //盾兵
    List<int> mahoutId = new List<int>();  //象兵
    List<int> halberdierId = new List<int>();  //戟兵
    List<int> lifeguardId = new List<int>();  //禁卫
    List<int> spearmanId = new List<int>();  //枪兵
    List<int> sowarId = new List<int>();  //骑兵
    List<int> counsellorId = new List<int>();  //军师
    List<int> sapperId = new List<int>();  //工兵
    List<int> necromancerId = new List<int>();  //方士
 ////////////////////////////////////////////////////////存放某类型兵种的名字
    List<string> shieldSoldierName = new List<string>();  //盾兵
    List<string> mahoutName = new List<string>();  //象兵
    List<string> halberdierName = new List<string>();  //戟兵
    List<string> lifeguardName = new List<string>();  //禁卫
    List<string> spearmanName  = new List<string>();  //枪兵
    List<string> sowarName = new List<string>();  //骑兵
    List<string> counsellorName = new List<string>();  //军师
    List<string> sapperName = new List<string>();  //工兵
    List<string> necromancerName = new List<string>();  //方士
    void Start()
    {
        init();
        init_up();
        //for (int i = 0; i < heroTypeName.Count; i++)
        //{
        //    print(heroTypeName[i]);
        //}
        init_btn();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void init()
    {
        GetExcelFile();
        GetHeroTypeName();
    }
    void GetExcelFile()
    {
        //string filePath = "F:/dev/GameCommon/111.xlsx";   
        string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        FileInfo fileinfo = new FileInfo(filePath);
        using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))
        {
            ExcelWorksheet worksheet1 = excelpackge.Workbook.Worksheets[1];
            ArraySort(myHeroId);
            for (int i = 0; i < myHeroId.Count; i++)
            {
                GetHeroType(worksheet1, myHeroId[i]);
            }
            //给heroTypeAll数列去除重复数据
            int[] str = heroTypeAll.ToArray();
            int[] str2 = str.Distinct().ToArray();
            heroTypeAll = new List<int>(str2);
        }
    }
    //传入英雄id，拿到英雄兵种类型
    void GetHeroType(ExcelWorksheet worksheet, int id)
    {
        int num = 4;
        for (int i = 1; i < 88 + 1; i++)
        {
            if (i > 1)
            {
                if (int.Parse(worksheet.Cells[i, 1].Value.ToString()) == id)
                {
                    heroTypeAll.Add(int.Parse(worksheet.Cells[i, num].Value.ToString()));
                }
            }
        }
    }
    //拿到对应兵种的名字
    void GetHeroTypeName()
    {
        for (int i = 0; i < heroTypeAll.Count; i++)
        {
            string name = "";
            if (heroTypeAll[i] == 1)
            {
                name = "盾兵";
            }
            else if (heroTypeAll[i] == 2)
            {
                name = "象兵";
            }
            else if (heroTypeAll[i] == 3)
            {
                name = "戟兵";
            }
            else if (heroTypeAll[i] == 4)
            {
                name = "禁卫";
            }
            else if (heroTypeAll[i] == 5)
            {
                name = "枪兵";
            }
            else if (heroTypeAll[i] == 6)
            {
                name = "骑兵";
            }
            else if (heroTypeAll[i] == 7)
            {
                name = "军师";
            }
            else if (heroTypeAll[i] == 8)
            {
                name = "工兵";
            }
            else if (heroTypeAll[i] == 9)
            {
                name = "方士";
            }
            heroTypeName.Add(name);
        }
    }
    //冒泡排序
    void ArraySort(List<int> arr)
    {
        int temp = 0;
        for (int i = 0; i < arr.Count - 1; i++)
        {
            for (int j = 0; j < arr.Count - 1 - i; j++)
            {
                if (arr[j] > arr[j + 1])
                {
                    temp = arr[j + 1];
                    arr[j + 1] = arr[j];
                    arr[j] = temp;
                }
            }
        }
    }
    /////////////////////////////////////////////上面是玩家拥有的，也就是购买的所有英雄可能激活的左边信息
    /////////////////////////////////////////////下面是玩家拖上阵的英雄累计
    void init_up()
    {
        De_weightForBattleHeroId();
        GetExcelFile1();
        GetSoldiersTypeNum();
        GetSkillId();
        GetExcelFile2();
        //for (int i = 0; i < skillInformation.Count; i++)
        //{
        //    print(skillInformation[i]);
        //}
    }
    //给传过来的上阵英雄数列去重
    void De_weightForBattleHeroId()
    {
        int[] str = battleHeroId.ToArray();
        int[] str2 = str.Distinct().ToArray();
        battleHeroId = new List<int>(str2);
    }
    //读表
    void GetExcelFile1()
    {
        //string filePath = "F:/dev/GameCommon/111.xlsx";   
        string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        FileInfo fileinfo = new FileInfo(filePath);
        using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))
        {
            ExcelWorksheet worksheet1 = excelpackge.Workbook.Worksheets[1];
            for (int i = 0; i < battleHeroId.Count; i++)
            {
                GetHeroTypeFromId(worksheet1, battleHeroId[i]);
            }
        }
    }
    //传入英雄id，拿到英雄兵种类型
    void GetHeroTypeFromId(ExcelWorksheet worksheet, int id)
    {
        int num = 4;
        for (int i = 1; i < 88 + 1; i++)
        {
            if (i > 1)
            {
                if (int.Parse(worksheet.Cells[i, 1].Value.ToString()) == id)
                {
                    battleHeroType.Add(int.Parse(worksheet.Cells[i, num].Value.ToString()));
                }
            }
        }
    }
    //给上阵的兵种类型计数
    void GetSoldiersTypeNum()
    {
        for (int i=0;i<battleHeroType.Count;i++)
        {
            if (battleHeroType[i] == 1)
            {
                shieldSoldierNum++;
            }
            else if (battleHeroType[i] == 2)
            {
                mahoutNum++;
            }
            else if (battleHeroType[i] == 3)
            {
                halberdierNum++;
            }
            else if (battleHeroType[i] == 4)
            {
                lifeguardNum++;
            }
            else if (battleHeroType[i] == 5)
            {
                spearmanNum++;
            }
            else if (battleHeroType[i] == 6)
            {
                sowarNum++;
            }
            else if (battleHeroType[i] == 7)
            {
                counsellorNum++;
            }
            else if (battleHeroType[i] == 8)
            {
                sapperNum++;
            }
            else if (battleHeroType[i] == 9)
            {
                necromancerNum++;
            }
        }
    }
    //通过条件获得激活
    void GetSkillId()
    {
        if (shieldSoldierNum > 2 && shieldSoldierNum < 6)
        {
            shieldSoldierNum_All = 3;
            skillIndex = 13;
            skillArray.Add(skillIndex);
        }
        else if (shieldSoldierNum > 5)
        {
            shieldSoldierNum_All = 6;
            skillIndex = 16;
            skillArray.Add(skillIndex);
        }
        if (mahoutNum > 2 && mahoutNum < 6)
        {
            mahoutNum_All = 3;
            skillIndex = 23;
        }
        else if (mahoutNum > 5)
        {
            mahoutNum_All = 6;
            skillIndex = 26;
            skillArray.Add(skillIndex);
        }
        if (halberdierNum > 2 && halberdierNum < 6)
        {
            halberdierNum_All = 3;
            skillIndex = 33;
            skillArray.Add(skillIndex);
        }
        else if (halberdierNum > 5)
        {
            halberdierNum_All = 6;
            skillIndex = 36;
            skillArray.Add(skillIndex);
        }
        if (lifeguardNum > 2 && lifeguardNum < 6)
        {
            lifeguardNum_All = 3;
            skillIndex = 43;
            skillArray.Add(skillIndex);
        }
        else if (lifeguardNum > 5)
        {
            lifeguardNum_All = 6;
            skillIndex = 46;
            skillArray.Add(skillIndex);
        }
        if (spearmanNum > 2 && spearmanNum < 6)
        {
            spearmanNum_All = 3;
            skillIndex = 53;
            skillArray.Add(skillIndex);
        }
        else if (spearmanNum > 5)
        {
            spearmanNum_All = 6;
            skillIndex = 56;
            skillArray.Add(skillIndex);
        }
        if (sowarNum > 2 && sowarNum < 6)
        {
            sowarNum_All = 3;
            skillIndex = 63;
            skillArray.Add(skillIndex);
        }
        else if (sowarNum  > 5)
        {
            sowarNum_All = 6;
            skillIndex = 66;
            skillArray.Add(skillIndex);
        }
        if (counsellorNum > 2 && counsellorNum < 6)
        {
            counsellorNum_All = 3;
            skillIndex = 73;
            skillArray.Add(skillIndex);
        }
        else if (counsellorNum > 5)
        {
            counsellorNum_All = 6;
            skillIndex = 76;
            skillArray.Add(skillIndex);
        }
        if (sapperNum > 2 && sapperNum < 6)
        {
            sapperNum_All = 3;
            skillIndex = 83;
            skillArray.Add(skillIndex);
        }
        else if (sapperNum > 5)
        {
            sapperNum_All = 6;
            skillIndex = 86;
            skillArray.Add(skillIndex);
        }
        if(necromancerNum > 2 && necromancerNum < 6)
        {
            necromancerNum_All = 3;
            skillIndex = 93;
            skillArray.Add(skillIndex);
        }
        else if (necromancerNum > 5)
        {
            necromancerNum_All = 6;
            skillIndex = 96;
            skillArray.Add(skillIndex);
        }
    }
    void GetExcelFile2()
    {
        //string filePath = "F:/dev/GameCommon/111.xlsx";   
        string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        FileInfo fileinfo = new FileInfo(filePath);
        using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))
        {
            ExcelWorksheet worksheet1 = excelpackge.Workbook.Worksheets["SoldierSkillTable"];
            for (int i = 0; i < skillArray.Count; i++)
            {
                GetSkillInformationFromSkillId(worksheet1, skillArray[i]);
            }
        }
    }
    //通过兵种skill索引获取信息
    void GetSkillInformationFromSkillId(ExcelWorksheet worksheet,int index)
    {
        int num = 2;
        for (int i = 1; i < 19 + 1; i++)
        {
            if (i > 1)
            {
                if (int.Parse(worksheet.Cells[i, 1].Value.ToString()) == index)
                {
                    skillInformation.Add(worksheet.Cells[i, num].Value.ToString());
                }
            }
        }
    }
    ////////////////////////////////////////////////////////下面的方法可以是点击左边兵种，获取相同兵种的其他英雄名字
    void init_btn()
    {
        GetExcelFile3();
        for (int i = 0; i < shieldSoldierName.Count; i++)
        {
            //print(shieldSoldierName[i]);
        }
    }
    void GetExcelFile3()
    {
        //string filePath = "F:/dev/GameCommon/111.xlsx";   
        string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        FileInfo fileinfo = new FileInfo(filePath);
        using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))
        {
            ExcelWorksheet worksheet1 = excelpackge.Workbook.Worksheets[1];
            GetSoldiersAll(worksheet1);
            for (int i = 0; i < shieldSoldierId.Count; i++)
            {
                shieldSoldierName.Add(GetHeroNameFromId(worksheet1,shieldSoldierId[i]));
            }
            for (int i = 0; i < mahoutId.Count; i++)
            {
                mahoutName.Add(GetHeroNameFromId(worksheet1, mahoutId[i]));
            }
            for (int i = 0; i < halberdierId.Count; i++)
            {
                halberdierName.Add(GetHeroNameFromId(worksheet1, halberdierId[i]));
            }
            for (int i = 0; i < lifeguardId.Count; i++)
            {
                lifeguardName.Add(GetHeroNameFromId(worksheet1, lifeguardId[i]));
            }
            for (int i = 0; i < spearmanId.Count; i++)
            {
                spearmanName.Add(GetHeroNameFromId(worksheet1, spearmanId[i]));
            }
            for (int i = 0; i < sowarId.Count; i++)
            {
                sowarName.Add(GetHeroNameFromId(worksheet1, sowarId[i]));
            }
            for (int i = 0; i < counsellorId.Count; i++)
            {
                counsellorName.Add(GetHeroNameFromId(worksheet1, counsellorId[i]));
            }
            for (int i = 0; i < sapperId.Count; i++)
            {
                sapperName.Add(GetHeroNameFromId(worksheet1, sapperId[i]));
            }
            for (int i = 0; i < necromancerId.Count; i++)
            {
                necromancerName.Add(GetHeroNameFromId(worksheet1, necromancerId[i]));
            }
        }
    }
    void GetSoldiersAll(ExcelWorksheet worksheet)
    {
        int num = 4;     
        for (int i = 1; i < 88 + 1; i++)
        {
            if (i > 1)
            {
                if (worksheet.Cells[i, num].Value.ToString() == "1")
                {
                    shieldSoldierId.Add(int.Parse(worksheet.Cells[i, 1].Value.ToString()));
                }
                else if (worksheet.Cells[i, num].Value.ToString() == "2")
                {
                    mahoutId.Add(int.Parse(worksheet.Cells[i, 1].Value.ToString()));
                }
                else if (worksheet.Cells[i, num].Value.ToString() == "3")
                {
                    halberdierId.Add(int.Parse(worksheet.Cells[i, 1].Value.ToString()));
                }
                else if (worksheet.Cells[i, num].Value.ToString() == "4")
                {
                    lifeguardId.Add(int.Parse(worksheet.Cells[i, 1].Value.ToString()));
                }
                else if (worksheet.Cells[i, num].Value.ToString() == "5")
                {
                    spearmanId.Add(int.Parse(worksheet.Cells[i, 1].Value.ToString()));
                }
                else if (worksheet.Cells[i, num].Value.ToString() == "6")
                {
                    sowarId.Add(int.Parse(worksheet.Cells[i, 1].Value.ToString()));
                }
                else if (worksheet.Cells[i, num].Value.ToString() == "7")
                {
                    counsellorId.Add(int.Parse(worksheet.Cells[i, 1].Value.ToString()));
                }
                else if (worksheet.Cells[i, num].Value.ToString() == "8")
                {
                    sapperId.Add(int.Parse(worksheet.Cells[i, 1].Value.ToString()));
                }
                else if (worksheet.Cells[i, num].Value.ToString() == "9")
                {
                    necromancerId.Add(int.Parse(worksheet.Cells[i, 1].Value.ToString()));
                }
            }
        }
    }
    string GetHeroNameFromId(ExcelWorksheet worksheet, int id)
    {
        string name = "";
        for (int i = 1; i < 88 + 1; i++)
        {
            if (i > 1)
            {
                if (int.Parse(worksheet.Cells[i, 1].Value.ToString()) == id)
                {
                    name = worksheet.Cells[i, 2].Value.ToString();
                }
            }
        }
        return name;
    }
}
