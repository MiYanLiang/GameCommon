﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroIdChangeAndSave : MonoBehaviour
{
    public static int[] pos_heroId = new int[16];     //记录九宫格和备战区的英雄id

    public Transform JiuGongGe; //九宫格
    public Transform BeiZhanWei;//备战位
    

    public List<string> fightIdList = new List<string>();   //上阵英雄id
    public List<string> allIdList = new List<string>();     //全部英雄id

    List<int> allIdList_int = new List<int>();     //全部英雄id
    List<int> fightIdList_int = new List<int>();   //上阵英雄id

    public List<string> heroTypeName = new List<string>();
    List<List<string>> fetterInformation = new List<List<string>>(); /////////////////////////////////////////////////////////////////////////激活羁绊的详细信息

    List<string> arrayGo = new List<string>() { "19", "89", "53", "54", "68" };  //上阵英雄数组,用以测试

    [SerializeField]
    GameObject SellCardBtn; //出售卡牌按钮
    [HideInInspector]
    public Transform SelectHerpCard;   //选中的英雄卡牌

    public Transform LeftInformationBar;//左侧信息栏
    public GameObject heroTypePrefab;    //信息预制件
    public GameObject forcesName;

    int shieldSoldierNum;              //盾兵数量
    int mahoutNum;                     //象兵数量
    int halberdierNum ;                //戟兵数量
    int lifeguardNum ;                 //禁卫数量
    int spearmanNum ;                   //枪兵数量
    int sowarNum ;                      //骑兵数量
    int counsellorNum ;                 //军师数量
    int sapperNum;                      //工兵数量
    int necromancerNum ;                //方士数量
    int god_beast;                      //神兽数量
    List<int> soldiersKindId = new List<int>();//上阵英雄的兵种类型
    List<int> activationSkillId_soldiers = new List<int>();//////////////////////////////////////////////////////////////////激活的兵种技能
    List<string> fetterArray = new List<string>();
    List<string> intersectionArray = new List<string>();  //存放交集
    List<int> fetterIndex = new List<int>();//存放激活羁绊的id   每次传需要清除，现在没清
    int forcesIndex;

    string[] a;
    string[] b;
    /// <summary>
    /// 出售选中的卡牌武将
    /// </summary>
    public void SellSelectHeroCard()
    {
        CreateAndUpdate.money += SelectHerpCard.GetComponent<HeroDataControll>().Price_hero;
        Destroy(SelectHerpCard.gameObject);
        SelectHerpCard = null;
        Invoke("LoadTextOfNum",0.1f);
    }
    //延时刷新备战位和上阵位人数显示
    private void LoadTextOfNum()
    {
        GameObject.Find("ArrayText").GetComponent<UIDynamicDisplay>().ChangeNumOfPeople();
        GameObject.Find("ReadyText").GetComponent<UIDynamicDisplay>().ChangeNumOfPeople();
    }

    /// <summary>
    /// 根据英雄id获取当前该英雄的拥有数量
    /// </summary>
    /// <param name="heroId">武将ID</param>
    /// <returns></returns>
    public int StatisticsHeroNums(int heroId)
    {
        int num = 0;
        for (int i = 0; i < pos_heroId.Length; i++)
        {
            if (pos_heroId[i] == heroId)
            {
                if (i < 9)    //在上阵位
                {
                    //1阶代表1个数量，2阶3个，3阶9个
                    num += ((int)Mathf.Pow(3, JiuGongGe.GetChild(i).GetChild(0).GetComponent<HeroDataControll>().Grade_hero - 1));
                }
                else
                {
                    num += ((int)Mathf.Pow(3, BeiZhanWei.GetChild(i - 9).GetChild(0).GetComponent<HeroDataControll>().Grade_hero - 1));
                }
            }
        }
        //print("拥有数为： " + num);
        return num;
    }
    //GameObject.Find("backGround").GetComponent<HeroIdChangeAndSave>().StatisticsHeroNums(int.Parse(newData[0]));


    private void Start()
    {
        forcesIndex = PlayerPrefs.GetInt("forcesId");
        for (int i = 0; i < 11; i++)
        {
            if (int.Parse(LoadJsonFile.forcesTableDatas[i][0]) == forcesIndex)
            {
                forcesName.GetComponent<Text>().text = LoadJsonFile.forcesTableDatas[i][4];//左侧信息栏显示
            }
        }
        SelectHerpCard = null;
        for (int i = 0; i < pos_heroId.Length; i++)
        {
            pos_heroId[i] = 0;
        }
        //拿到当前声望
        print("声望(prestigeNum):"+PlayerPrefs.GetInt("prestigeNum"));
    }

    private void Update()
    {
        //string str = "///";
        //for (int i = 0; i < 16; i++)
        //{
        //    str += (pos_heroId[i] + " ");
        //}
        //Debug.Log(str);
    }


    /// <summary>
    /// 刷新保存当前拥有的武将id
    /// </summary>
    public void SaveNowHeroId()
    {
        heroTypeName.Clear();
        fightIdList.Clear();
        allIdList.Clear();
        allIdList_int.Clear();
        fetterInformation.Clear();
        fightIdList_int.Clear();
        for (int i = 0; i < 9; i++)
        {
            if (JiuGongGe.GetChild(i).childCount > 0)
            {
                fightIdList.Add(JiuGongGe.GetChild(i).GetChild(0).GetComponent<HeroDataControll>().HeroData[0]);
                allIdList.Add(JiuGongGe.GetChild(i).GetChild(0).GetComponent<HeroDataControll>().HeroData[0]);
            }
        }
        for (int i = 0; i < 7; i++)
        {
            if (BeiZhanWei.GetChild(i).childCount > 0)
            {
                allIdList.Add(BeiZhanWei.GetChild(i).GetChild(0).GetComponent<HeroDataControll>().HeroData[0]);
            }
        }
        for (int i = 0; i < fightIdList.Count; i++)
        {
            fightIdList_int.Add(int.Parse(fightIdList[i]));
        }
        //测试
        //for (int i = 0; i < arrayGo.Count; i++)
        //{
        //    fightIdList_int.Add(int.Parse(arrayGo[i]));
        //}

        for (int i = 0; i < allIdList.Count; i++)
        {
            allIdList_int.Add(int.Parse(allIdList[i]));
        }
        init_Go(fightIdList);
        heroTypeName = GameObject.Find("SoldiersControl").GetComponent<SoldiersControl>().init(allIdList_int);//初始兵种信息
        Show_Left(fightIdList_int);
        //print("盾兵数量=" + shieldSoldierNum+ "象兵数量=" + mahoutNum + "戟兵数量=" + halberdierNum + "禁卫数量=" + lifeguardNum + "枪兵数量=" + spearmanNum + "骑兵数量=" + sowarNum + "军师数量=" + counsellorNum + "工兵数量=" + sapperNum + "方士数量=" + necromancerNum+"神兽数量"+ god_beast);
    }
    private void OnServerInitialized()
    {

    }

    /// <summary>
    /// 恢复所有武将卡牌未选中状态（选中框显示等）
    /// </summary>
    public void RestoreCardUnSelect(Transform tran)
    {
        if (SelectHerpCard != null)
        {
            SelectHerpCard.GetChild(3).gameObject.SetActive(false);
        }
        SelectHerpCard = tran;
        //显示出售按钮
        SellCardBtn.transform.GetChild(1).GetComponent<Text>().text = tran.GetComponent<HeroDataControll>().Price_hero.ToString();
        SellCardBtn.SetActive(true);

    }


    void Show_Left(List<int> battleHeroId)
    {
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
        soldiersKindId.Clear();
        activationSkillId_soldiers.Clear();
        GetExcelFile1(battleHeroId);
        GetSoldiersTypeNum();
        MakeLeftInformation();
        for (int i = 0; i < activationSkillId_soldiers.Count; i++)
        {
            print("activationSkillId_soldiers:"+ activationSkillId_soldiers[i]);
        }
    }

    //左侧信息栏显示
    void MakeLeftInformation()
    {
        for (int i = 1; i < heroTypeName.Count+1; i++)
        {
            GameObject obj = Instantiate(heroTypePrefab, LeftInformationBar.GetChild(i));
            obj.transform.position = LeftInformationBar.GetChild(i).position;
            obj.GetComponentsInChildren<Text>()[0].text = heroTypeName[i - 1];
            if (heroTypeName[i - 1] == "山兽")
            {
                if (shieldSoldierNum < 3)
                {
                    obj.GetComponentsInChildren<Text>()[1].text = shieldSoldierNum.ToString() + "/" + "3";
                }
                else
                {
                    obj.GetComponentsInChildren<Text>()[1].text = shieldSoldierNum.ToString() + "/" + "6";
                    if (shieldSoldierNum < 6)
                    {
                        activationSkillId_soldiers.Add(13);
                    }
                    else
                    {
                        activationSkillId_soldiers.Add(16);
                    }
                }
            }
            else if (heroTypeName[i - 1] == "海兽")
            {
                if (mahoutNum < 3)
                {
                    obj.GetComponentsInChildren<Text>()[1].text = mahoutNum.ToString() + "/" + "3";
                }
                else
                {
                    obj.GetComponentsInChildren<Text>()[1].text = mahoutNum.ToString() + "/" + "6";
                    if (mahoutNum < 6)
                    {
                        activationSkillId_soldiers.Add(23);
                    }
                    else
                    {
                        activationSkillId_soldiers.Add(26);
                    }
                }
            }
            else if (heroTypeName[i - 1] == "飞兽")
            {
                if (halberdierNum <3)
                {
                    obj.GetComponentsInChildren<Text>()[1].text = halberdierNum.ToString() + "/" + "3";
                }
                else
                {
                    obj.GetComponentsInChildren<Text>()[1].text = halberdierNum.ToString() + "/" + "6";
                    if (halberdierNum < 6)
                    {
                        activationSkillId_soldiers.Add(33);
                    }
                    else
                    {
                        activationSkillId_soldiers.Add(36);
                    }
                }
            }
            else if (heroTypeName[i - 1] == "人杰")
            {
                if (lifeguardNum <3)
                {
                    obj.GetComponentsInChildren<Text>()[1].text = lifeguardNum.ToString() + "/" + "3";
                }
                else
                {
                    obj.GetComponentsInChildren<Text>()[1].text = lifeguardNum.ToString() + "/" + "6";
                    if (lifeguardNum < 6)
                    {
                        activationSkillId_soldiers.Add(43);
                    }
                    else
                    {
                        activationSkillId_soldiers.Add(46);
                    }
                }
            }
            else if (heroTypeName[i - 1] == "祖巫")
            {
                if (spearmanNum < 3)
                {
                    obj.GetComponentsInChildren<Text>()[1].text = spearmanNum.ToString() + "/" + "3";
                }
                else
                {
                    obj.GetComponentsInChildren<Text>()[1].text = spearmanNum.ToString() + "/" + "6";
                    if(spearmanNum < 6)
                    {
                        activationSkillId_soldiers.Add(53);
                    }
                    else
                    {
                        activationSkillId_soldiers.Add(56);
                    }
                }
            }
            else if (heroTypeName[i - 1] == "散仙")
            {
                if (sowarNum < 3)
                {
                    obj.GetComponentsInChildren<Text>()[1].text = sowarNum.ToString() + "/" + "3";
                }
                else
                {
                    obj.GetComponentsInChildren<Text>()[1].text = sowarNum.ToString() + "/" + "6";
                    if (sowarNum < 6)
                    {
                        activationSkillId_soldiers.Add(63);
                    }
                    else
                    {
                        activationSkillId_soldiers.Add(66);
                    }
                }
            }
            else if (heroTypeName[i - 1] == "辅神")
            {
                if (counsellorNum < 3)
                {
                    obj.GetComponentsInChildren<Text>()[1].text = counsellorNum.ToString() + "/" + "3";
                }
                else
                {
                    obj.GetComponentsInChildren<Text>()[1].text = counsellorNum.ToString() + "/" + "6";
                    if (counsellorNum < 6)
                    {
                        activationSkillId_soldiers.Add(73);
                    }
                    else
                    {
                        activationSkillId_soldiers.Add(76);
                    }
                }
            }
            else if (heroTypeName[i - 1] == "魔神")
            {
                if (sapperNum < 3)
                {
                    obj.GetComponentsInChildren<Text>()[1].text = sapperNum.ToString() + "/" + "3";
                }
                else
                {
                    obj.GetComponentsInChildren<Text>()[1].text = sapperNum.ToString() + "/" + "6";
                    if(sapperNum < 6)
                    {
                        activationSkillId_soldiers.Add(83);
                    }
                    else
                    {
                        activationSkillId_soldiers.Add(86);
                    }
                }
            }
            else if (heroTypeName[i - 1] == "天神")
            {
                if (necromancerNum < 3)
                {
                    obj.GetComponentsInChildren<Text>()[1].text = necromancerNum.ToString() + "/" + "3";
                }
                else
                {
                    obj.GetComponentsInChildren<Text>()[1].text = necromancerNum.ToString() + "/" + "6";
                    if (necromancerNum < 6)
                    {
                        activationSkillId_soldiers.Add(93);
                    }
                    else
                    {
                        activationSkillId_soldiers.Add(96);
                    }
                }
            }
            else if (heroTypeName[i - 1] == "神兽")
            {
                if (god_beast < 3)
                {
                    obj.GetComponentsInChildren<Text>()[1].text = god_beast.ToString() + "/" + "3";
                }
                else
                {
                    obj.GetComponentsInChildren<Text>()[1].text = god_beast.ToString() + "/" + "6";
                }
            }

        }
    }
    void GetExcelFile1(List<int> battleHeroId)
    {
        //string filePath = "F:/dev/GameCommon/111.xlsx";   
        //string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        //FileInfo fileinfo = new FileInfo(filePath);
        //using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))
        //{
        //    ExcelWorksheet worksheet1 = excelpackge.Workbook.Worksheets[1];
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
                soldiersKindId.Add(int.Parse(LoadJsonFile.RoleTableDatas[i][num]));
            }
        }
    }
    //给上阵的兵种类型计数
    void GetSoldiersTypeNum()
    {
        for (int i = 0; i < soldiersKindId.Count; i++)
        {
            if (soldiersKindId[i] == 1)
            {
                shieldSoldierNum++;
            }
            else if (soldiersKindId[i] == 2)
            {
                mahoutNum++;
            }
            else if (soldiersKindId[i] == 3)
            {
                halberdierNum++;
            }
            else if (soldiersKindId[i] == 4)
            {
                lifeguardNum++;
            }
            else if (soldiersKindId[i] == 5)
            {
                spearmanNum++;
            }
            else if (soldiersKindId[i] == 6)
            {
                sowarNum++;
            }
            else if (soldiersKindId[i] == 7)
            {
                counsellorNum++;
            }
            else if (soldiersKindId[i] == 8)
            {
                sapperNum++;
            }
            else if (soldiersKindId[i] == 9)
            {
                necromancerNum++;
            }
            else if (soldiersKindId[i] == 10)
            {
                god_beast++;
            }
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////下面拿到激活的羁绊
    public void init_Go(List<string> array0)
    {
        fetterInformation.Clear();
        GetExcelFile();
        MakeArray(array0);
        GetExcelFile1();
        //for (int j = 0; j < fetterInformation.Count; j++)
        //{
        //    for (int i = 0; i < fetterInformation[j].Count; i++)
        //    {
        //        print("ssssssssssssssssssssss" + fetterInformation[j][i]);
        //    }
        //}
    }
    //读表
    void GetExcelFile()
    {
        //string filePath = "F:/dev/GameCommon/111.xlsx";   //绝对路径
        //string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        //FileInfo fileinfo = new FileInfo(filePath);
        //using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))   //using用来强行做资源释放
        //{
        //    ExcelWorksheet worksheet7 = excelpackge.Workbook.Worksheets[7];
        GetAllFetterArray();
        //}
    }
    //将所有的羁绊数组储存
    void GetAllFetterArray()
    {
        for (int i = 0; i < 46; i++)
        {
            fetterArray.Add(LoadJsonFile.FetterTableDates[i][2]);     //羁绊数组的下标为羁绊表的id-1
        }
    }
    //将读到的羁绊数组拆分，并组成数组
    //传进来上阵英雄List
    void MakeArray(List<string> battleHeroId)
    {
        for (int i = 0; i < fetterArray.Count; i++)
        {
            intersectionArray.Clear();
            List<string> heroId = new List<string>();
            //给字符串去掉首尾
            string array1 = fetterArray[i];
            string array2 = "";
            for (int j = 0; j < array1.Length; j++)
            {
                if (j > 0 && j < array1.Length - 1)
                {
                    array2 += array1[j];
                }
            }
            //将字符串按“，”分开，存储在List中
            string[] heroId1 = array2.Split(',');
            for (int j = 0; j < heroId1.Length; j++)
            {
                heroId.Add(heroId1[j]);
            }
            //将每一个羁绊数组和上阵数组做交运算
            a = heroId.ToArray();
            b = battleHeroId.ToArray();
            GetIntersection(a, b);
            ////////////////////////////////////////判断交集与heroid数组是否相等
            //将两个比较的数组排序比较
            ArraySort(intersectionArray);
            ArraySort(heroId);
            //print("i:" + i + ".." + "arr0"+".." + ArrayChangeString(intersectionArray));
            //print("i:" + i + ".." + "arr1"+".." + ArrayChangeString(heroId));
            if (ArrayChangeString(intersectionArray) == ArrayChangeString(heroId))
            {
                //print("i:" + i);
                fetterIndex.Add(i + 1);
            }
        }
    }
    //读表  拿到羁绊的相关数据
    void GetExcelFile1()
    {
        //string filePath = "F:/dev/GameCommon/111.xlsx";   //绝对路径
        //string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        //FileInfo fileinfo = new FileInfo(filePath);
        //using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))   //using用来强行做资源释放
        //{
        //    ExcelWorksheet worksheet7 = excelpackge.Workbook.Worksheets[7];
        for (int i = 0; i < fetterIndex.Count; i++)
        {
            fetterInformation.Add(GetFetterInformation(fetterIndex[i]));
        }
        //}
    }
    //获取羁绊信息
    List<string> GetFetterInformation(int id)
    {
        List<string> arr = new List<string>();
        for (int i = 1; i < 44 + 1; i++)
        {
            if (int.Parse(LoadJsonFile.FetterTableDates[i][0]) == id)
            {
                for (int j = 0; j < 16; j++)
                {
                    arr.Add(LoadJsonFile.FetterTableDates[i][j]);
                }
            }
        }
        return arr;
    }
    //两个字符串取交集
    void GetIntersection(string[] array1, string[] array2)
    {
        //数组1
        int size1 = array1.Length;

        //数组2
        int size2 = array2.Length;

        int end = size1;

        bool swap = false;

        for (int i = 0; i < end;)
        {

            swap = false;//开始假设是第一种情况

            for (int j = i; j < size2; j++)//找到与该元素存在相同的元素，将这个相同的元素交换到与该元素相同下标的位置上
            {

                if (array1[i] == array2[j])//第二种情况，找到了相等的元素
                {

                    string tmp = array2[i];//对数组2进行交换

                    array2[i] = array2[j];

                    array2[j] = tmp;

                    swap = true;//设置标志

                    break;

                }
            }

            if (swap != true)//第一种情况，没有相同元素存在时，将这个元素交换到尚未进行比较的尾部
            {
                string tmp = array1[i];

                array1[i] = array1[--end];

                array1[end] = tmp;
            }
            else
            {
                i++;
            }
        }
        //输出交集
        for (int i = 0; i < end; i++)
        {
            //print(array1[i].ToString());
            intersectionArray.Add(array1[i].ToString());
        }
    }
    //冒泡排序
    void ArraySort(List<string> arr)
    {
        string temp = "";
        for (int i = 0; i < arr.Count - 1; i++)
        {
            for (int j = 0; j < arr.Count - 1 - i; j++)
            {
                if (int.Parse(arr[j]) > int.Parse(arr[j + 1]))
                {
                    temp = arr[j + 1];
                    arr[j + 1] = arr[j];
                    arr[j] = temp;
                }
            }
        }
    }
    //将数组转换成字符串,用于比较
    string ArrayChangeString(List<string> arr)
    {
        string date = "";
        for (int i = 0; i < arr.Count; i++)
        {
            date += arr[i];
        }
        return date;
    }
}
