using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;  //去除重复

public class SoldiersControl : MonoBehaviour
{
    // Start is called before the first frame update
    List<int> myHeroId = new List<int>() { 1, 1, 6, 16, 26 }; //我拥有的英雄id数列，，，，每次购买的时候给我传过来
    List<int> battleHeroId_ = new List<int>() { 4, 6, 3, 21, 5, 5, 3,72,71,71,72,70 };  //传过来的上阵英雄Id数列
    List<int> heroTypeAll = new List<int>();//存放我的所有兵种id
    List<int> battleHeroType = new List<int>();//存放上阵所有兵种id
    List<string> heroTypeName = new List<string>();//存放我的所有兵种名字，无重复
    [HideInInspector]
    public int shieldSoldierNum = 0;              //盾兵数量
    [HideInInspector]
    public int mahoutNum = 0;                     //象兵数量
    [HideInInspector]
    public int halberdierNum = 0;                //戟兵数量
    [HideInInspector]
    public int lifeguardNum = 0;                 //禁卫数量
    [HideInInspector]
    public int spearmanNum = 0;                   //枪兵数量
    [HideInInspector]
    public int sowarNum = 0;                      //骑兵数量
    [HideInInspector]
    public int counsellorNum = 0;                 //军师数量
    [HideInInspector]
    public int sapperNum = 0;                      //工兵数量
    [HideInInspector]
    public int necromancerNum = 0;                //方士数量
    [HideInInspector]
    public int god_beast = 0;                      //神兽数量
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
        //init(myHeroId);

