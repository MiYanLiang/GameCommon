using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using OfficeOpenXml;    //引入使用EPPlus类库

public class ChangeAndGet : MonoBehaviour {
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
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
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
        btnNum =btn.GetComponentInChildren<Text>().text;
        btnTag = int.Parse(btn.name);
        mycard = GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>().myCard;//拿到脚本CreateAndUpdate中的myCard
        money = GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>().money;
        //GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>().getCard.Add(btnNum);
        //for (int i = 0; i < GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>().sendCardId.Count; i++)
        //{
        //    print(GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>().sendCardId[i]);
        //}
    }
    public void UpdateGetCard()
    {
        //getCard = GameObject.FindWithTag("Back").GetComponent<CreateAndUpdate>().getCard;
        if (boolIndex == true)
        {
            txt.GetComponent<Text>().text = txt.GetComponent<Text>().text + getCard[getCard.Count - 1].ToString() + "  ";
        }
    }
   //读表
    void GetExcelFile1()
    {
        string filePath = "F:/dev/GameCommon/111.xlsx";   //绝对路径
        FileInfo fileinfo = new FileInfo(filePath);
        using (ExcelPackage excelpackge = new ExcelPackage(fileinfo))   //using用来强行做资源释放
        {
            ExcelWorksheet worksheet1 = excelpackge.Workbook.Worksheets[1];
            ExcelWorksheet worksheet2 = excelpackge.Workbook.Worksheets[2];
            GetHeroId(btnTag,worksheet2);
            GetSpecificValue(heroId, worksheet1, "recruitingMoney");
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
                    heroId = int.Parse(worksheet.Cells[i,1].Value.ToString());
                }
            }
        }
    }
}
