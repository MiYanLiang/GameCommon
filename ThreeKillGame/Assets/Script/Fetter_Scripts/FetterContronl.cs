using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using OfficeOpenXml;    //引入使用EPPlus类库


public class FetterContronl : MonoBehaviour
{
    List<string> arrayTest = new List<string>() { "29" };   //点击的英雄id
    List<string> arrayGo = new List<string>() { "19", "89", "104", "67", "68" };  //上阵英雄数组

    int heroId_One;
    // Start is called before the first frame update
    List<string> fetterArray = new List<string>();
    List<string> intersectionArray = new List<string>();  //存放交集
    List<int> fetterIndex = new List<int>();//存放激活羁绊的id   每次传需要清除，现在没清
    List<List<string>> fetterInformation = new List<List<string>>();//二维数组，存放激活的羁绊的所有信息
    List<int> fetterId = new List<int>();//存放点击英雄可能激活羁绊的id   每次传需要清除，现在没清
    List<List<string>> fetterInformationFromId = new List<List<string>>();//二维数组，存放点击英雄可能激活的羁绊的所有信息
    [HideInInspector]
    public List<List<string>> fetterInformationFromId1 = new List<List<string>>();
    public List<List<string>> fetterInformation1 = new List<List<string>>();

    string[] a;
    string[] b;
    

    private void Awake()
    {
        //init_Go(arrayGo);
        //init_One(arrayTest);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void init_Go(List<string> array0)
    {
        fetterInformation.Clear();
        GetExcelFile();
        MakeArray(array0);
        GetExcelFile1();
        fetterInformation1 = fetterInformation;
        //for (int j = 0; j < fetterInformation.Count; j++)
        //{
        //    for (int i = 0; i < fetterInformation[j].Count; i++)
        //    {
        //        print(fetterInformation[j][i]);
        //    }
        //}
    }
    //读表
    void GetExcelFile()
    {
        //string filePath = "F:/dev/GameCommon/111.xlsx";   //绝对路径
        string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        FileInfo fileinfo = new FileInfo(filePath);
        using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))   //using用来强行做资源释放
        {
            ExcelWorksheet worksheet7 = excelpackge.Workbook.Worksheets[7];
            GetAllFetterArray(worksheet7);
        }
    }
    //将所有的羁绊数组储存
    void GetAllFetterArray(ExcelWorksheet worksheet)
    {
        for (int i = 2; i < 44 + 1; i++)
        {
            fetterArray.Add(worksheet.Cells[i, 3].Value.ToString());     //羁绊数组的下标为羁绊表的id-1
        }
    }
    /// /////////////////////////////////////////////////////上面方法通用
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
                fetterIndex.Add(i+1);
            }
        }
    }
    //读表  拿到羁绊的相关数据
    void GetExcelFile1()
    {
        //string filePath = "F:/dev/GameCommon/111.xlsx";   //绝对路径
        string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        FileInfo fileinfo = new FileInfo(filePath);
        using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))   //using用来强行做资源释放
        {
            ExcelWorksheet worksheet7 = excelpackge.Workbook.Worksheets[7];
            for (int i = 0; i < fetterIndex.Count; i++)
            {
                fetterInformation.Add(GetFetterInformation(worksheet7, fetterIndex[i]));
            }
        }
    }
    //获取羁绊信息
    List<string> GetFetterInformation(ExcelWorksheet worksheet, int id)
    {
        List<string> arr = new List<string>();
        for (int i = 1; i < 44 + 1; i++)
        {
            if (i > 1)
            {
                if (int.Parse(worksheet.Cells[i, 1].Value.ToString()) == id)
                {
                    for (int j = 1; j < 10 + 1; j++)
                    {
                        arr.Add(worksheet.Cells[i, j].Value.ToString());
                    }
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
        string temp="";
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
    ////////////////////////////////////////上面实现了传进来上阵英雄数组，判断激活哪些羁绊
    ////////////////////////////////////////下面实现了点击英雄，通过英雄id获取与谁形成羁绊，且羁绊属性
    public List<List<string>> init_One(List<string> array1)
    {
        //print("进来");
        fetterInformationFromId.Clear();
        //fetterInformationFromId1.Clear();
        fetterId.Clear();
        GetExcelFile();
        MakeArray1(array1);
        GetExcelFile2();
        //for (int i = 0; i < fetterInformationFromId.Count; i++)
        //{
        //    fetterInformationFromId[i].Clear();
        //}
        //fetterInformationFromId1 = fetterInformationFromId;
        //GetExcelFile3();
        //for (int i = 0; i < fetterId.Count; i++)
        //{
        //    print(fetterId[i]);
        //}
        //for (int j = 0; j < fetterInformationFromId.Count; j++)
        //{
        //    for (int i = 0; i < fetterInformationFromId[j].Count; i++)
        //    {
        //        print(fetterInformationFromId[j][i]);
        //    }
        //}
        return fetterInformationFromId;

    }
    //传进来点击的英雄id
    void MakeArray1(List<string> ClickHeroId)
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
            b = ClickHeroId.ToArray();
            GetIntersection(a, b);
            ////////////////////////////////////////判断交集与heroid数组是否相等
            //将两个比较的数组排序比较
            ArraySort(intersectionArray);
            ArraySort(ClickHeroId);
            if (ArrayChangeString(intersectionArray) == ArrayChangeString(ClickHeroId))
            {
                //print("i:" + i);
                fetterId.Add(i + 1);
            }
        }
    }
    //读表  拿到羁绊的相关数据
    void GetExcelFile2()
    {
        //string filePath = "F:/dev/GameCommon/111.xlsx";   //绝对路径
        string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        FileInfo fileinfo = new FileInfo(filePath);
        using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))   //using用来强行做资源释放
        {
            ExcelWorksheet worksheet7 = excelpackge.Workbook.Worksheets[7];
            for (int i = 0; i < fetterId.Count; i++)
            {
                fetterInformationFromId.Add(GetFetterInformationFromId(worksheet7, fetterId[i]));
            }
        }
    }
    //获取羁绊英雄名
    List<string> GetFetterInformationFromId(ExcelWorksheet worksheet, int id)
    {
        List<string> arr = new List<string>();
        for (int i = 1; i < 44 + 1; i++)
        {
            if (i > 1)
            {
                if (int.Parse(worksheet.Cells[i, 1].Value.ToString()) == id)
                {
                    for (int j = 1; j < 10 + 1; j++)
                    {
                        arr.Add(worksheet.Cells[i, j].Value.ToString());
                    }
                }

            }
        }
        return arr;
    }
}
