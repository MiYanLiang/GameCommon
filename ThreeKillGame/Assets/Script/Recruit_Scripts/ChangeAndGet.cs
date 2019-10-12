using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using OfficeOpenXml;    //引入使用EPPlus类库

public class ChangeAndGet : MonoBehaviour
{
    public GameObject btn;
    public GameObject txt;
    List<int> mycard = new List<int>();
    List<string> getCard = new List<string>();
    int price;
    int heroId;
    int btnTag;
    int money;
    bool boolIndex;
    string btnNum;
    public GameObject hero_Card;    //英雄卡片预制件
    public Transform preparation;   //备战位
    List<string> heroData = new List<string>();
    List<int> ChickenRibsHeroId = new List<int>();  //存放所有的拥有的英雄Id
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ChangeBtnColor()
    {
        GetExcelFile1();
        ChickenRibsHeroId.Add(heroId);
        if (money >= price)
        {
            boolIndex = true;
            transform.Find("Image").gameObject.SetActive(true);
            transform.GetComponent<Button>().enabled = false;   //关闭点击事件
            //print(btnTag);
            money = money - price;
            GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>().money = money;
        }
        else
        {
            boolIndex = false;
            Debug.Log("呸，穷鬼，买不起");
        }
        if (boolIndex == true)
        {
            //GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>().getCard.Add(btnNum);
            getCard.Add(btnNum);
        }
    }
    public void GetResidueCard()
    {
        btnNum = btn.GetComponentInChildren<Text>().text;
        btnTag = int.Parse(btn.name);
        mycard = GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>().myCard;//拿到脚本CreateAndUpdate中的myCard
        money = GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>().money;
        ChickenRibsHeroId = GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>().ChickenRibsHeroId;
        //GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>().getCard.Add(btnNum);
        //for (int i = 0; i < GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>().sendCardId.Count; i++)
        //{
        //    print(GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>().sendCardId[i]);
        //}
    }
    //购买英雄
    public void UpdateGetCard()
    {
        //getCard = GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>().getCard;
        if (boolIndex == true)
        {
            txt.GetComponent<Text>().text = txt.GetComponent<Text>().text + getCard[getCard.Count - 1].ToString() + "  ";
            GetExcelFile2();
            print("heroId:" + heroId);
            int num = 0;
            while (preparation.GetChild(num).childCount > 0)
            {
                num++;
                if (num >= preparation.childCount)
                {
                    Debug.Log("备战位已满");
                    return;
                }
            }
            //实例化武将卡牌到备战位,并传递数据过去
            GameObject newheroCard = Instantiate(hero_Card, preparation.GetChild(num));
            //hero_Card.transform.SetParent(preparation.GetChild(num));
            newheroCard.transform.position = preparation.GetChild(num).position;
            //newheroCard.transform.position = new Vector3(0, 0, 0);
            newheroCard.AddComponent<HeroDataControll>();
            newheroCard.GetComponent<HeroDataControll>().heroData = heroData;
            //Debug.Log("///ChangeAndGet///+" + newheroCard.GetComponent<HeroDataControll>().heroData.Count);

            newheroCard.GetComponent<HeroDataControll>().ShowThisHeroData();
            preparation.GetChild(num).GetChild(0).GetComponent<HeroDataControll>().heroData = heroData;
            
            //heroData.Clear();

        }
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
            GetHeroId(btnTag, worksheet2);
            GetSpecificValue(heroId, worksheet1, "recruitingMoney");
            //ChickenRibsHeroId.Add(heroId);
            //print(price);
        }
    }
    //读取相应英雄的所有数据
    void GetExcelFile2()
    {
        //string filePath = "F:/dev/GameCommon/111.xlsx";   //绝对路径
        string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";  //相对路径
        FileInfo fileinfo = new FileInfo(filePath);
        using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))   //using用来强行做资源释放
        {
            ExcelWorksheet worksheet1 = excelpackge.Workbook.Worksheets[1];
            ExcelWorksheet worksheet2 = excelpackge.Workbook.Worksheets[2];
            GetHeroDateFromId(heroId, worksheet1);
            //print(price);
        }
    }

    //获取表中英雄的所有数据
    void GetHeroDateFromId(int id, ExcelWorksheet worksheet)
    {
        int num = 0;
        string rowTxt = "";
        for (int i = 1; i < 87 + 1; i++)
        {
            for (int j = 1; j < 21 + 1; j++)
            {
                if (j == 1 && i > 1)
                {
                    if (int.Parse(worksheet.Cells[i, j].Value.ToString()) == id)    //通过Id获取当前单元格在第几行
                    {
                        string n = worksheet.Cells[i, j].GetEnumerator().ToString();
                        for (int x = 0; x < n.Length; x++)
                        {
                            if (x > 0)
                            {
                                rowTxt = rowTxt + n[x];
                            }
                        }
                        num = int.Parse(rowTxt);
                    }
                }
            }
        }
        for (int y = 1; y < 15 + 1; y++)
        {
            //获取存储该英雄的所有数据到heroData
            heroData.Add(worksheet.Cells[num, y].Value.ToString());
        }
    }

    //获取表中英雄的价格
    void GetSpecificValue(int id, ExcelWorksheet worksheet, string name)
    {
        int num = 0;
        string numy = "";
        string rowTxt = "";
        for (int i = 1; i < 87 + 1; i++)
        {
            for (int j = 1; j < 21 + 1; j++)
            {
                if (j == 1 && i > 1)
                {
                    if (int.Parse(worksheet.Cells[i, j].Value.ToString()) == id)    //通过Id获取当前单元格在第几行
                    {
                        string n = worksheet.Cells[i, j].GetEnumerator().ToString();
                        for (int x = 0; x < n.Length; x++)
                        {
                            if (x > 0)
                            {
                                rowTxt = rowTxt + n[x];
                            }
                        }
                        num = int.Parse(rowTxt);
                    }
                }
                if (i == 1)
                {
                    if (worksheet.Cells[i, j].Value.ToString() == name)   //通过列名获取当前列的首字母
                    {
                        string n = worksheet.Cells[i, j].GetEnumerator().ToString();
                        numy = n[0].ToString();
                    }
                }
            }
        }
        for (int y = 1; y < 21 + 1; y++)
        {
            if (name == "recruitingMoney")
            {
                if (worksheet.Cells[num, y].GetEnumerator().ToString() == numy + num.ToString())
                {
                    price = int.Parse(worksheet.Cells[num, y].Value.ToString());
                }
            }
        }
    }
    
    //在表一中拿到点击英雄的id
    void GetHeroId(int num, ExcelWorksheet worksheet)
    {
        for (int i = 1; i < 784 + 1; i++)
        {
            for (int j = 1; j < 2 + 1; j++)
            {
                if (worksheet.Cells[i, 2].Value.ToString() == num.ToString())
                {
                    heroId = int.Parse(worksheet.Cells[i, 1].Value.ToString());
                }
            }
        }
    }
}
