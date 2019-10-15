using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using OfficeOpenXml;    //引入使用EPPlus类库

public class text2 : MonoBehaviour
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
    TableDatas tableData = new TableDatas();    //存储武将表
    UseEPPlusFun useepplusfun = new UseEPPlusFun();
    void Awake()
    { 
        string tableName = "RoleTable1";
        tableData = useepplusfun.FindExcelFiles(tableName); //获取武将数据表
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 购买后背景鲜红
    /// </summary>
    public void ChangeBtnColor()
    {
        GetExcelFile1();
        if (money >= price)
        {
            boolIndex = true;
            transform.Find("Image").gameObject.SetActive(true);
            //print(btnTag);
            money = money - price;
            GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>().money = money;
            transform.GetComponent<Button>().enabled = false;   //点击事件取消
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
        btnTag = int.Parse(btn.name);   //获取到招募卡牌的随机数字
        mycard = GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>().myCard;//拿到脚本CreateAndUpdate中的myCard
        money = GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>().money;
        //GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>().getCard.Add(btnNum);
        //for (int i = 0; i < GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>().sendCardId.Count; i++)
        //{
        //    print(GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>().sendCardId[i]);
        //}
    }


    List<string> heroData = new List<string>();
    /// <summary>
    /// 购买成功后更新得到的武将信息
    /// </summary>
    public void UpdateGetCard()
    {
        //getCard = GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>().getCard;
        if (boolIndex == true)
        {
            txt.GetComponent<Text>().text = txt.GetComponent<Text>().text + getCard[getCard.Count - 1].ToString() + "  ";

            //根据武将的ID获取他的所有信息，heroId计算的行值为  Index = ID - ( ID/10 ) - 8
            int index = heroId - (heroId / 10) - 8;
            for (int i = 1; i < 21; i++)
            {
                //存储英雄所有数值
                heroData.Add(tableData.worksheet.Cells[index, i].Value.ToString());
            }
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
            Instantiate(hero_Card, preparation.GetChild(num));
            hero_Card.transform.position = new Vector3(0, 0, 0);
            hero_Card.transform.SetParent(preparation.GetChild(num));
            hero_Card.transform.position = preparation.GetChild(num).position;
            hero_Card.GetComponent<HeroDataControll>().heroData = heroData;
            heroData.Clear();
        }
    }
    //读表
    void GetExcelFile1()
    {
        //string filePath = "F:/dev/GameCommon/111.xlsx";   //绝对路径
        string filePath = Application.streamingAssetsPath + "\\TableFiles\\111.xlsx";
        FileInfo fileinfo = new FileInfo(filePath);
        using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))   //using用来强行做资源释放
        {
            ExcelWorksheet worksheet_111file = excelpackge.Workbook.Worksheets[1];
            GetHeroId(btnTag, worksheet_111file);   //获取武将的ID

            GetSpecificValue(heroId, tableData.worksheet, "recruitingMoney");
            //print(price);
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