        //init_up(battleHeroId_);
        //for (int i = 0; i < heroTypeName.Count; i++)
        //{
        //    print(heroTypeName[i]);
        //}
        //init_btn();
        //init_up(battleHeroId_);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public List<string> init(List<int> myHeroId)
    {
        heroTypeName.Clear();
        GetExcelFile(myHeroId);
        GetHeroTypeName();
        //for (int i = 0; i < heroTypeName.Count; i++)
        //{
        //    print("init:"+heroTypeName[i]);
        //}
        return heroTypeName;
    }
    void GetExcelFile(List<int> myHeroId)
    {
        ////string filePath = "F:/dev/GameCommon/111.xlsx";   
        //string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        //FileInfo fileinfo = new FileInfo(filePath);
        //using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))
        //{
        //    ExcelWorksheet worksheet1 = excelpackge.Workbook.Worksheets[1];
            ArraySort(myHeroId);
            for (int i = 0; i < myHeroId.Count; i++)
            {
                GetHeroType(myHeroId[i]);
            }
            //给heroTypeAll数列去除重复数据
            int[] str = heroTypeAll.ToArray();
            int[] str2 = str.Distinct().ToArray();
            heroTypeAll = new List<int>(str2);
        //}
    }
    //传入英雄id，拿到英雄兵种类型
    void GetHeroType(int id)
    {
        int num = 3;
        for (int i = 0; i < 88; i++)
        {
            if (int.Parse(LoadJsonFile.RoleTableDatas[i][0]) == id)
            {
                heroTypeAll.Add(int.Parse(LoadJsonFile.RoleTableDatas[i][num]));
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
                name = "山兽";
            }
            else if (heroTypeAll[i] == 2)
            {
                name = "海兽";
            }
            else if (heroTypeAll[i] == 3)
            {
                name = "飞兽";
            }
            else if (heroTypeAll[i] == 4)
            {
                name = "人杰";
            }
            else if (heroTypeAll[i] == 5)
            {
                name = "祖巫";
            }
            else if (heroTypeAll[i] == 6)
            {
                name = "散仙";
            }
            else if (heroTypeAll[i] == 7)
            {
                name = "辅神";
            }
            else if (heroTypeAll[i] == 8)
            {
                name = "魔神";
            }
            else if (heroTypeAll[i] == 9)
            {
                name = "天神";
            }
            else if (heroTypeAll[i] == 10)
            {
                name = "神兽";
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
    public List<string> init_up(List<int> battleHeroId)
    {
        print("进来");
        skillInformation.Clear();
        shieldSoldierNum = 0;              //盾兵数量
        mahoutNum = 0;                     //象兵数量
        halberdierNum = 0;                //戟兵数量
        lifeguardNum = 0;                 //禁卫数量
        spearmanNum = 0;                   //枪兵数量
        sowarNum = 0;                      //骑兵数量
        counsellorNum = 0;                 //军师数量
        sapperNum = 0;                      //工兵数量
        necromancerNum = 0;                //方士数量
        god_beast = 0;                     //神兽数量
        De_weightForBattleHeroId(battleHeroId);
        GetExcelFile1(battleHeroId);
        GetSoldiersTypeNum();
        GetSkillId();
        GetExcelFile2();
        //print("sssssssssssssss"+"盾兵数量=" + shieldSoldierNum + "象兵数量=" + mahoutNum + "戟兵数量=" + halberdierNum + "禁卫数量=" + lifeguardNum + "枪兵数量=" + spearmanNum + "骑兵数量=" + sowarNum + "军师数量=" + counsellorNum + "工兵数量=" + sapperNum + "方士数量=" + necromancerNum+"神兽数量"+ god_beast);
        //for (int i = 0; i < skillInformation.Count; i++)
        //{
        //    print(skillInformation[i]);
        //}
        return skillInformation;
    }
    //给传过来的上阵英雄数列去重
    void De_weightForBattleHeroId(List<int> battleHeroId)
    {
        int[] str = battleHeroId.ToArray();
        int[] str2 = str.Distinct().ToArray();
        battleHeroId = new List<int>(str2);
    }
    //读表
    void GetExcelFile1(List<int> battleHeroId)
    {
        //string filePath = "F:/dev/GameCommon/111.xlsx";   
        //string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        //FileInfo fileinfo = new FileInfo(filePath);
        //using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))
        //{
            //ExcelWorksheet worksheet1 = excelpackge.Workbook.Worksheets[1];
            for (int i = 0; i < battleHeroId.Count; i++)
            {
                GetHeroTypeFromId(battleHeroId[i]);
            }
        //}
    }
    //传入英雄id，拿到英雄兵种类型
    void GetHeroTypeFromId(int id)
    {
        int num = 3;
        for (int i = 0; i < 88; i++)
        {
            if (int.Parse(LoadJsonFile.RoleTableDatas[i][0]) == id)
            {
                battleHeroType.Add(int.Parse(LoadJsonFile.RoleTableDatas[i][num]));
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
            else if (battleHeroType[i] == 10)
            {
                god_beast++;
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
        //string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        //FileInfo fileinfo = new FileInfo(filePath);
        //using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))
        //{
        //    ExcelWorksheet worksheet1 = excelpackge.Workbook.Worksheets["SoldierSkillTable"];
            for (int i = 0; i < skillArray.Count; i++)
            {
                GetSkillInformationFromSkillId(skillArray[i]);
            }
        //}
    }
    //通过兵种skill索引获取信息
    void GetSkillInformationFromSkillId(int index)
    {
        int num = 1;
        for (int i = 0; i < 18; i++)
        {
            if (int.Parse(LoadJsonFile.soldierSkillTableDatas[i][0]) == index)
            {
                skillInformation.Add(LoadJsonFile.soldierSkillTableDatas[i][num]);
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
        //string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        //FileInfo fileinfo = new FileInfo(filePath);
        //using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))
        //{
        //    ExcelWorksheet worksheet1 = excelpackge.Workbook.Worksheets[1];
            GetSoldiersAll();
            for (int i = 0; i < shieldSoldierId.Count; i++)
            {
                shieldSoldierName.Add(GetHeroNameFromId(shieldSoldierId[i]));
            }
            for (int i = 0; i < mahoutId.Count; i++)
            {
                mahoutName.Add(GetHeroNameFromId( mahoutId[i]));
            }
            for (int i = 0; i < halberdierId.Count; i++)
            {
                halberdierName.Add(GetHeroNameFromId( halberdierId[i]));
            }
            for (int i = 0; i < lifeguardId.Count; i++)
            {
                lifeguardName.Add(GetHeroNameFromId( lifeguardId[i]));
            }
            for (int i = 0; i < spearmanId.Count; i++)
            {
                spearmanName.Add(GetHeroNameFromId( spearmanId[i]));
            }
            for (int i = 0; i < sowarId.Count; i++)
            {
                sowarName.Add(GetHeroNameFromId( sowarId[i]));
            }
            for (int i = 0; i < counsellorId.Count; i++)
            {
                counsellorName.Add(GetHeroNameFromId( counsellorId[i]));
            }
            for (int i = 0; i < sapperId.Count; i++)
            {
                sapperName.Add(GetHeroNameFromId( sapperId[i]));
            }
            for (int i = 0; i < necromancerId.Count; i++)
            {
                necromancerName.Add(GetHeroNameFromId( necromancerId[i]));
            }
        //}
    }
    void GetSoldiersAll()
    {
        int num = 3;
        for (int i = 0; i < 88; i++)
        {
            if (LoadJsonFile.RoleTableDatas[i][num] == "1")
            {
                shieldSoldierId.Add(int.Parse(LoadJsonFile.RoleTableDatas[i][0]));
            }
            else if (LoadJsonFile.RoleTableDatas[i][num] == "2")
            {
                mahoutId.Add(int.Parse(LoadJsonFile.RoleTableDatas[i][0]));
            }
            else if (LoadJsonFile.RoleTableDatas[i][num] == "3")
            {
                halberdierId.Add(int.Parse(LoadJsonFile.RoleTableDatas[i][0]));
            }
            else if (LoadJsonFile.RoleTableDatas[i][num] == "4")
            {
                lifeguardId.Add(int.Parse(LoadJsonFile.RoleTableDatas[i][0]));
            }
            else if (LoadJsonFile.RoleTableDatas[i][num] == "5")
            {
                spearmanId.Add(int.Parse(LoadJsonFile.RoleTableDatas[i][0]));
            }
            else if (LoadJsonFile.RoleTableDatas[i][num] == "6")
            {
                sowarId.Add(int.Parse(LoadJsonFile.RoleTableDatas[i][0]));
            }
            else if (LoadJsonFile.RoleTableDatas[i][num] == "7")
            {
                counsellorId.Add(int.Parse(LoadJsonFile.RoleTableDatas[i][0]));
            }
            else if (LoadJsonFile.RoleTableDatas[i][num] == "8")
            {
                sapperId.Add(int.Parse(LoadJsonFile.RoleTableDatas[i][0]));
            }
            else if (LoadJsonFile.RoleTableDatas[i][num] == "9")
            {
                necromancerId.Add(int.Parse(LoadJsonFile.RoleTableDatas[i][0]));
            }
        }
    }
    string GetHeroNameFromId(int id)
    {
        string name = "";
        for (int i = 0; i < 88; i++)
        {
            if (int.Parse(LoadJsonFile.RoleTableDatas[i][0]) == id)
            {
                name = LoadJsonFile.RoleTableDatas[i][1];
            }
        }
        return name;
    }
}
